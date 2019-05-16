namespace Microsoft.Content.Build.Java2Yaml
{
    public class LogEntry
    {
        public string Phase { get; set; }

        public LogLevel Level { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }

    public enum LogLevel
    {
        Error,
        Warning,
        Info,
        Verbose,
    }
}
