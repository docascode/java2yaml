namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.Diagnostics;
    using System.Text;

    public static class ProcessUtility
    {
        public static void Execute(string fileName, string commandLineArgs, string cwd = null, bool redirectOutput = true)
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

            var process = Process.Start(psi);

            if (redirectOutput)
            {
                output.AppendLine(process.StandardOutput.ReadToEnd());
                error.AppendLine(process.StandardError.ReadToEnd());
            }

            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                Console.WriteLine(output);
            }
            else
            {
                var message = $"'\"{fileName}\" {commandLineArgs}' failed in directory '{cwd}' with exit code {process.ExitCode}: \nSTDOUT:'{output}'\nSTDERR:\n'{error}'";
                throw new InvalidOperationException(message);
            }
        }
    }
}
