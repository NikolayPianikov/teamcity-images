using System;
using System.Collections.Generic;
using System.Linq;
using IoC;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Generate
{
    internal class DockerMetadataProvider : IDockerMetadataProvider
    {
        private const string PriorityPrefix = "# Priority ";
        private const string IdPrefix = "# Id ";
        private const string TagPrefix = "# Tag ";
        private const string BaseImagePrefix = "# Based on ";
        private const string ComponentsPrefix = "# Install ";
        private const string RepoPrefix = "# Repo ";

        public Metadata GetMetadata(IEnumerable<DockerLine> dockerFileContent)
        {
            if (dockerFileContent == null)
            {
                throw new ArgumentNullException(nameof(dockerFileContent));
            }

            var priority = int.MaxValue;
            var imageId = "unknown";
            var tags = new List<string>();
            var baseImages = new List<string>();
            var components = new List<string>();
            var repos = new List<string>();
            foreach (var comment in dockerFileContent.Where(line => line.Type == DockerLineType.Comment))
            {
                if (
                    TrySetByPrefix(
                        comment.Text,
                        PriorityPrefix, 
                        value =>
                        {
                            if (int.TryParse(value, out var curPriority))
                            {
                                priority = curPriority;
                            }
                        }) ||
                    TrySetByPrefix(comment.Text, IdPrefix, value => imageId = value) ||
                    TrySetByPrefix(comment.Text, TagPrefix, value => tags.Add(value)) ||
                    TrySetByPrefix(comment.Text, BaseImagePrefix, value => baseImages.Add(value)) ||
                    TrySetByPrefix(comment.Text, ComponentsPrefix, value => components.Add(value)) ||
                    TrySetByPrefix(comment.Text, RepoPrefix, value => repos.Add(value)))
                { }
            }

            return new Metadata(priority, imageId, tags, baseImages, components, repos);
        }

        private static bool TrySetByPrefix([NotNull] string text, [NotNull] string prefix, Action<string> setter)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            if (text.StartsWith(prefix))
            {
                var value = text.Substring(prefix.Length).Trim();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    setter(value);
                }

                return true;
            }

            return false;
        }
    }
}
