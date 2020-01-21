using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public Result<IEnumerable<DockerFileConfiguration>> Explore(string sourcePath, IEnumerable<string> configurationFiles)
        {
            var additionalVars = new Dictionary<string, string>();
            using (_logger.CreateBlock("Explore"))
            {
                foreach (var configurationFile in configurationFiles)
                {
                    if (!_fileSystem.IsFileExist(configurationFile))
                    {
                        _logger.Log($"The configuration file \"{configurationFile}\"  (\"{Path.GetFullPath(configurationFile)}\") does not exist.", Result.Error);
                        return new Result<IEnumerable<DockerFileConfiguration>>(Enumerable.Empty<DockerFileConfiguration>());
                    }

                    using (_logger.CreateBlock(configurationFile))
                    {
                        additionalVars = UpdateVariables(additionalVars, GetVariables(configurationFile));
                    }
                }

                _logger.Log($"The configuration path is \"{sourcePath}\" (\"{Path.GetFullPath(sourcePath)}\")");
            }

            return new Result<IEnumerable<DockerFileConfiguration>>(GetConfigurations(sourcePath, additionalVars));
        }

        private IEnumerable<DockerFileConfiguration> GetConfigurations(string sourcePath, IDictionary<string, string> additionalVars)
        {
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
                            var vars = UpdateVariables(GetVariables(configFile), additionalVars);
                            variants.Add(new DockerFileVariant(buildPath, vars));
                        }
                    }

                    yield return new DockerFileConfiguration(dockerfileTemplateContent, variants);
                }
            }
        }

        private IDictionary<string, string> GetVariables(string configFile)
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

            return vars;
        }

        private Dictionary<string, string> UpdateVariables(IDictionary<string, string> variables, IDictionary<string, string> newVariables)
        {
            var result = new Dictionary<string, string>(variables);
            foreach (var (key, value) in newVariables)
            {
                if (result.ContainsKey(key))
                {
                    _logger.Log($"UPDATE {key}={value}");
                    result[key] = value;
                }
                else
                {
                    _logger.Log($"SET {key}={value}");
                    result.Add(key, value);
                }
            }

            return result;
        }
    }
}