namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;
    using YamlDotNet.Serialization;

    [Serializable]
    public class FileItemYaml
    {
        [YamlMember(Alias = "uid")]
        [JsonProperty("uid")]
        public string Uid { get; set; }

        [YamlMember(Alias = "id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [YamlMember(Alias = "artifact")]
        [JsonProperty("artifact")]
        public string Artifact { get; set; }

        [YamlMember(Alias = "parent")]
        [JsonProperty("parent")]
        public string Parent { get; set; }

        [YamlMember(Alias = "children")]
        [JsonProperty("children")]
        public List<string> Children { get; set; }

        [YamlMember(Alias = "langs")]
        [JsonProperty("langs")]
        public string[] Langs { get; set; }

        [YamlMember(Alias = "name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [YamlMember(Alias = "nameWithType")]
        [JsonProperty("nameWithType")]
        public string NameWithType { get; set; }

        [YamlMember(Alias = "fullName")]
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [YamlMember(Alias = "overload")]
        [JsonProperty("overload")]
        public string Overload { get; set; }

        [YamlMember(Alias = "type")]
        [JsonProperty("type")]
        public string Type { get; set; }

        [YamlMember(Alias = "package")]
        [JsonProperty("package")]
        public string Package { get; set; }

        [YamlMember(Alias = "summary")]
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [YamlMember(Alias = "syntax")]
        [JsonProperty("syntax")]
        public Syntax Syntax { get; set; }

        [YamlMember(Alias = "inheritance")]
        [JsonProperty("inheritance")]
        public List<string> Inheritance { get; set; }

        [YamlMember(Alias = "implements")]
        [JsonProperty("implements")]
        public List<string> Implements { get; set; }

        [YamlMember(Alias = "exceptions")]
        [JsonProperty("exceptions")]
        public List<ExceptionItem> Exceptions { get; set; }

        [YamlMember(Alias = "isExternal")]
        [JsonProperty("isExternal")]
        public bool IsExternal { get; set; }

        [YamlMember(Alias = "spec.java")]
        public List<SpecViewModel> SpecForJava { get; set; }

        [YamlMember(Alias = "inheritedMembers")]
        public List<string> InheritedMembers { get; set; }
    }
}