﻿namespace Microsoft.Content.Build.Java2Yaml
{
    public static class Constants
    {
        public const string ConfigFileName = "Code2Yaml.json";
        public const string RepoListFileName = "Repo.json";

        public const string packageConfigFileName = "Package.json";

        public const string Config = "config";
        public const string InputPaths = "input_paths";
        public const string OutputPath = "output_path";
        public const string ExcludePaths = "exclude_paths";
        public const string Language = "language";
        public const string Repo = "repo";
        public const string FolderName = "name";
        public const string ArtifactId = "packageArtifactId";

        public const string Src = @"src\";
        public const string Doc = "_javadoc";
        public const string PackageFolder = "Dependency";

        public const string DocletClass = "com.microsoft.doclet.DocFxDoclet";
        public const string DocletLocation = @"tools\docfx-doclet.jar";
        public const string Files = "files";
        public const string Options = "options";

        public const string Toc = "toc.yml";

        public static class YamlMime
        {
            public const string YamlMimePrefix = "### YamlMime:";
            public const string TableOfContent = YamlMimePrefix + "TableOfContent";
        }

        public static class Extensions
        {
            public const string Jar = "jar";
            public const string Java = "java";
            public const string Yaml = "yml";
        }

        public static class BuildToolConfig
        {
            public const string Maven = "pom.xml";
        }
    }
}
