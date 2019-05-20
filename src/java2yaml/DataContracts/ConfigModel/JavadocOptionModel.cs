namespace Microsoft.Content.Build.Java2Yaml
{
    public class JavadocOptionModel
    {
        public string ClassPath { get; set; }
        public string Encoding => " UTF-8";
        public string DocletPath { get; set; }
        public string Doclet => Constants.DocletClass;
        public string SourcePath { get; set; }
        public string OutputPath { get; set; }
    }
}
