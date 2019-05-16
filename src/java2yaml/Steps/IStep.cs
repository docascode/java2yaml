namespace Microsoft.Content.Build.Java2Yaml
{
    using System.Threading.Tasks;

    public interface IStep
    {
        string StepName { get; }

        Task RunAsync(ConfigModel config);
    }
}
