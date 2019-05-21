namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class GenerateToc : IStep
    {
        public string StepName
        {
            get { return "GenerateToc"; }
        }

        public Task RunAsync(ConfigModel config)
        {
            return Task.Run(() =>
            {
                if (config.RepositoryFolders.Count == 1)
                {
                    var repositoryPath = config.RepositoryFolders
                        .First();

                    var sourcePath = Path.Combine(repositoryPath, Constants.Doc);

                    ConsoleLogger.WriteLine(new LogEntry
                    {
                        Phase = StepName,
                        Level = LogLevel.Info,
                        Message = $" Merge toc not reqruied..."
                    });

                    CopyUtility.CopyFile(sourcePath, config.OutputPath, Constants.Toc);
                }
                else
                {
                    MergeToc();
                }
            }
            );
        }

        private void MergeToc()
        {
            throw new NotImplementedException();
        }
    }
}
