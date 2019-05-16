namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class StepCollection : IStep
    {
        public string StepName
        {
            get { return "StepCollection"; }
        }

        private IEnumerable<IStep> _steps;

        public StepCollection(params IStep[] steps)
        {
            _steps = steps ?? throw new ArgumentNullException("steps");
        }

        public async Task RunAsync(ConfigModel config)
        {
            foreach (IStep step in _steps)
            {
                try
                {
                    ConsoleLogger.WriteLine(new LogEntry { Phase = step.StepName, Level = LogLevel.Info, Message = " Start ..." });
                    await step.RunAsync(config);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.WriteLine(
                        new LogEntry
                        {
                            Phase = step.StepName,
                            Level = LogLevel.Error,
                            Message = ex.Message,
                            Data = ex,
                        });
                    // rethrow exception
                    throw;
                }
            }
        }
    }
}
