namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Linq;

    public class RestoreDependency : IStep
    {
        public RestoreDependency(string path)
        {
            RepositoryPath = path;
        }
        public string StepName
        {
            get { return "RestoreDependency"; }
        }

        public string RepositoryPath { get; }


        public Task RunAsync(ConfigModel config)
        {
            return Task.Run(() =>
            {
                Guard.ArgumentNotNullOrEmpty(RepositoryPath, nameof(RepositoryPath));

                var projectFiles = FileUtility.GetFilesByName(RepositoryPath, Constants.BuildTool.Maven);
                var packagePath = Path.Combine(RepositoryPath, Constants.PackageFolder);

                if (projectFiles.Count() == 0)
                {
                    ConsoleLogger.WriteLine(new LogEntry
                    {
                        Phase = StepName,
                        Level = LogLevel.Error,
                        Message = " The project file pom.xml cannot be found, exiting..."
                    });

                    Environment.Exit(1);
                }

                ConsoleLogger.WriteLine(new LogEntry
                {
                    Phase = StepName,
                    Level = LogLevel.Info,
                    Message = $"{projectFiles.Count()} pom.xml founded."
                });

                foreach (var file in projectFiles)
                {
                    RunRestoreCommand(packagePath, file.DirectoryName);
                }
            }
         );
        }

        public void RunRestoreCommand(string packagePath, string workingDirectory)
        {
            string commandLineArgs = "/c mvn dependency:copy-dependencies -DoutputDirectory=" + packagePath;

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
