using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChunkyMonkey.Unity
{
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
}
