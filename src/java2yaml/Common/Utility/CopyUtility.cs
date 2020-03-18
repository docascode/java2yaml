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
                CopyFile(file,Path.Combine(targetPath, Path.GetFileName(file)), true);
            }
        }

        public static void CopyWithExclusion(string sourcePath, string targetPath, List<string> exclusions)
        {
            var files = FileUtility.GetFilesByExtension(sourcePath, Constants.Extensions.Yaml)
                .Where(f => !IsExclude(exclusions, Path.GetFileName(f)))
                .ToList();
            
            CopyFiles(files, targetPath);

            Console.WriteLine($"Total {files.Count} copied.");
        }

        public static void CopyFile(string sourcePath, string targetPath, string fileName)
        {
            var filePath = Path.Combine(sourcePath, fileName);
            CopyFile(filePath, Path.Combine(targetPath, Path.GetFileName(filePath)), true);
        }

        internal static void CopyFile(string source, string target, bool isOverWrite = true)
        {
            Guard.ArgumentNotNullOrEmpty(source, nameof(source));
            Guard.ArgumentNotNullOrEmpty(target, nameof(target));

            File.Copy(PathUtility.ResolveLongPath(source), PathUtility.ResolveLongPath(target), overwrite: isOverWrite);
        }

        private static bool IsExclude(List<string> exclusion, string fileName)
        {
            return exclusion.Any(p => fileName.Equals(p));
        }
    }
}
