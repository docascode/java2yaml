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
                var targetPath = Path.Combine(Directory.GetParent(config.OutputPath).ToString(), Constants.Doc);

                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                foreach (var packageConfig in config.PackageConfigs)
                {
                    var sourcePath = Path.Combine(packageConfig.RepositoryFolder, Constants.Doc);

                    ConsoleLogger.WriteLine(new LogEntry
                    {
                        Phase = StepName,
                        Level = LogLevel.Info,
                        Message = $" Copy yaml files from {sourcePath} to {targetPath}."
                    });

                    var exclusions = new List<string> { Constants.Toc };

                    CopyUtility.CopyWithExclusion(sourcePath, targetPath, exclusions);
                }
            });
        }
    }
}
