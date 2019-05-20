namespace Microsoft.Content.Build.Java2Yaml
{
    using System.IO;
    using System.Threading.Tasks;

    public class CentralizeDocument : IStep
    {
        public string StepName => "CentralizeDocument";

        public Task RunAsync(ConfigModel config)
        {
            return Task.Run(() =>
            {
                var exists = System.IO.Directory.Exists(config.OutputPath);

                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(config.OutputPath);
                }

                foreach (var r in config.RepositoryFolders)
                {
                    CopyProcesser.ExcludeCopy(Path.Combine(r, "_javadoc"), config.OutputPath);
                }
            }
            );
        }
    }
}
