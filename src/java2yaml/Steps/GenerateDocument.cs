namespace Microsoft.Content.Build.Java2Yaml
{
    using System.Threading.Tasks;

    public class GenerateDocument : IStep
    {
        public string StepName
        {
            get { return "GenerateDocument"; }
        }

        public Task RunAsync(ConfigModel config)
        {
            return Task.Run(() =>
            {
                foreach (var path in config.RepositoryFolders)
                {
                    var procedure = new StepCollection(
                        new RestoreDependency(path),
                        new RunJavadoc(path));

                    try
                    {
                        procedure.RunAsync(config).Wait();
                    }

                    catch
                    {
                        // do nothing
                    }
                }
            }
            );
        }

    }
}
