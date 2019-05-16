namespace Microsoft.Content.Build.Java2Yaml
{
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
                //TODO
            }
            );
        }
    }
}
