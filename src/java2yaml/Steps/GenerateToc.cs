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

                if (_config.RepositoryFolders.Count == 1)
                {
                    var sourcePath = Path.Combine(config.RepositoryFolders[0], Constants.Doc);

                    ConsoleLogger.WriteLine(new LogEntry
                    {
                        Phase = StepName,
                        Level = LogLevel.Info,
                        Message = $" Merge TOC not reqruied, copy toc.yml ..."
                    });

                    CopyUtility.CopyFile(sourcePath, config.OutputPath, Constants.Toc);
                }
                else
                {
                    MergeToc();

                    ConsoleLogger.WriteLine(new LogEntry
                    {
                        Phase = StepName,
                        Level = LogLevel.Info,
                        Message = $" Merge completed, write toc.yml to {_config.OutputPath} ..."
                    });
                }
            }
            );
        }

        private void MergeToc()
        {
            var tocFiles = new List<TocYaml>();

            tocFiles = GetTocList()
                .Select(file => DeserializeYaml(file))
                .ToList();

            WriteMergedTocToDisk(tocFiles);
        }

        private List<string> GetTocList()
        {
            return ( _config.RepositoryFolders
                    .Select(path => FileUtility.GetFilesByName(Path.Combine(path, Constants.Doc), Constants.Toc).FirstOrDefault())
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

        private void WriteMergedTocToDisk(List<TocYaml> tocFiles)
        {
            string tocFile = Path.Combine(_config.OutputPath, Constants.Toc);

            using (var writer = new StreamWriter(tocFile))
            {
                writer.WriteLine(Constants.YamlMime.TableOfContent);

                var yamlSerializer = new YamlSerializer();

                tocFiles.ForEach(tocYaml => yamlSerializer.Serialize(writer, tocYaml));
            }
        }
    }
}
