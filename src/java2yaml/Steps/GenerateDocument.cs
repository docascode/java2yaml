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
                foreach (var pak in config.PackageConfigs)
                {
                    var procedure = new StepCollection(
                        new RestoreDependency(pak.RepositoryFolder),
                        new RunJavadoc(pak.RepositoryFolder, pak.Package));

                    await procedure.RunAsync(config);
                }
            });
        }

    }
}
