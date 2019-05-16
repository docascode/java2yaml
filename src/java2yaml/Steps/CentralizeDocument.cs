namespace Microsoft.Content.Build.Java2Yaml
{
    using System.Threading.Tasks;

    public class CentralizeDocument : IStep
    {
        public string StepName
        {
            get { return "CentralizeDocument"; }
        }

        public Task RunAsync(ConfigModel config)
        {
            return Task.Run(() =>
            {
            }
            );
        }
    }
}
