namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Threading;

    public static class ProcessUtility
    {
        public static void Execute(
            string fileName,
            string commandLineArgs,
            string cwd = null,
            bool redirectOutput = true,
            int timeoutInMinutes = 20)
        {
            Guard.ArgumentNotNullOrEmpty(fileName, nameof(fileName));
            Guard.ArgumentNotNullOrEmpty(commandLineArgs, nameof(commandLineArgs));

            var error = new StringBuilder();
            var output = new StringBuilder();

            var psi = new ProcessStartInfo
            {
                FileName = fileName,
                WorkingDirectory = cwd,
                Arguments = commandLineArgs,
                UseShellExecute = false,
            };

            if (redirectOutput)
            {
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
            }

            using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
            using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
            {
                var process = Process.Start(psi);

                process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        outputWaitHandle.Set();
                    }
                    else
                    {
                        output.AppendLine(e.Data);
                    }
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        errorWaitHandle.Set();
                    }
                    else
                    {
                        error.AppendLine(e.Data);
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                int timeout = ConvertMinuteToMillisecond(timeoutInMinutes);

                if (process.WaitForExit(timeout) &&
                    outputWaitHandle.WaitOne(timeout) &&
                    errorWaitHandle.WaitOne(timeout))
                {
                    if (process.ExitCode == 0 && error.Length == 0)
                    {
                        Console.WriteLine(output);
                    }
                    else
                    {
                        var message = $"'\"{fileName}\" {commandLineArgs}' failed in directory '{cwd}' with exit code {process.ExitCode}: \nSTDOUT:'{output}'\nSTDERR:\n'{error}'";
                        ExceptionUtility.ThrowExceptionForAzureDevops(fileName, commandLineArgs, output, error);                      
                        throw new InvalidOperationException(message);
                    }
                }
                else
                {
                    var message = $"'\"{fileName}\" {commandLineArgs}' failed in directory '{cwd}'. Reached {timeoutInMinutes} minutes timeout.";
                    ExceptionUtility.ThrowExceptionForAzureDevops(message);
                    throw new InvalidOperationException(message);
                }
            }
        }

        private static int ConvertMinuteToMillisecond(int minute)
        {
            return minute * 60 * 1000;
        }
    }
}
