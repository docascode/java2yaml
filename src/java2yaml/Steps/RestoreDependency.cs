namespace Microsoft.Content.Build.Java2Yaml
{
    using System.IO;
    using System.Threading.Tasks;
    using System.Linq;

    public class RestoreDependency : IStep
    {
        public RestoreDependency(PackageConfigModel packageConfig, GradleUtility gradleUtility)
        {
            _packageConfig = packageConfig;
            _gradleUtility = gradleUtility;
        }

        public string StepName => "RestoreDependency";

        private PackageConfigModel _packageConfig { get; }
        private GradleUtility _gradleUtility { get; }

        // This is profiles to exclude unpublished test jar packages when download dependencies
        // For some packages, their test jar does not publish to Maven Central but they cannot be removed from pom
        // But copy-dependencies must go through test scope, so work around by putting test jar to a profile, and skip this profile
        public string[] MavenProfileToExclude = new string[1] { "!azure-mgmt-sdk-test-jar" };

        public Task RunAsync(ConfigModel config)
        {
            return Task.Run(() =>
            {
                Guard.ArgumentNotNull(_packageConfig, nameof(_packageConfig));

                var projectFiles = FileUtility.GetFilesByName(_packageConfig.RepositoryFolder, Constants.BuildToolConfig.Maven).ToList();
                var package = string.Concat(_packageConfig.Package.GroupId, ":"
                    , _packageConfig.Package.ArtifactId, ":"
                    , _packageConfig.Package.PackageVersion);

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
                    RunRestoreCommand(package, file.DirectoryName, _gradleUtility);
                }
            }
         );
        }

        private void RunRestoreCommand(string package, string workingDirectory, GradleUtility gradleUtility)
        {
            string profilesToExclude = string.Join(",", MavenProfileToExclude);

            gradleUtility.GenerateBuilFile(package, workingDirectory);

            string commandLineArgs = "/c gradle getDeps";

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
