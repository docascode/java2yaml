namespace Microsoft.Content.Build.Java2Yaml
{
    using System;

    using YamlDotNet.Serialization;

    [Serializable]
    public class TocItemYaml
    {
        [YamlMember(Alias = "uid")]
        public string Uid { get; set; }

        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [YamlMember(Alias = "href")]
        public string Href { get; set; }

        [YamlMember(Alias = "type")]
        public string Type { get; set; }

        [YamlMember(Alias = "items")]
        public TocYaml Items { get; set; }
    }
}
