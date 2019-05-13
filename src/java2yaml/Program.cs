namespace Microsoft.Content.Build.JavaYaml
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Microsoft.Content.Build.Java2Yaml.Constants;

    class Program
    {
        static int Main(string[] args)
        {
            if (!ValidateConfig(args))
            {
                return 1;
            }

            var status = 1;

            try
            {
                // generate documents
            }

            catch
            {
                // catch exception
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
                Console.Error.WriteLine($"Cannot find config file: {Constants.ConfigFileName} or {Constants.RepoListFileName}");
                return false;
            }

            return true;
        }
    }
}
