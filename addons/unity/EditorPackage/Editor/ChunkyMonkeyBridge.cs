using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ChunkyMonkey.Unity
{
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
}
