using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IoC;
using TeamCity.Docker.Generic;
using TeamCity.Docker.Model;

namespace TeamCity.Docker
{
    /*
    internal class Builder
    {
        public async Task<Result> Build(IGraph<IArtifact, Dependency> graph, int maxWeight)
        {
            var imagesWithLeaves = graph.Links.Select(i => i.From).Where(i => i.Value is Image).ToList();
            var leaveImages = graph.Nodes.Where(i => i.Value is Image).Except(imagesWithLeaves).ToList();

            var state = new State();
            foreach (var leave in leaveImages)
            {
                await Build(false, graph, (INode<Image>)leave, state.Clone(), maxWeight);
            }
        }

        private async Task<Result> Build(bool probe, IGraph<IArtifact, Dependency> graph, INode<Image> imageNode, State state, int maxWeight)
        {
            var children = GraphExtensions.GetChildren(graph, imageNode).ToList();
            var references = new List<Reference>();
            var images = new List<INode<Image>>();
            foreach (var child in children)
            {
                switch (child.Value)
                {
                    case Image _:
                        images.Add((INode<Image>)child);
                        break;

                    case Reference reference:
                        references.Add(reference);
                        break;
                }
            }

            foreach (var reference in references)
            {
                state.Cache.Add(reference);
            }

            state.Cache.Add(imageNode.Value);

            if (state.Weight > maxWeight)
            {
                var parents = GraphExtensions.GetAllParents(graph, imageNode).ToHashSet();
                var toRemove = state.Cache.Except(parents.Select(i => i.Value)).ToList();
                while (state.Weight > maxWeight && toRemove.Count > 0)
                {
                    var removing = toRemove.First();
                    state.Cache.Remove(removing);
                    toRemove.Remove(removing);
                }

                if (state.Weight > maxWeight)
                {
                    return Result.Error;
                }
            }

            var sortedImages = new List<Tuple<int, INode<Image>>>();
            foreach (var image in images)
            {
                var newState = state.Clone();
                await Build(true, graph, image, newState, maxWeight);
                var commonState = new State(newState.Cache.Intersect(state.Cache));
                sortedImages.Add(Tuple.Create(commonState.Weight, image));
            }

            sortedImages = sortedImages.OrderBy(i => i.Item1).ToList();
            foreach (var image in sortedImages)
            {
                await Build(false, graph, image.Item2, state, maxWeight);
            }
        }

        private class State
        {
            public readonly HashSet<IArtifact> Cache;

            public State()
                :this(Enumerable.Empty<IArtifact>())
            { }

            public State(IEnumerable<IArtifact> artifacts)
            {
                Cache = new HashSet<IArtifact>(artifacts);
            }

            public int Weight => Cache.Aggregate(0, (acc, artifact) => acc + artifact.Weight.Value);

            public State Clone()
            {
                return new State(Cache);
            }
        }
    }
*/
}
