namespace Microsoft.Content.Build.Java2Yaml
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class FileUtility
    {
        public static IEnumerable<FileInfo> GetFilesByName(string rootPath, string fileName)
        {
            Guard.ArgumentNotNullOrEmpty(rootPath, nameof(rootPath));
            Guard.ArgumentNotNullOrEmpty(fileName, nameof(fileName));

            return new DirectoryInfo(rootPath)
                .GetFiles(fileName, SearchOption.AllDirectories);
        }

        public static IEnumerable<string> GetFilesByExtension(string rootPath, string extension)
        {
            Guard.ArgumentNotNullOrEmpty(rootPath, nameof(rootPath));

            if (string.IsNullOrEmpty(extension))
            {
                extension = "*";
            }

            var pattern = string.Join("*." , extension);
            return Directory.GetFiles(rootPath, pattern, SearchOption.AllDirectories);
        }
    }
}
