namespace Microsoft.Content.Build.Java2Yaml
{
    public class ArtifactItem
    {
        public string GroupId { get; set; }

        public string ArtifactId { get; set; }

        // Type of an artifact (war,jar,etc)
        public string Type { get; set; }

        public string Classifier { get; set; }

        public string Version { get; set; }

        public string Scope { get; set; }
    }
}