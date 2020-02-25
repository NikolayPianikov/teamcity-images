using System.Collections.Generic;
using IoC;
using TeamCity.Docker.Generic;
using TeamCity.Docker.Model;

namespace TeamCity.Docker
{
    internal interface IBuildPathProvider
    {
        IEnumerable<INode<IArtifact>> GetPath([NotNull] IGraph<IArtifact, Dependency> buildGraph);
        IEnumerable<INode<IArtifact>> GetPath([NotNull] IGraph<IArtifact, Dependency> buildGraph, [NotNull] INode<IArtifact> leafNode);
    }
}