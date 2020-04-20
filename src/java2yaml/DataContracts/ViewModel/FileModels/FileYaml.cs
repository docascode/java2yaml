namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;
    using YamlDotNet.Serialization;


    [Serializable]
    public class FileYaml
    {
        [YamlMember(Alias = "items")]
        [JsonProperty("items")]
        public List<FileItemYaml> Items { get; set; }

        [YamlMember(Alias = "references")]
        [JsonProperty("references")]
        public List<FileItemYaml> References { get; set; }
    }
}
