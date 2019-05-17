namespace Microsoft.Content.Build.Java2Yaml
{

    using System.Threading.Tasks;

    public class RunJavadoc : IStep
    {
        public RunJavadoc(string path)
        {
            RepositoryPath = path;
        }

        public string StepName
        {
            get { return "RunJavadoc"; }
        }

        public string RepositoryPath { get; }

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
