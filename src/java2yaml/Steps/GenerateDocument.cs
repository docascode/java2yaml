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
                foreach (var packageConfig in config.PackageConfigs)
                {
                    var procedure = new StepCollection(
                        new RunJavadoc(packageConfig.RepositoryFolder, packageConfig.Package),
                        new ApplyArtifactInformation(packageConfig.RepositoryFolder, packageConfig.Package));


                    await procedure.RunAsync(config);
                }
            });
        }

    }
}
