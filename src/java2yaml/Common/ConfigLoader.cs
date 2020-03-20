namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;

    public static class ConfigLoader
    {
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
                var pak = new ConfigModel(packageConfigPath, config);
                pak.InputPaths = folderList;
                pak.ExcludePaths = excludePaths;

                configs.Add(pak);
                
            }

            return configs;
        }

        public static List<string> LoadRepositoryList(string repoListPath)
        {
            var repos = JsonConvert.DeserializeObject<RepositoryModel>(File.ReadAllText(repoListPath)).Repository;

            var list = (from p in repos
                        let FolderName = Path.Combine(Constants.Src, p.FolderName)
                        select PathUtility.TransformPath(repoListPath, FolderName))
                .ToList();

            return list;
        }

        private static List<string> LoadPackageFolders(string configPath, PackageBasedConfigModel packageBasedConfig)
        {
            return (from p in packageBasedConfig.Packages
                    let FolderName = Path.Combine(Constants.Src, packageBasedConfig.OutputPath, p.ArtifactId)
                    select PathUtility.TransformPath(configPath, FolderName))
            .ToList();
        }

        private static List<string> LoadExcludePaths(string configPath, PackageBasedConfigModel packageConfig)
        {
            return (from p in packageConfig.Packages
                    where p.ExcludePaths != null
                    from e in p.ExcludePaths
                    let FolderName = Path.Combine(Constants.Src, packageConfig.OutputPath, p.ArtifactId, e)
                    select PathUtility.TransformPath(configPath, FolderName))
            .ToList();
        }
    }
}
