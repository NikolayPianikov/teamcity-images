﻿using System;
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
                nodes[repoTag.tag] = new Node(new TreeDependency(repoTag.file));
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
                    var dockerFile = new DockerFile(string.Empty, metadata, new List<DockerLine>());
                    var newNode = new Node(new TreeDependency(dockerFile));
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

                    roots.Remove(node);
                    hasBaseImage = true;
                    parentNode.Children.Add(node);
                }

                if (!hasBaseImage)
                {
                    roots.Add(node);
                }
            }

            return (
                from node in roots.Select(root => CreateTreeNode(root)).EnumerateNodes()
                where node.Parent != null && string.IsNullOrEmpty(node.Parent.Value.File.Path)
                select node)
                .Distinct();
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

            public override bool Equals(object obj) => obj is Node other && Dependency.File.Equals(other.Dependency.File);

            public override int GetHashCode() => Dependency.GetHashCode();

            public override string ToString() => Dependency.ToString();
        }
    }
}
