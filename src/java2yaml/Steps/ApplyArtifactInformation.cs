namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
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
                        var ymlFileModel = YamlUtility.Deserialize<FileYaml>(PathUtility.ResolveLongPath(yamlFile));

                        YamlUtility.Serialize(PathUtility.ResolveLongPath(yamlFile), ymlFileModel, YamlMime.ManagedReference);
                    }
                    catch (Exception)
                    {
                        ConsoleLogger.WriteLine(
                            new LogEntry
                            {
                                Phase = StepName,
                                Level = LogLevel.Error,
                                Message = $"Failed to process {yamlFile}.",
                            });
                        throw;
                    }
                }

                ConsoleLogger.WriteLine(new LogEntry
                {
                    Phase = StepName,
                    Level = LogLevel.Info,
                    Message = $" Artifact added to {yamlFiles.Count} yaml files."
                });
            });
        }
    }
}
