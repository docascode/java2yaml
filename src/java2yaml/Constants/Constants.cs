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
        public const string Doc = @"_javadoc";
        public const string PackageFolder = @"packages";

        public const string DocletClass = "com.microsoft.doclet.DocFxDoclet";
        public const string DocletLocation = @"tools\docfx-doclet.jar";
        public const string Files = @"files";
        public const string Options = @"options";

        public static class Extensions
        {
            public const string Jar = "jar";
            public const string Java = "java";
        }

        public static class BuildTool
        {
            public const string Maven = "pom.xml";
        }
    }
}
