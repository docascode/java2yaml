namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    class Program
    {
        private static ConfigModel _config;

        static int Main(string[] args)
        {
            if (!ValidateConfig(args))
            {
                return 1;
            }

            var status = 1;

            var procedure = new StepCollection(
            new GenerateDocument(),
            new CentralizeDocument(),
            new GenerateToc()
            );

            try
            {
                procedure.RunAsync(_config).Wait();
            }

            catch
            {
                // catch exception

                return status;
            }

            status = 0;

            return status;
        }

        private static bool ValidateConfig(string[] args)
        {
            if (args.Length != 0 && args.Length != 2)
            {
                Console.Error.WriteLine("Unrecognized parameters. Usage : Java2Yaml.exe [code2yaml.json, repo.json]");
                return false;
            }

            string configPath = args.Length == 0 ? Constants.ConfigFileName : args[0];
            string repoListPath = args.Length == 0 ? Constants.RepoListFileName : args[1];

            if (!File.Exists(configPath) || !File.Exists(repoListPath))
            {
                Console.Error.WriteLine($"Cannot find config file: {configPath} or {repoListPath}");
                return false;
            }

            try
            {
                _config = ConfigLoader.LoadConfig(Path.GetFullPath(configPath), Path.GetFullPath(repoListPath));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Fail to deserialize config files: {configPath}, {repoListPath} . Exception: {ex}");
                return false;
            }

            Console.WriteLine($"Config files {configPath}, {repoListPath} found. Start processing...");

            return true;
        }
    }
}
