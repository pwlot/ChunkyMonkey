using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ChunkyMonkey.Unity
{
    public sealed class ChunkyMonkeyWindow : EditorWindow
    {
        private Vector2 scroll;
        private Report report;
        private BridgeStatus bridge;
        private string bridgeMessage;

        [MenuItem("Tools/ChunkyMonkey/Repo Doctor")]
        public static void Open()
        {
            GetWindow<ChunkyMonkeyWindow>("ChunkyMonkey");
        }

        private void OnEnable()
        {
            Refresh();
        }

        private void OnGUI()
        {
            if (report == null)
            {
                Refresh();
            }

            EditorGUILayout.Space(8);
            EditorGUILayout.LabelField("ChunkyMonkey Unity Tools", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(report.ProjectRoot, EditorStyles.miniLabel);
            EditorGUILayout.Space(6);
            DrawBridge();
            EditorGUILayout.Space(6);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Refresh", GUILayout.Height(28))) Refresh();
                if (GUILayout.Button("Apply .gitignore", GUILayout.Height(28))) ApplyGitIgnore();
                if (GUILayout.Button("Apply LFS Rules", GUILayout.Height(28))) ApplyLfsRules();
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(!bridge.Found))
                {
                    if (GUILayout.Button("Open in ChunkyMonkey", GUILayout.Height(28))) OpenChunkyMonkey();
                }

                if (GUILayout.Button("Download ChunkyMonkey", GUILayout.Height(28))) DownloadChunkyMonkey();
            }

            EditorGUILayout.Space(8);
            scroll = EditorGUILayout.BeginScrollView(scroll);
            DrawSummary();
            DrawSection("Missing .meta files", report.MissingMeta);
            DrawSection("Generated folders present", report.GeneratedFolders);
            DrawSection("Missing .gitignore rules", report.MissingIgnoreRules);
            DrawSection("Missing .gitattributes LFS rules", report.MissingLfsRules);
            DrawSection("Scan warnings", report.ScanWarnings);
            DrawSection("Large untracked assets", report.LargeUntrackedAssets);
            EditorGUILayout.EndScrollView();
        }

        private void DrawSummary()
        {
            EditorGUILayout.LabelField("Status", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                $"Git repo: {YesNo(report.IsGitRepo)}\n" +
                $"Missing .meta: {report.MissingMeta.Count}\n" +
                $"Generated folders: {report.GeneratedFolders.Count}\n" +
                $"Missing ignore rules: {report.MissingIgnoreRules.Count}\n" +
                $"Missing LFS rules: {report.MissingLfsRules.Count}\n" +
                $"Scan warnings: {report.ScanWarnings.Count}\n" +
                $"Large untracked assets: {report.LargeUntrackedAssets.Count}",
                report.HasWarnings ? MessageType.Warning : MessageType.Info);
        }

        private static void DrawSection(string title, IReadOnlyList<string> rows)
        {
            EditorGUILayout.Space(8);
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            if (rows.Count == 0)
            {
                EditorGUILayout.LabelField("OK", EditorStyles.miniLabel);
                return;
            }

            foreach (var row in rows.Take(80))
            {
                EditorGUILayout.SelectableLabel(row, EditorStyles.miniLabel, GUILayout.Height(18));
            }

            if (rows.Count > 80)
            {
                EditorGUILayout.LabelField($"+ {rows.Count - 80} more", EditorStyles.miniLabel);
            }
        }

        private void Refresh()
        {
            report = RepoScanner.Scan(ProjectRoot());
            bridge = ChunkyMonkeyBridge.Detect();
            bridgeMessage = string.Empty;
            Repaint();
        }

        private void DrawBridge()
        {
            var status = bridge.Found ? $"Found: {bridge.Path}" : "Not found";
            if (!string.IsNullOrWhiteSpace(bridgeMessage))
            {
                status += Environment.NewLine + bridgeMessage;
            }

            EditorGUILayout.HelpBox($"ChunkyMonkey desktop: {status}", bridge.Found ? MessageType.Info : MessageType.Warning);
        }

        private void ApplyGitIgnore()
        {
            FileRules.AppendMissing(Path.Combine(ProjectRoot(), ".gitignore"), ChunkyMonkeyWindowRules.IgnoreRules);
            AssetDatabase.Refresh();
            Refresh();
        }

        private void ApplyLfsRules()
        {
            FileRules.AppendMissing(
                Path.Combine(ProjectRoot(), ".gitattributes"),
                ChunkyMonkeyWindowRules.LfsPatterns.Select(pattern => $"{pattern} filter=lfs diff=lfs merge=lfs -text"));
            AssetDatabase.Refresh();
            Refresh();
        }

        private void OpenChunkyMonkey()
        {
            var root = ProjectRoot();
            var result = ChunkyMonkeyBridge.Open(bridge, root);
            bridgeMessage = result.Ok ? "Opened for this Unity project." : result.Output;
            Repaint();
        }

        private static void DownloadChunkyMonkey()
        {
            Application.OpenURL("https://chunkymonkey.dev/#download");
        }

        private static string ProjectRoot()
        {
            return Directory.GetParent(Application.dataPath)?.FullName ?? Application.dataPath;
        }

        private static string YesNo(bool value)
        {
            return value ? "yes" : "no";
        }
    }

    internal static class RepoScanner
    {
        private const long LargeAssetBytes = 50L * 1024L * 1024L;
        private static readonly string[] GeneratedFolders = { "Library", "Temp", "Obj", "Build", "Builds", "Logs", "UserSettings" };

        public static Report Scan(string root)
        {
            var scanWarnings = new List<string>();
            return new Report
            {
                ProjectRoot = root,
                IsGitRepo = Directory.Exists(Path.Combine(root, ".git")),
                MissingMeta = MissingMeta(root),
                GeneratedFolders = GeneratedFolders.Where(name => Directory.Exists(Path.Combine(root, name))).ToList(),
                MissingIgnoreRules = FileRules.Missing(Path.Combine(root, ".gitignore"), ChunkyMonkeyWindowRules.IgnoreRules),
                MissingLfsRules = FileRules.Missing(Path.Combine(root, ".gitattributes"), ChunkyMonkeyWindowRules.LfsRules),
                ScanWarnings = scanWarnings,
                LargeUntrackedAssets = LargeUntracked(root, scanWarnings)
            };
        }

        private static List<string> MissingMeta(string root)
        {
            var assets = Path.Combine(root, "Assets");
            if (!Directory.Exists(assets)) return new List<string>();

            return Directory.EnumerateFileSystemEntries(assets, "*", SearchOption.AllDirectories)
                .Where(path => !path.EndsWith(".meta", StringComparison.OrdinalIgnoreCase))
                .Where(path => !File.Exists(path + ".meta"))
                .Select(path => Rel(root, path))
                .Take(200)
                .ToList();
        }

        private static List<string> LargeUntracked(string root, List<string> scanWarnings)
        {
            var result = ProcessLauncher.Capture("git", "status --porcelain=v1 -z --untracked-files=all", root);
            if (!result.Ok)
            {
                var detail = string.IsNullOrWhiteSpace(result.Output) ? "git status failed." : result.Output.Trim();
                scanWarnings.Add($"Large untracked asset check skipped: {detail}");
                return new List<string>();
            }

            return result.Output
                .Split(new[] { '\0' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(row => row.StartsWith("?? ", StringComparison.Ordinal))
                .Select(row => row.Substring(3))
                .Where(path => IsLargeFile(Path.Combine(root, path)))
                .Select(path => path.Replace('\\', '/'))
                .Take(100)
                .ToList();
        }

        private static bool IsLargeFile(string path)
        {
            try
            {
                return File.Exists(path) && new FileInfo(path).Length >= LargeAssetBytes;
            }
            catch (IOException)
            {
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        private static string Rel(string root, string path)
        {
            return path.Substring(root.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Length + 1).Replace('\\', '/');
        }
    }

    internal static class ChunkyMonkeyWindowRules
    {
        public static readonly string[] IgnoreRules =
        {
            "[Ll]ibrary/",
            "[Tt]emp/",
            "[Oo]bj/",
            "[Bb]uild/",
            "[Bb]uilds/",
            "[Ll]ogs/",
            "[Uu]ser[Ss]ettings/"
        };

        public static readonly string[] LfsRules =
        {
            "*.psd filter=lfs diff=lfs merge=lfs -text",
            "*.psb filter=lfs diff=lfs merge=lfs -text",
            "*.fbx filter=lfs diff=lfs merge=lfs -text",
            "*.blend filter=lfs diff=lfs merge=lfs -text",
            "*.wav filter=lfs diff=lfs merge=lfs -text",
            "*.mp3 filter=lfs diff=lfs merge=lfs -text",
            "*.ogg filter=lfs diff=lfs merge=lfs -text",
            "*.mp4 filter=lfs diff=lfs merge=lfs -text",
            "*.mov filter=lfs diff=lfs merge=lfs -text",
            "*.unitypackage filter=lfs diff=lfs merge=lfs -text",
            "*.exr filter=lfs diff=lfs merge=lfs -text",
            "*.tga filter=lfs diff=lfs merge=lfs -text"
        };

        public static readonly string[] LfsPatterns =
        {
            "*.psd",
            "*.psb",
            "*.fbx",
            "*.blend",
            "*.wav",
            "*.mp3",
            "*.ogg",
            "*.mp4",
            "*.mov",
            "*.unitypackage",
            "*.exr",
            "*.tga"
        };
    }

    internal static class FileRules
    {
        public static List<string> Missing(string path, IEnumerable<string> expected)
        {
            var existing = ExistingRules(path);
            return expected.Where(rule => !existing.Contains(Normalize(rule))).ToList();
        }

        public static void AppendMissing(string path, IEnumerable<string> expected)
        {
            var missing = Missing(path, expected).ToList();
            if (missing.Count == 0) return;

            var prefix = File.Exists(path) && new FileInfo(path).Length > 0 ? Environment.NewLine : string.Empty;
            File.AppendAllText(path, prefix + "# ChunkyMonkey" + Environment.NewLine + string.Join(Environment.NewLine, missing) + Environment.NewLine);
        }

        private static HashSet<string> ExistingRules(string path)
        {
            if (!File.Exists(path)) return new HashSet<string>();
            return new HashSet<string>(
                File.ReadAllLines(path)
                    .Select(line => line.Trim())
                    .Where(line => line.Length > 0 && !line.StartsWith("#", StringComparison.Ordinal))
                    .Select(Normalize));
        }

        private static string Normalize(string line)
        {
            return line
                .Trim()
                .Trim('/')
                .ToLowerInvariant()
                .Replace("[ll]", "l")
                .Replace("[tt]", "t")
                .Replace("[oo]", "o")
                .Replace("[bb]", "b")
                .Replace("[uu]", "u")
                .Replace("[ss]", "s");
        }
    }

    internal static class ProcessLauncher
    {
        public static CommandResult Capture(string fileName, string arguments, string workingDirectory, int timeoutMilliseconds = 30000)
        {
            try
            {
                using (var process = Process.Start(new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    WorkingDirectory = workingDirectory,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }))
                {
                    if (process == null) return CommandResult.Fail("");
                    var outputTask = process.StandardOutput.ReadToEndAsync();
                    var errorTask = process.StandardError.ReadToEndAsync();
                    if (!process.WaitForExit(timeoutMilliseconds))
                    {
                        process.Kill();
                        return CommandResult.Fail($"{fileName} timed out after {timeoutMilliseconds / 1000}s.");
                    }

                    return process.ExitCode == 0
                        ? CommandResult.Success(outputTask.Result)
                        : CommandResult.Fail(errorTask.Result);
                }
            }
            catch (Exception error)
            {
                return CommandResult.Fail(error.Message);
            }
        }
    }

    internal static class ChunkyMonkeyBridge
    {
        public static BridgeStatus Detect()
        {
            var candidates = CandidatePaths().Where(File.Exists).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            return candidates.Count == 0
                ? BridgeStatus.NotFound()
                : BridgeStatus.FoundAt(candidates[0]);
        }

        public static CommandResult Open(BridgeStatus bridge, string projectRoot)
        {
            if (!bridge.Found) return CommandResult.Fail("Install ChunkyMonkey desktop first, or use Download ChunkyMonkey.");

            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = bridge.Path,
                    Arguments = $"--repo \"{projectRoot}\"",
                    WorkingDirectory = projectRoot,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                return process == null
                    ? CommandResult.Fail("ChunkyMonkey did not start.")
                    : CommandResult.Success("");
            }
            catch (Exception error)
            {
                Debug.Log($"ChunkyMonkey launch failed: {error.Message}");
                return CommandResult.Fail($"Could not open ChunkyMonkey: {error.Message}");
            }
        }

        private static IEnumerable<string> CandidatePaths()
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (!string.IsNullOrWhiteSpace(localAppData))
            {
                yield return Path.Combine(localAppData, "Programs", "ChunkyMonkey", "ChunkyMonkey.exe");
                yield return Path.Combine(localAppData, "Programs", "ChunkyMonkey", "chunkymonkey.exe");
            }

            foreach (var directory in PathDirectories())
            {
                yield return Path.Combine(directory, "ChunkyMonkey.exe");
                yield return Path.Combine(directory, "chunkymonkey.exe");
            }
        }

        private static IEnumerable<string> PathDirectories()
        {
            var path = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            return path
                .Split(new[] { Path.PathSeparator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(entry => entry.Trim().Trim('"'))
                .Where(Directory.Exists);
        }
    }

    internal sealed class BridgeStatus
    {
        public bool Found;
        public string Path;

        public static BridgeStatus FoundAt(string path)
        {
            return new BridgeStatus { Found = true, Path = path };
        }

        public static BridgeStatus NotFound()
        {
            return new BridgeStatus { Found = false, Path = string.Empty };
        }
    }

    internal sealed class CommandResult
    {
        public bool Ok { get; private set; }
        public string Output { get; private set; }

        public static CommandResult Success(string output)
        {
            return new CommandResult { Ok = true, Output = output ?? string.Empty };
        }

        public static CommandResult Fail(string output)
        {
            return new CommandResult { Ok = false, Output = output ?? string.Empty };
        }
    }

    internal sealed class Report
    {
        public string ProjectRoot;
        public bool IsGitRepo;
        public List<string> MissingMeta = new List<string>();
        public List<string> GeneratedFolders = new List<string>();
        public List<string> MissingIgnoreRules = new List<string>();
        public List<string> MissingLfsRules = new List<string>();
        public List<string> ScanWarnings = new List<string>();
        public List<string> LargeUntrackedAssets = new List<string>();

        public bool HasWarnings
        {
            get
            {
                return MissingMeta.Count > 0 ||
                    GeneratedFolders.Count > 0 ||
                    MissingIgnoreRules.Count > 0 ||
                    MissingLfsRules.Count > 0 ||
                    ScanWarnings.Count > 0 ||
                    LargeUntrackedAssets.Count > 0;
            }
        }
    }
}
