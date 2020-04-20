namespace Microsoft.Content.Build.Java2Yaml
{
    using System;

    using Newtonsoft.Json;
    using YamlDotNet.Serialization;

    [Serializable]
    public class Parameters
    {
        [YamlMember(Alias = "id")]
        [JsonProperty("id")]
        public string ID { get; set; }

        [YamlMember(Alias = "type")]
        [JsonProperty("type")]
        public string Type { get; set; }

        [YamlMember(Alias = "description")]
        [JsonProperty("description")]
        public string Description { get; set; }
    }

    [Serializable]
    public class TypeParameters
    {
        [YamlMember(Alias = "id")]
        [JsonProperty("id")]
        public string ID { get; set; }
    }
}
