using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChunkyMonkey.GitLfsTools
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
                LargeAssets = FindLargeAssets(root, scanWarnings)
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

        private static List<string> FindLargeAssets(string root, List<string> scanWarnings)
        {
            var assets = Path.Combine(root, "Assets");
            if (!Directory.Exists(assets)) return new List<string>();

            try
            {
                return Directory.EnumerateFiles(assets, "*", SearchOption.AllDirectories)
                    .Where(IsLargeFile)
                    .Select(path => Rel(root, path))
                    .Take(100)
                    .ToList();
            }
            catch (IOException error)
            {
                scanWarnings.Add($"Large asset check skipped: {error.Message}");
                return new List<string>();
            }
            catch (UnauthorizedAccessException error)
            {
                scanWarnings.Add($"Large asset check skipped: {error.Message}");
                return new List<string>();
            }
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
