using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Generate
{
    internal class DependencyTreeFactory : IDependencyTreeFactory
    {
        public IEnumerable<TreeNode<DockerFile>> Create(IEnumerable<DockerFile> dockerFiles)
        {
            if (dockerFiles == null)
            {
                throw new ArgumentNullException(nameof(dockerFiles));
            }

            var repoTags =
                from file in dockerFiles
                from tag in file.Metadata.Tags
                select new { file, tag };

            var nodes = new Dictionary<string, TreeNode<DockerFile>>();
            foreach (var repoTag in repoTags)
            {
                nodes[repoTag.tag] = new TreeNode<DockerFile>(repoTag.file);
            }

            var roots = new HashSet<TreeNode<DockerFile>>();
            foreach (var node in nodes.Values)
            {
                var hasBaseImage = false;
                foreach (var baseImage in node.Value.Metadata.BaseImages)
                {
                    if (nodes.TryGetValue(baseImage, out var parentNode))
                    {
                        roots.Remove(node);
                        if (!parentNode.Children.Contains(node))
                        {
                            parentNode.Children.Add(node);
                        }

                        hasBaseImage = true;
                    }
                }

                if (!hasBaseImage)
                {
                    roots.Add(node);
                }
            }

            return roots;
        }
    }
}
