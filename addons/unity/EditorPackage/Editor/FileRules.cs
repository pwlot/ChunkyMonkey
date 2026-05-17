using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChunkyMonkey.Unity
{
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
}
