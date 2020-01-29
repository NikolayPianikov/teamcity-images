using System.Collections.Generic;
using IoC;

namespace TeamCity.Docker.Generate
{
    internal interface IReadmeGenerator
    {
        [NotNull] IEnumerable<ReadmeFile> Generate([NotNull] IEnumerable<TreeNode<DockerFile>> dockerNodes);
    }
}