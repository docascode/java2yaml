namespace Microsoft.Content.Build.Java2Yaml
{
    using System.IO;

    public static class StringUtility
    {
        public static string BackSlashToForwardSlash(this string input)
        {
            if (string.IsNullOrEmpty(input)) return null;
            return input.Replace('\\', '/');
        }

        public static string ToNormalizedFullPath(this string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            return Path.GetFullPath(path).BackSlashToForwardSlash().TrimEnd('/');
        }
    }
}
