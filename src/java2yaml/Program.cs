namespace Microsoft.Content.Build.Java2Yaml
{

    class Program
    {
        private static List<ConfigModel> _configs;
        private static string _docletPath;

        static async Task<int> Main(string[] args)
        {
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
                    config.DocletPath = _docletPath;

                    ConsoleLogger.WriteLine(new LogEntry
                    {
                        Phase = "Initialize",
                        Level = LogLevel.Info,
                        Message = $" {config.PackageConfigs.Count} package(s) to process, target folder: '{config.OutputPath}'"
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
                string docletPath = Path.Combine(PathUtility.GetAssemblyDirectory(), Constants.DocletLocation);
                return ValidatePackageConfig(args[0]) && IsDocletExstis(docletPath);
            }
            else if (args.Length == 2)
            {
                // args[0] - package.json
                // args[1] - doclet path
                return ValidatePackageConfig(args[0]) && IsDocletExstis(args[1]);
            }
            else
            {
                Console.Error.WriteLine("Unrecognized parameters.");
                Console.Error.WriteLine("Package-based Usage : Java2Yaml.exe [package.json]");
                Console.Error.WriteLine("Or : Java2Yaml.exe [package.json] [doclet-path]");
                return false;
            }
        }

        private static bool ValidatePackageConfig(string path)
        {
            string packageConfigPath = Path.GetFullPath(path);

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

            Console.WriteLine($"Config file {packageConfigPath} found.");

            return true;
        }

        private static bool IsDocletExstis(string docletPath)
        {
            var doclet = Path.GetFullPath(docletPath);

            if (File.Exists(doclet))
            {
                _docletPath = doclet;
                Console.WriteLine($"Config file {doclet} found.");
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
