namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class CopyUtility
    {
        public static int CopyFiles(List<string> sourcePath, string targetPath)
        {
            Guard.ArgumentNotNullOrEmpty(targetPath, nameof(targetPath));

            var fileCopyed = 0;

            foreach (var file in sourcePath)
            {
                File.Copy(file, Path.Combine(targetPath, Path.GetFileName(file)), overwrite: true);
                fileCopyed++;
            }

            return fileCopyed;
        }

        public static void CopyWithExclusion(string sourcePath, string targetPath, string exclusion)
        {
            var files = FileUtility.GetFilesByExtension(sourcePath, Constants.Extensions.Yaml)
                .Where(f => !IsExclude(exclusion, Path.GetFileName(f)))
                .ToList();

            var copyed = CopyFiles(files, targetPath);

            Console.WriteLine($"Total {files.Count} to copy, {copyed} processed.");

            if (files.Count != copyed)
            {
                throw new InvalidOperationException("Copy files failed to complete.");
            }
        }

        public static void CopyFile(string sourcePath, string targetPath, string fileName)
        {
            var filePath = Path.Combine(sourcePath, fileName);

            Console.WriteLine($"Copy {filePath} to {targetPath}.");

            File.Copy(filePath, Path.Combine(targetPath, Path.GetFileName(filePath)), overwrite: true);
        }

        private static bool IsExclude(string exclusion, string fileName)
        {
            return exclusion.Any(p => fileName.Equals(p));
        }
    }
}
