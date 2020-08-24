namespace Microsoft.Content.Build.Java2Yaml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class RunJavadoc : IStep
    {
        private static readonly Regex MavenArtifactPattern = new Regex(@"^\s+(?<groupId>[^:]+):(?<artifactId>[^:]+):(?<type>[^:]+)(:(?<classifier>[^:]+))?:(?<version>[^:]+):(?<scope>[^:]+)", RegexOptions.Compiled);

        public RunJavadoc(string path, Package package)
        {
            RepositoryPath = path;
            ExcludePackages = package.ExcludePackages;
        }

        public string StepName => "RunJavadoc";

        public string RepositoryPath { get; }
        public string ExcludePackages { get; }

        private static ConfigModel _config;

        public Task RunAsync(ConfigModel config)
        {
            return Task.Run(() =>
            {
                Guard.ArgumentNotNullOrEmpty(RepositoryPath, nameof(RepositoryPath));

                _config = config;

                GenerateFileList(RepositoryPath);

                var options = GenerateOptions(RepositoryPath, ExcludePackages);
                var commandLineArgs = $"{options} @{Constants.Files}";

                ConsoleLogger.WriteLine(new LogEntry
                {
                    Phase = StepName,
                    Level = LogLevel.Info,
                    Message = $" Invoking Javadoc '{RepositoryPath}'..."
                });

                ProcessUtility.Execute("javadoc.exe", commandLineArgs, RepositoryPath);
            }
            );
        }

        private void GenerateFileList(string repositoryPath)
        {
            int count = 0;
            var inputPaths = GetInputPaths(repositoryPath);
            var targetPath = Path.Combine(repositoryPath, Constants.Files);

            using (TextWriter tw = new StreamWriter(targetPath))
            {
                foreach (var path in inputPaths)
                {
                    var files = FileUtility.GetFilesByExtension(path, Constants.Extensions.Java).ToList();

                    foreach (string file in files)
                    {
                        tw.WriteLine(file);
                        count++;
                    };
                }
            }

            ConsoleLogger.WriteLine(new LogEntry
            {
                Phase = StepName,
                Level = LogLevel.Info,
                Message = $" {count} java files to scan, detail in '{targetPath}'..."
            });
        }

        // todo: fix doclet package to allow us invoke with this format "javadoc.exe @options @files"
        private string GenerateOptions(string repositoryPath, string excludePackages)
        {
            var dependencies = GetDependencies(RepositoryPath);
            var docletPath = _config.DocletPath;
            var sourcePath = string.Join(";", GetInputPathsFromConfig(repositoryPath));
            var outputPath = Path.Combine(repositoryPath, Constants.Doc);

            var options = "-classpath " + dependencies
             + " -encoding UTF-8"
             + " -docletpath " + docletPath
             + " -doclet com.microsoft.doclet.DocFxDoclet"
             + " -sourcepath " + sourcePath
             + " -outputpath " + outputPath;

            if (!String.IsNullOrEmpty(excludePackages))
            {
                options += " -excludePackages " + excludePackages;

                ConsoleLogger.WriteLine(new LogEntry
                {
                    Phase = StepName,
                    Level = LogLevel.Warning,
                    Message = $" Invalid excludePackages parameter: '{excludePackages}', processing without apply excludePackages..."
                });
            }

            return options;
        }

        private string GetDependencies(string repositoryPath)
        {
            var packageString = new StringBuilder();
            var dependencies = GetDependecyList();

            foreach (var artifact in dependencies)
            {
                var jarName = String.Concat(artifact.ArtifactId, '-', artifact.Version);
                jarName = String.IsNullOrEmpty(artifact.Classifier) ? jarName : String.Concat(jarName, '-', artifact.Classifier);
                var jar = FileUtility.GetFilesByNameExtension(
                    Path.Combine(RepositoryPath, Constants.PackageFolder),
                    jarName,
                    (artifact.Type) == "test-jar" ? "jar" : artifact.Type)
                    .FirstOrDefault();
                if (jar.Exists)
                {
                    packageString.Append(string.Concat(jar.FullName, ";"));
                }
                else if (artifact.Scope == "test")
                {
                    ConsoleLogger.WriteLine(new LogEntry
                    {
                        Phase = StepName,
                        Level = LogLevel.Warning,
                        Message = $" Test dependency not downlaoded and skipped: '{artifact.GroupId}:{artifact.ArtifactId}:{artifact.Version}'..."
                    });
                }
                else
                {
                    ConsoleLogger.WriteLine(new LogEntry
                    {
                        Phase = StepName,
                        Level = LogLevel.Error,
                        Message = $" Dependency not downlaoded and abort processing: '{artifact.GroupId}:{artifact.ArtifactId}:{artifact.Version}'..."
                    });

                    throw new Exception();
                }
            }

            return packageString.ToString();
        }

        private List<string> GetInputPaths(string repositoryPath)
        {
            var directories = GetInputPathsFromConfig(repositoryPath);

            if (_config.ExcludePaths?.Count == 0)
            {
                return directories;
            }
            else
            {
                return RemoveExcludePaths(directories);
            }
        }
        private List<string> GetInputPathsFromConfig(string repositoryPath)
        {
            return _config.InputPaths
                .Where(p => p.Equals(repositoryPath))
                .ToList();
        }

        private List<string> RemoveExcludePaths(List<string> directories)
        {
            var paths = new List<string>();

            foreach (var dir in directories)
            {
                var subDirectories = Directory.GetDirectories(dir, "*.*", SearchOption.AllDirectories)
                    .Where(p => !InExcludePaths(p))
                    .ToList();

                paths.AddRange(subDirectories);
            }

            return paths;
        }

        private bool InExcludePaths(string dir)
        {
            return _config.ExcludePaths.Any(p => dir.StartsWith(p));
        }

        private List<ArtifactItem> GetDependecyList()
        {
            var dependencyFile = FileUtility.GetFilesByNameExtension(RepositoryPath, "dependencies.raw", "txt").FirstOrDefault();

            if (null == dependencyFile)
            {
                ConsoleLogger.WriteLine(new LogEntry
                {
                    Phase = StepName,
                    Level = LogLevel.Info,
                    Message = $" File dependencies.raw.txt cannot be found under path'{RepositoryPath}'..."
                });

                throw new Exception();
            }
            else
            {
                var artifacts = new List<ArtifactItem>();

                foreach (string line in File.ReadLines(dependencyFile.FullName))
                {
                    Match m = MavenArtifactPattern.Match(line);

                    if (m.Success)
                    {
                        artifacts.Add(new ArtifactItem()
                        {
                            GroupId = m.Groups["groupId"].ToString(),
                            ArtifactId = m.Groups["artifactId"].ToString(),
                            Type = m.Groups["type"].ToString(),
                            Classifier = m.Groups["classifier"].ToString(),
                            Version = m.Groups["version"].ToString(),
                            Scope = m.Groups["scope"].ToString()
                        });
                    }
                }
                return artifacts;
            }
        }
    }
}
