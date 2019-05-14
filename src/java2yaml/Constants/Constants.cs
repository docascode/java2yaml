namespace Microsoft.Content.Build.Java2Yaml
{
    public static class Constants
    {
        public const string ConfigFileName = "Code2Yaml.json";
        public const string RepoListFileName = "Repo.json";

        public const string Config = "config";
        public const string InputPaths = "input_paths";
        public const string OutputPath = "output_path";
        public const string ExcludePaths = "exclude_paths";
        public const string Language = "language";
        public const string Repo = "repo";
        public const string FolderName = "name";

        public const string Src = @"src\";
        public const string PackageFolder = @"packages";

        public static class BuildTool
        {
            const string Maven = "pom.xml";
        }

        public static class JavadocArgumentModel
        {
            public const string ClassPath = "-classpath";
            public const string Encoding = "-encoding";
            public const string DocletPath = "-docletpath";
            public const string Doclet = "-doclet";
            public const string SourcePath = "-sourcepath";
            public const string OutputPath = "-outputpath";
            public const string SubPackages = "-subpackages";
        }
    }
}
