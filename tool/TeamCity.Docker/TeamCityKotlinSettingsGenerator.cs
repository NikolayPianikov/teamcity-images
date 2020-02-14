using System;
using IoC;
using TeamCity.Docker.Generic;
using TeamCity.Docker.Model;
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker
{
    internal class TeamCityKotlinSettingsGenerator: IGenerator
    {
        public void Generate([NotNull] IGraph<IArtifact, Dependency> graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
        }
    }
}
