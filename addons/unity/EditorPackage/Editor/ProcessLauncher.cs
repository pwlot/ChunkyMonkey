using System;
using System.Diagnostics;

namespace ChunkyMonkey.Unity
{
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
}
