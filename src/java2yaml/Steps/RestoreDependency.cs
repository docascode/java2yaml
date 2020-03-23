namespace Microsoft.Content.Build.Java2Yaml
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

        // This is profiles to exclude unpublished test jar packages when download dependencies
        // For some packages, their test jar does not publish to Maven Central but they cannot be removed from pom
        // But copy-dependencies must go through test scope, so work around by putting test jar to a profile, and skip this profile
        public string[] MavenProfileToExclude = new string[1] { "!azure-mgmt-sdk-test-jar" };

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
            string profilesToExclude = string.Join(",", MavenProfileToExclude);

            string commandLineArgs = string.Concat("/c mvn dependency:copy-dependencies -P ", profilesToExclude, " -DoutputDirectory=", packagePath);

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
