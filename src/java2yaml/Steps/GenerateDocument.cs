namespace Microsoft.Content.Build.Java2Yaml
{
    using System.Threading.Tasks;

    public class GenerateDocument : IStep
    {
        public string StepName => "GenerateDocument";

        public Task RunAsync(ConfigModel config)
        {
            return Task.Run(async () =>
            {
                foreach (var path in config.RepositoryFolders)
                {
                    var procedure = new StepCollection(
                        new RestoreDependency(path),
                        new RunJavadoc(path));

                    await procedure.RunAsync(config);
                }
            });
        }

    }
}
