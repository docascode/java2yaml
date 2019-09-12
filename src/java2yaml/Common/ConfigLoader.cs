namespace Microsoft.Content.Build.Java2Yaml
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;

    public static class ConfigLoader
    {
        public static ConfigModel LoadConfig(string configPath, string repoListPath)
        {
            var config = JsonConvert.DeserializeObject<ConfigModel>(File.ReadAllText(configPath));

            config.InputPaths = (from p in config.InputPaths
                                 select TransformPath(configPath, p)).ToList();

            config.OutputPath = TransformPath(configPath, config.OutputPath);

            if (config.ExcludePaths != null)
            {
                config.ExcludePaths = (from p in config.ExcludePaths
                                       select TransformPath(configPath, p)).ToList();
            }

            config.RepositoryFolders = LoadRepositoryList(repoListPath);

            if (string.IsNullOrEmpty(config.OutputPath) || string.IsNullOrWhiteSpace(config.OutputPath))
            {
                throw new InvalidDataException($"Invalid \"{Constants.OutputPath}\" in {configPath}");
            }

            return config;
        }

        public static ConfigModel LoadConfig(string packageConfigPath)
        {
            var packageConfig = JsonConvert.DeserializeObject<PackageBasedConfigModel>(File.ReadAllText(packageConfigPath));

            var folderList = LoadPackageFolders(packageConfigPath, packageConfig);

            var excludePaths = LoadExcludePaths(packageConfigPath, packageConfig);

            // the path of unzipped -soruce.jar will be consider as inputPath, as it contains all the .java files we need to document for each artifact.
            var config = new ConfigModel
            {
                InputPaths = folderList,
                OutputPath = TransformPath(packageConfigPath, packageConfig.OutputPath),
                ExcludePaths = excludePaths,
                RepositoryFolders = folderList
            };

            return config;
        }

        public static List<string> LoadRepositoryList(string repoListPath)
        {
            var repos = JsonConvert.DeserializeObject<RepositoryModel>(File.ReadAllText(repoListPath)).Repository;

            var list = (from p in repos
                        let FolderName = string.Concat(Constants.Src, p.FolderName)
                        select TransformPath(repoListPath, FolderName))
                .ToList();

            return list;
        }

        private static List<string> LoadPackageFolders(string configPath, PackageBasedConfigModel packageConfig)
        {
            return (from p in packageConfig.Packages
                    let FolderName = string.Concat(Constants.Src, p.ArtifactId)
                    select TransformPath(configPath, FolderName))
            .ToList();
        }

        private static List<string> LoadExcludePaths(string configPath, PackageBasedConfigModel packageConfig)
        {
            return (from p in packageConfig.Packages
                    where p.ExcludePaths != null
                    from e in p.ExcludePaths
                    let FolderName = string.Concat(Constants.Src, p.ArtifactId, e)
                    select TransformPath(configPath, FolderName))
            .ToList();
        }

        private static string TransformPath(string configPath, string path)
        {
            return PathUtility.IsRelativePath(path) ? PathUtility.GetAbsolutePath(configPath, path) : path;
        }
    }
}
