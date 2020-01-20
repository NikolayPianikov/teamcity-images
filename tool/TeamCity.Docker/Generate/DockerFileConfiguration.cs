using System.Collections.Generic;

namespace TeamCity.Docker.Generate
{
    internal struct DockerFileConfiguration
    {
        public readonly string DockerfileTemplateContent;
        public readonly IReadOnlyList<DockerFileVariant> Variants;

        public DockerFileConfiguration(string dockerfileTemplateContent, IReadOnlyList<DockerFileVariant> variants)
        {
            DockerfileTemplateContent = dockerfileTemplateContent;
            Variants = variants;
        }
    }
}
