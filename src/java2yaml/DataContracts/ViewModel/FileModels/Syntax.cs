namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;
    using YamlDotNet.Serialization;

    [Serializable]
    public class Syntax
    {
        [YamlMember(Alias = "content")]
        [JsonProperty("content")]
        public string Content { get; set; }

        [YamlMember(Alias = "parameters")]
        [JsonProperty("parameters")]
        public List<Parameters> Parameters { get; set; }

        [YamlMember(Alias = "return")]
        [JsonProperty("return")]
        public Return Return { get; set; }

        [YamlMember(Alias = "typeParameters")]
        [JsonProperty("typeParameters")]
        public List<TypeParameters> TypeParameters { get; set; }
    }
}
