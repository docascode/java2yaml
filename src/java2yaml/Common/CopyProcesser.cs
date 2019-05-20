namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class CopyProcesser
    {
        const string Exeluded = "toc.yml";
        const string Pattern = "*.yml";

        public static int ExcludeCopy(string source, string target)
        {
            var sourceFiles = Directory.EnumerateFiles(source, Pattern, SearchOption.AllDirectories)
                .Where(x => !Path.GetFileName(x).Equals(Exeluded, StringComparison.OrdinalIgnoreCase));

            var result = Copy(sourceFiles, target);

            return result;
        }

        public static int Copy(IEnumerable<string> files, string targetPath)
        {
            foreach (var file in files)
            {
                File.Copy(file, Path.Combine(targetPath, Path.GetFileName(file)), overwrite: true);
            }

            var target = Directory.EnumerateFiles(targetPath, Pattern, SearchOption.AllDirectories).Count();

            Console.WriteLine($"Total {files.Count()} to copy, {target} processed.");

            if (files.Count() != target)
            {

                return 1;
            }

            return 0;
        }
    }
}