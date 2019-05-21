﻿namespace Microsoft.Content.Build.Java2Yaml
{
    using System.IO;
    using System.Threading.Tasks;
    using System.Linq;

    public class RestoreDependency : IStep
    {
        public RestoreDependency(string path)
        {
            RepositoryPath = path;
        }

        public string StepName => "RestoreDependency";

        public string RepositoryPath { get; }

        public Task RunAsync(ConfigModel config)
        {
            return Task.Run(() =>
            {
                Guard.ArgumentNotNullOrEmpty(RepositoryPath, nameof(RepositoryPath));

                var projectFiles = FileUtility.GetFilesByName(RepositoryPath, Constants.BuildToolConfig.Maven).ToList();
                var packagePath = Path.Combine(RepositoryPath, Constants.PackageFolder);

                if (projectFiles.Count == 0)
                {
                    throw new System.InvalidOperationException("POM.xml not available.");
                }

                ConsoleLogger.WriteLine(new LogEntry
                {
                    Phase = StepName,
                    Level = LogLevel.Info,
                    Message = $" {projectFiles.Count} pom.xml founded."
                });

                foreach (var file in projectFiles)
                {
                    RunRestoreCommand(packagePath, file.DirectoryName);
                }
            }
         );
        }

        private void RunRestoreCommand(string packagePath, string workingDirectory)
        {
            string commandLineArgs = string.Concat("/c mvn dependency:copy-dependencies -DoutputDirectory=", packagePath);

            ConsoleLogger.WriteLine(new LogEntry
            {
                Phase = StepName,
                Level = LogLevel.Info,
                Message = $" Restoring dependency under directory '{workingDirectory}'..."
            });

            ProcessUtility.Execute("cmd.exe", commandLineArgs, workingDirectory);
        }
    }
}