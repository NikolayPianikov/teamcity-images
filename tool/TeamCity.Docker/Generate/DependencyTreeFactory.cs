using System;
using System.Collections.Generic;
using System.Linq;
using IoC;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Generate
{
    internal class DependencyTreeFactory : IDependencyTreeFactory
    {
        public IEnumerable<TreeNode<TreeDependency>> Create(IEnumerable<DockerFile> dockerFiles)
        {
            if (dockerFiles == null)
            {
                throw new ArgumentNullException(nameof(dockerFiles));
            }

            var repoTags =
                from file in dockerFiles
                from tag in file.Metadata.Tags
                select new { file, tag };

            var nodes = new Dictionary<string, Node>();
            foreach (var repoTag in repoTags)
            {
                nodes[repoTag.tag] = new Node(new TreeDependency(repoTag.file, new Dependency(repoTag.tag, DependencyType.Build)));
            }

            foreach (var node in nodes.Values.ToList())
            {
                foreach (var dependency in node.Dependency.File.Metadata.Dependencies)
                {
                    if (nodes.ContainsKey(dependency.RepoTag))
                    {
                        continue;
                    }

                    var metadata = new Metadata(dependency.RepoTag, new List<string>(), new List<Dependency>(), new List<string>(), new List<string>());
                    var dockerFile = new DockerFile("", metadata, new List<DockerLine>());
                    var newNode = new Node(new TreeDependency(dockerFile, new Dependency(dependency.RepoTag, DependencyType.Pull)));
                    nodes.Add(dependency.RepoTag, newNode);
                }
            }

            var roots = new List<Node>();
            foreach (var node in nodes.Values)
            {
                var hasBaseImage = false;
                foreach (var dependency in node.Dependency.File.Metadata.Dependencies)
                {
                    if (!nodes.TryGetValue(dependency.RepoTag, out var parentNode))
                    {
                        continue;
                    }

                    if (parentNode.Dependency.Dependency.DependencyType != DependencyType.Pull)
                    {
                        roots.Remove(node);
                        hasBaseImage = true;
                    }

                    parentNode.Children.Add(node);
                }

                if (!hasBaseImage && node.Dependency.Dependency.DependencyType != DependencyType.Pull)
                {
                    roots.Add(node);
                }
            }

            return roots.Select(root => CreateTreeNode(root));
        }

        private TreeNode<TreeDependency> CreateTreeNode([NotNull] Node node, [CanBeNull] TreeNode<TreeDependency> parent = null)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            var children = new List<TreeNode<TreeDependency>>();
            var treeNode = new TreeNode<TreeDependency>(parent, node.Dependency, children);
            children.AddRange(node.Children.Select(i => CreateTreeNode(i, treeNode)));
            return treeNode;
        }

        private class Node
        {
            public readonly TreeDependency Dependency;
            public readonly HashSet<Node> Children;

            public Node(TreeDependency dependency)
            {
                Dependency = dependency;
                Children = new HashSet<Node>();
            }

            public override bool Equals(object obj)
            {
                return obj is Node other && (Dependency.File.Equals(other.Dependency.File) && Dependency.Dependency.Equals(other.Dependency.Dependency));
            }

            public override int GetHashCode()
            {
                return Dependency.GetHashCode();
            }

            public override string ToString()
            {
                return Dependency.ToString();
            }
        }
    }
}
