namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    [Serializable]

    public class PackageBasedConfigModel
    {
        [JsonProperty(Constants.OutputPath, Required = Required.DisallowNull)]
        public string OutputPath { get; set; }

        [JsonProperty(Constants.Language)]
        public string Language { get; set; } = "java";

        public List<Package> Packages { get; set; }
    }

    public class Package
    {
        [JsonProperty(Constants.ArtifactId, Required = Required.DisallowNull)]
        public string ArtifactId { get; set; }

        [JsonProperty(Constants.InputPaths)]
        public List<string> InputPaths { get; set; }

        [JsonProperty(Constants.ExcludePaths)]
        public List<string> ExcludePaths { get; set; }
    }
}
