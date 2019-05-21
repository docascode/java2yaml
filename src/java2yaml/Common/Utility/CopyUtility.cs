namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class CopyUtility
    {
        public static void CopyFiles(List<string> sourcePath, string targetPath)
        {
            Guard.ArgumentNotNullOrEmpty(targetPath, nameof(targetPath));

            foreach (var file in sourcePath)
            {
                File.Copy(file, Path.Combine(targetPath, Path.GetFileName(file)), overwrite: true);
            }
        }

        public static void CopyWithExclusion(string sourcePath, string targetPath, List<string> exclusions)
        {
            var files = FileUtility.GetFilesByExtension(sourcePath, Constants.Extensions.Yaml)
                .Where(f => !IsExclude(exclusions, Path.GetFileName(f)))
                .ToList();

            Console.WriteLine($"Total {files.Count} copied.");
        }

        public static void CopyFile(string sourcePath, string targetPath, string fileName)
        {
            var filePath = Path.Combine(sourcePath, fileName);

            File.Copy(filePath, Path.Combine(targetPath, Path.GetFileName(filePath)), overwrite: true);
        }

        private static bool IsExclude(List<string> exclusion, string fileName)
        {
            return exclusion.Any(p => fileName.Equals(p));
        }
    }
}
