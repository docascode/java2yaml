namespace Microsoft.Content.Build.Java2Yaml
{
    using System;

    using Newtonsoft.Json;
    using YamlDotNet.Serialization;

    [Serializable]
    public class SpecViewModel
    {
        [YamlMember(Alias = "uid")]
        [JsonProperty("uid")]
        public string Uid { get; set; }

        [YamlMember(Alias = "name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [YamlMember(Alias = "fullName")]
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [YamlMember(Alias = "isExternal")]
        [JsonProperty("isExternal")]
        public bool IsExternal { get; set; }
    }
}
