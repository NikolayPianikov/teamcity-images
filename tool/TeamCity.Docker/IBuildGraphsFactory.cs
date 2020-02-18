using System.Collections.Generic;
using TeamCity.Docker.Generic;
using TeamCity.Docker.Model;

namespace TeamCity.Docker
{
    internal interface IBuildGraphsFactory
    {
        IEnumerable<IGraph<IArtifact, Dependency>> Create(IGraph<IArtifact, Dependency> graph);
    }
}