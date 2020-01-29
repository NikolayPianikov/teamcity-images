using System.Collections.Generic;
using IoC;

namespace TeamCity.Docker.Generate
{
    internal interface IDependencyTreeFactory
    {
        [NotNull] IEnumerable<TreeNode<DockerFile>> Create([NotNull] IEnumerable<DockerFile> dockerFiles);
    }
}