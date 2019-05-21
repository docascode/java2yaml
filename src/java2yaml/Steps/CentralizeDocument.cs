namespace Microsoft.Content.Build.Java2Yaml
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public class CentralizeDocument : IStep
    {
        public string StepName => "CentralizeDocument";

        public Task RunAsync(ConfigModel config)
        {
            return Task.Run(() =>
            {
                if (!Directory.Exists(config.OutputPath))
                {
                    Directory.CreateDirectory(config.OutputPath);
                }

                foreach (var repositoryPath in config.RepositoryFolders)
                {
                    var sourcePath = Path.Combine(repositoryPath, Constants.Doc);

                    ConsoleLogger.WriteLine(new LogEntry
                    {
                        Phase = StepName,
                        Level = LogLevel.Info,
                        Message = $" Copy yaml files from {sourcePath} to {config.OutputPath}."
                    });

                    var exclusions = new List<string> { Constants.Toc };

                    CopyUtility.CopyWithExclusion(sourcePath, config.OutputPath, exclusions);
                }
            });
        }
    }
}
