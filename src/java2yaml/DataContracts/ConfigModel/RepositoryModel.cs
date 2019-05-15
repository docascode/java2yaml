namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    [Serializable]
    public class RepositoryModel
    {
        [JsonProperty(Constants.Repo)]
        public List<Repository> Repository { get; set; }
    }

    public class Repository
    {
        [JsonProperty(Constants.FolderName)]
        public string FolderName { get; set; }

        public string ClassPath { get; set; }
    }

}
