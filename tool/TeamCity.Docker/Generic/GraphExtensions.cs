using System.Collections.Generic;
using System.Linq;

namespace TeamCity.Docker.Generic
{
    internal class GraphExtensions
    {
        public static IEnumerable<INode<TNode>> GetChildren<TNode, TLink>(IGraph<TNode, TLink> graph, INode<TNode> node) => 
            graph.Links.Where(i => i.From == node).Select(i => i.To);

        public static IEnumerable<INode<TNode>> GetAllChildren<TNode, TLink>(IGraph<TNode, TLink> graph, INode<TNode> node) => 
            GetChildren(graph, node).SelectMany(child => GetAllChildren(graph, child));

        public static IEnumerable<INode<TNode>> GetParents<TNode, TLink>(IGraph<TNode, TLink> graph, INode<TNode> node) =>
            graph.Links.Where(i => i.To == node).Select(i => i.From);

        public static IEnumerable<INode<TNode>> GetAllParents<TNode, TLink>(IGraph<TNode, TLink> graph, INode<TNode> node) =>
            GetParents(graph, node).SelectMany(child => GetAllParents(graph, child));
    }
}
