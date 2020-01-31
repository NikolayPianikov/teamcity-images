using System.Collections.Generic;
using IoC;

namespace TeamCity.Docker.Generate
{
    internal interface IDependencyTreeFactory
    {
        [NotNull] IEnumerable<TreeNode<TreeDependency>> Create([NotNull] IEnumerable<DockerFile> dockerFiles);
    }
}