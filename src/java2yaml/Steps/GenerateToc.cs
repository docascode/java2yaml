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

                if (_config.PackageConfigs.Count == 1)
                {
                    var sourcePath = Path.Combine(config.PackageConfigs.FirstOrDefault().RepositoryFolder, Constants.Doc);

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


        /*  Merge the toc of multiple packages into one toc file.
         *  Some types are defined under same package(namespace) but are published as different java artifacts.
         *  It can introduce duplicate toc nodes in package level and cause DocFx build failure.
         *  Following code will also handle following.
         *  Merge:
         *  ### YamlMime:TableOfContent
         *  - uid: com.microsoft.example
         *    items:
         *       - uid: com.microsoft.example.TypeA   
         *  - uid: com.microsoft.example
         *    items:
         *       - uid: com.microsoft.example.TypeB     
         *  Into:
         *  ### YamlMime:TableOfContent
         *  - uid: com.microsoft.example
         *    items:
         *       - uid: com.microsoft.example.TypeA
         *       - uid: com.microsoft.example.TypeB   
        */

        private void MergeToc(string targetPath)
        {
            // Get toc files and deserialize to a flat list
            var tocItems = GetTocList()
                .Select(file => DeserializeYaml(file))
                .SelectMany(o => o)
                .ToList();

            var toc = tocItems
                .GroupBy(o => o.Uid)
                .ToDictionary(g => g.Key, g => g.SelectMany(g => g.Items))
                .Select(kvp => new TocItemYaml()
                {
                    Name = kvp.Key,
                    Uid = kvp.Key,
                    Items = kvp.Value.ToList()
                })
                .ToList();

            WriteMergedTocToDisk(toc, targetPath);
        }

        private List<string> GetTocList()
        {
            return ( _config.PackageConfigs
                    .Select(packageConfig => FileUtility.GetFilesByName(
                        Path.Combine(packageConfig.RepositoryFolder, Constants.Doc), Constants.Toc).First()
                        )
                    .Select(x => x.FullName.ToString())
                    .ToList()
            );
        }

        private List<TocItemYaml> DeserializeYaml(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                var toc = new Deserializer().Deserialize<List<TocItemYaml>>(reader);
                return toc;
            }
        }

        private void WriteMergedTocToDisk(List<TocItemYaml> tocFiles, string targetPath)
        {
            string tocFile = Path.Combine(targetPath, Constants.Toc);

            using (var writer = new StreamWriter(tocFile))
            {
                writer.WriteLine(Constants.YamlMime.TableOfContent);

                var yamlSerializer = new YamlSerializer();

                yamlSerializer.Serialize(writer, tocFiles);
            }
        }
    }
}
