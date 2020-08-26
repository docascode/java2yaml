namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.DocAsCode.Common;

    public class ApplyArtifactInformation : IStep
    {
        public string StepName => "ApplyArtifactInformation";

        public string RepositoryPath { get; }
        public Package Package { get; }

        public ApplyArtifactInformation(string path, Package package)
        {
            RepositoryPath = path;
            Package = package;
        }

        public Task RunAsync(ConfigModel config)
        {
            return Task.Run(() =>
            {
                Guard.ArgumentNotNullOrEmpty(RepositoryPath, nameof(RepositoryPath));

                var yamlFiles = Directory.GetFiles(Path.Combine(RepositoryPath, Constants.Doc))
                .Where(item => !item.EndsWith("toc.yml")) // Artifact infomation does not apply to toc file
                .ToList(); 

                foreach (var yamlFile in yamlFiles)
                {
                    try
                    {
                        var ymlFile = PathUtility.ResolveLongPath(yamlFile);
                        string art = "artifact: "+(GetArtifctStringPerPackage());

                        File.AppendAllText(ymlFile, art + Environment.NewLine);

                        ValidateYaml(ymlFile);
                    }
                    catch (Exception)
                    {
                        ConsoleLogger.WriteLine(
                            new LogEntry
                            {
                                Phase = StepName,
                                Level = LogLevel.Error,
                                Message = $"Fail to append artifact for {yamlFile}.",
                            });
                        throw;
                    }
                }

                ConsoleLogger.WriteLine(new LogEntry
                {
                    Phase = StepName,
                    Level = LogLevel.Info,
                    Message = $" Artifact added for {yamlFiles.Count} yaml files."
                });
            });
        }

        private void ValidateYaml(string ymlFile)
        {
            try
            {
                YamlUtility.Deserialize<Dictionary<string, object>>(ymlFile);
            }
            catch (Exception)
            {
                ConsoleLogger.WriteLine(
                    new LogEntry
                    {
                        Phase = StepName,
                        Level = LogLevel.Error,
                        Message = $"Invalid yaml after add artifact.",
                    });
                throw;
            }
        }

        string GetArtifctStringPerPackage()
        {
            // GroupId:ArtifactId:Version
            return string.Concat(Package.GroupId, ":",
                    Package.ArtifactId, ":",
                    Package.PackageVersion);
        }
    }
}
