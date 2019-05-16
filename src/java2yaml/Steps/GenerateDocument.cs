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
                //TODO
            }
            );
        }

    }
}
