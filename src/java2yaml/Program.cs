namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    class Program
    {
        private static List<ConfigModel> _configs;

        static async Task<int> Main(string[] args)
        {
            if (!IsDocletExstis())
            {
                return 1;
            }

            if (!ValidateConfig(args))
            {
                return 1;
            }

            var status = 1;
            var watch = Stopwatch.StartNew();

            var procedure = new StepCollection(
                                new GenerateDocument(),
                                new CentralizeDocument(),
                                new GenerateToc()
                            );

            try
            {
                foreach (var config in _configs)
                {
                    ConsoleLogger.WriteLine(new LogEntry
                    {
                        Phase = "Initialize",
                        Level = LogLevel.Info,
                        Message = $" {config.RepositoryFolders.Count} package(s) to process, target folder: '{config.OutputPath}'"
                    });

                    await procedure.RunAsync(config);
                }
                status = 0;
            }

            catch
            {
                // do nothing
            }
            finally
            {
                watch.Stop();
            }

            var statusString = status == 0 ? "Succeeded" : "Failed";
            Console.WriteLine($"{statusString} in {watch.ElapsedMilliseconds} milliseconds.");
            return status;
        }

        private static bool ValidateConfig(string[] args)
        {
            if (args.Length == 1)
            {
                return ValidateByPackageBased(args);
            }
            else
            {
                Console.Error.WriteLine("Unrecognized parameters.");
                Console.Error.WriteLine("Package-based Usage : Java2Yaml.exe [package.json]");
                return false;
            }
        }

        private static bool ValidateByPackageBased(string[] args)
        {
            string packageConfigPath = args[0];

            if (!File.Exists(packageConfigPath))
            {
                Console.Error.WriteLine($"Cannot find config file: {packageConfigPath}.");
                return false;
            }

            try
            {
                _configs = ConfigLoader.LoadConfig(Path.GetFullPath(packageConfigPath));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Fail to deserialize config files: {packageConfigPath}. Exception: {ex}");
                return false;
            }

            Console.WriteLine($"Config files {packageConfigPath} found. Start processing...");

            return true;
        }

        private static bool IsDocletExstis()
        {
            var doclet = Path.Combine(PathUtility.GetAssemblyDirectory(), Constants.DocletLocation);

            if (File.Exists(doclet))
            {
                return true;
            }
            else
            {
                Console.WriteLine($"Cannot found docfx-doclet.jar.");
                return false;
            }
        }
    }
}
