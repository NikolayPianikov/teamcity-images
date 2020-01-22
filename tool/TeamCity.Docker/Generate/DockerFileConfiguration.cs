using System;
using System.Collections.Generic;
using IoC;

namespace TeamCity.Docker.Generate
{
    internal struct DockerFileConfiguration
    {
        [NotNull] public readonly string DockerfileTemplateContent;
        [NotNull] public readonly IReadOnlyList<DockerFileVariant> Variants;

        public DockerFileConfiguration([NotNull] string dockerfileTemplateContent, [NotNull] IReadOnlyList<DockerFileVariant> variants)
        {
            DockerfileTemplateContent = dockerfileTemplateContent ?? throw new ArgumentNullException(nameof(dockerfileTemplateContent));
            Variants = variants ?? throw new ArgumentNullException(nameof(variants));
        }
    }
}
