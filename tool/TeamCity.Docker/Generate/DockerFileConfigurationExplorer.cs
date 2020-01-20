using System.Collections.Generic;
using System.IO;
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Generate
{
    internal class DockerFileConfigurationExplorer : IDockerFileConfigurationExplorer
    {
        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;

        public DockerFileConfigurationExplorer(
            ILogger logger,
            IFileSystem fileSystem)
        {
            _logger = logger;
            _fileSystem = fileSystem;
        }

        public IEnumerable<DockerFileConfiguration> Explore(string sourcePath)
        {
            using (_logger.CreateBlock("Explore configs"))
            {
                _logger.Log($"The configuration path is \"{sourcePath}\"");
                var templateCounter = 0;
                foreach (var dockerfileTemplate in _fileSystem.EnumerateFileSystemEntries(sourcePath, "*.Dockerfile"))
                {
                    var dockerfileTemplateContent = _fileSystem.ReadFile(dockerfileTemplate);
                    var dockerfileTemplateRelative = Path.GetRelativePath(sourcePath, dockerfileTemplate);
                    using (_logger.CreateBlock($"{++templateCounter:000} {dockerfileTemplateRelative}"))
                    {
                        var dockerfileTemplateDir = Path.GetDirectoryName(dockerfileTemplate) ?? ".";
                        var dockerfileTemplatePath = Path.GetFileName(dockerfileTemplate);
                        var variants = new List<DockerFileVariant>();
                        var configCounter = 0;
                        foreach (var configFile in _fileSystem.EnumerateFileSystemEntries(dockerfileTemplateDir, dockerfileTemplatePath + ".config"))
                        {
                            var buildPath = Path.GetDirectoryName(Path.GetRelativePath(sourcePath, configFile)) ?? "";
                            using (_logger.CreateBlock($"{templateCounter:000}.{++configCounter:000} {Path.GetRelativePath(sourcePath, configFile)}"))
                            {
                                var vars = new Dictionary<string, string>();
                                foreach (var line in _fileSystem.ReadLines(configFile))
                                {
                                    var text = line.Trim();
                                    if (text.StartsWith('#') || text.Length < 3)
                                    {
                                        continue;
                                    }

                                    var eq = text.IndexOf('=');
                                    if (eq < 1)
                                    {
                                        continue;
                                    }

                                    var key = text.Substring(0, eq);
                                    var val = text.Substring(eq + 1);
                                    vars[key] = val;
                                    _logger.Log($"SET {key}={val}");
                                }

                                variants.Add(new DockerFileVariant(buildPath, vars));
                            }
                        }

                        yield return new DockerFileConfiguration(dockerfileTemplateContent, variants);
                    }
                }
            }
        }
    }
}