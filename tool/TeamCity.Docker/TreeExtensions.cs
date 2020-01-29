using System.Collections.Generic;

namespace TeamCity.Docker
{
    internal static class TreeExtensions
    {
        public static IEnumerable<TreeNode<T>> EnumerateNodes<T>(this IEnumerable<TreeNode<T>> nodes)
        {
            foreach (var node in nodes)
            {
                yield return node;
                foreach (var child in EnumerateNodes(node.Children))
                {
                    yield return child;
                }
            }
        }

        public static IEnumerable<TreeNode<T>> EnumerateNodes<T>(this TreeNode<T> node)
        {
            yield return node;
            foreach (var child in EnumerateNodes(node.Children))
            {
                yield return child;
            }
        }
    }
}
