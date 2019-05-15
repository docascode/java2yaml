namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    [Serializable]
    public class ConfigModel
    {
        [JsonProperty(Constants.InputPaths, Required = Required.DisallowNull)]
        public List<string> InputPaths { get; set; }

        [JsonProperty(Constants.OutputPath, Required = Required.DisallowNull)]
        public string OutputPath { get; set; }

        [JsonProperty(Constants.Language)]
        public string Language { get; set; } = "java";

        [JsonProperty(Constants.ExcludePaths)]
        public List<string> ExcludePaths { get; set; }

        public List<string> RepositoryFolders { get; set; }
    }
}
