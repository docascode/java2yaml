namespace Microsoft.Content.Build.Java2Yaml
{
    using System;

    using Newtonsoft.Json;
    using YamlDotNet.Serialization;

    [Serializable]
    public class Return
    {
        [YamlMember(Alias = "type")]
        [JsonProperty("type")]
        public string ReturnType { get; set; }

        [YamlMember(Alias = "description")]
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
