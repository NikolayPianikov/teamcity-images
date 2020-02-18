﻿using System;
using System.Collections.Generic;
using System.Linq;
using TeamCity.Docker.Generic;
using TeamCity.Docker.Model;

namespace TeamCity.Docker
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class BuildGraphsFactory : IBuildGraphsFactory
    {
        public IEnumerable<IGraph<IArtifact, Dependency>> Create(IGraph<IArtifact, Dependency> graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));

            var buildGraph = graph.Copy(i => i.Value is Image || i.Value is Reference);
            var cutNodes = new HashSet<INode<IArtifact>>();
            while (true)
            {
                var bestCut = new HashSet<INode<IArtifact>>(buildGraph.FindMinimumCutByStoerWagner(links => links.Select(i => i.From.Value.Weight.Value).Concat(Enumerable.Repeat(0, 1)).Sum(), out var bestCost));
                if (bestCost > 0)
                {
                    yield return buildGraph.Copy(node => node.Value is Image);
                    break;
                }

                if (bestCut.Count > buildGraph.NodesCount >> 1)
                {
                    bestCut = new HashSet<INode<IArtifact>>(graph.Nodes.Except(bestCut));
                }

                cutNodes.UnionWith(bestCut);
                yield return buildGraph.Copy(node => node.Value is Image && bestCut.Contains(node));
                buildGraph = buildGraph.Copy(node => !cutNodes.Contains(node));
            }
        }
    }
}