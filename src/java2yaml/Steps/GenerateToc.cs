namespace Microsoft.Content.Build.Java2Yaml
{

    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.DocAsCode.YamlSerialization;
    using YamlDotNet.Serialization;

    public class GenerateToc : IStep
    {
        private static ConfigModel _config;

        public string StepName
        {
            get { return "GenerateToc"; }
        }

        public Task RunAsync(ConfigModel config)
        {
            return Task.Run(() =>
            {
                _config = config;
                var targetPath = Path.Combine(Directory.GetParent(config.OutputPath).ToString(), Constants.Doc);

                if (_config.RepositoryFolders.Count == 1)
                {
                    var sourcePath = Path.Combine(config.RepositoryFolders[0], Constants.Doc);

                    ConsoleLogger.WriteLine(new LogEntry
                    {
                        Phase = StepName,
                        Level = LogLevel.Info,
                        Message = $" Merge TOC not reqruied, copy toc.yml ..."
                    });

                    CopyUtility.CopyFile(sourcePath, targetPath, Constants.Toc);
                }
                else
                {
                    MergeToc(targetPath);

                    ConsoleLogger.WriteLine(new LogEntry
                    {
                        Phase = StepName,
                        Level = LogLevel.Info,
                        Message = $" Merge TOC completed, write toc.yml to {targetPath} ..."
                    });
                }
            }
            );
        }

        private void MergeToc(string targetPath)
        {
            var tocFiles = new List<TocYaml>();

            tocFiles = GetTocList()
                .Select(file => DeserializeYaml(file))
                .ToList();

            WriteMergedTocToDisk(tocFiles, targetPath);
        }

        private List<string> GetTocList()
        {
            return ( _config.RepositoryFolders
                    .Select(path => FileUtility.GetFilesByName(Path.Combine(path, Constants.Doc), Constants.Toc).First())
                    .Select(x => x.FullName.ToString())
                    .ToList()
            );
        }

        private TocYaml DeserializeYaml(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                var toc = new Deserializer().Deserialize<TocYaml>(reader);
                return toc;
            }
        }

        private void WriteMergedTocToDisk(List<TocYaml> tocFiles, string targetPath)
        {
            string tocFile = Path.Combine(targetPath, Constants.Toc);

            using (var writer = new StreamWriter(tocFile))
            {
                writer.WriteLine(Constants.YamlMime.TableOfContent);

                var yamlSerializer = new YamlSerializer();

                tocFiles.ForEach(tocYaml => yamlSerializer.Serialize(writer, tocYaml));
            }
        }
    }
}
