namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;

    public static class ConfigLoader
    {
        // Load configuration from code2yaml.json and repo.json
        public static ConfigModel LoadConfig(string configPath, string repoListPath)
        {
            if (string.IsNullOrWhiteSpace(configPath) || string.IsNullOrWhiteSpace(repoListPath))
            {
                throw new ArgumentException($"{nameof(configPath)} or {nameof(repoListPath)} cannot be null or empty.");
            }

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

            if (string.IsNullOrWhiteSpace(config.OutputPath))
            {
                throw new InvalidDataException($"Invalid \"{Constants.OutputPath}\" in {configPath}");
            }

            return config;
        }

        // Load configuration from package.json
        public static List<ConfigModel> LoadConfig(string packageConfigPath)
        {
            if (string.IsNullOrWhiteSpace(packageConfigPath))
            {
                throw new ArgumentException($"{packageConfigPath} cannot be null or empty.");
            }

            var configs = new List<ConfigModel>();

            var packageBasedConfigs = JsonConvert.DeserializeObject<List<PackageBasedConfigModel>>(File.ReadAllText(packageConfigPath));

            foreach(var config in packageBasedConfigs)
            {
                var folderList = LoadPackageFolders(packageConfigPath, config);

                var excludePaths = LoadExcludePaths(packageConfigPath, config);

                // the path of unzipped -soruce.jar will be consider as inputPath, as it contains all the .java files we need to document for each artifact.
                configs.Add(new ConfigModel
                {
                    InputPaths = folderList,
                    OutputPath = TransformPath(packageConfigPath, config.OutputPath),
                    ExcludePaths = excludePaths,
                    RepositoryFolders = folderList
                });               
            }

            return configs;
        }

        public static List<string> LoadRepositoryList(string repoListPath)
        {
            var repos = JsonConvert.DeserializeObject<RepositoryModel>(File.ReadAllText(repoListPath)).Repository;

            var list = (from p in repos
                        let FolderName = Path.Combine(Constants.Src, p.FolderName)
                        select TransformPath(repoListPath, FolderName))
                .ToList();

            return list;
        }

        private static List<string> LoadPackageFolders(string configPath, PackageBasedConfigModel packageBasedConfig)
        {
            return (from p in packageBasedConfig.Packages
                    let FolderName = Path.Combine(Constants.Src, packageBasedConfig.OutputPath, p.ArtifactId)
                    select TransformPath(configPath, FolderName))
            .ToList();
        }

        private static List<string> LoadExcludePaths(string configPath, PackageBasedConfigModel packageConfig)
        {
            return (from p in packageConfig.Packages
                    where p.ExcludePaths != null
                    from e in p.ExcludePaths
                    let FolderName = Path.Combine(Constants.Src, packageConfig.OutputPath, p.ArtifactId, e)
                    select TransformPath(configPath, FolderName))
            .ToList();
        }

        private static string TransformPath(string configPath, string path)
        {
            return PathUtility.IsRelativePath(path) ? PathUtility.GetAbsolutePath(configPath, path) : path;
        }
    }
}
