namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Newtonsoft.Json;

    [Serializable]
    public class ConfigModel
    {
        [JsonProperty(Constants.InputPaths)]
        public List<string> InputPaths { get; set; }

        [JsonProperty(Constants.ExcludePaths)]
        public List<string> ExcludePaths { get; set; }

        [JsonProperty(Constants.OutputPath, Required = Required.DisallowNull)]
        public string OutputPath { get; set; }

        [JsonProperty(Constants.Language)]
        public string Language { get; set; } = "java";

        public List<PackageConfigModel> PackageConfigs { get; set; }
    }

    public class PackageConfigModel
    {
        public string RepositoryFolder { get; set; }

        public Package Package { get; set; }

        public PackageConfigModel(string outputPath, string configPath, Package package)
        {
            RepositoryFolder = PathUtility.TransformPath(configPath, Path.Combine(Constants.Src, outputPath, package.ArtifactId));
            Package = package;
        }
    }
}
