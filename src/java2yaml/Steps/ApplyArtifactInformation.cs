namespace Microsoft.Content.Build.Java2Yaml
{
    using System.IO;
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
                
                var yamlFiles = Directory.GetFiles(Path.Combine(RepositoryPath, Constants.Doc));

                foreach (var yamlFile in yamlFiles)
                {
                    // Artifact infomation does not apply to toc file
                    if (!yamlFile.EndsWith("toc.yml"))
                    {
                        var ymlFileModel = YamlUtility.Deserialize<YamlFileModel>(PathUtility.ResolveLongPath(yamlFile));

                        // Only add artifact infomation for items declared in class 
                        foreach (var item in ymlFileModel.Items)
                        {
                            item.Artifact = GetArtifctStringPerPackage();
                        }

                        YamlUtility.Serialize(yamlFile, ymlFileModel, YamlMime.ManagedReference);
                    }
                }
            });
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
