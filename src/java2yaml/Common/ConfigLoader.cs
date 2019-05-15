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

            return config;
        }

        public static List<string> LoadRepositoryList(string repoListPath)
        {
            var repos = JsonConvert.DeserializeObject<RepositoryModel>(File.ReadAllText(repoListPath)).Repository;

            var list = (from p in repo
                        let FolderName = string.Concat(Constants.Src, p.FolderName)
                        select TransformPath(repoListPath, FolderName)).ToList();

            return list;
        }

        private static string TransformPath(string configPath, string path)
        {
            return PathUtility.IsRelativePath(path) ? PathUtility.GetAbsolutePath(configPath, path) : path;
        }
    }
}
