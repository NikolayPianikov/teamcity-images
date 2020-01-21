using System;
using System.Collections.Generic;
using System.Linq;

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

        public Metadata GetMetadata(IEnumerable<DockerLine> dockerFileContent)
        {
            var priority = int.MaxValue;
            var imageId = "unknown";
            var tags = new List<string>();
            var baseImages = new List<string>();
            var components = new List<string>();
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
                    TrySetByPrefix(comment.Text, ComponentsPrefix, value => components.Add(value)))
                { }
            }

            return new Metadata(priority, imageId, tags, baseImages, components);
        }

        private static bool TrySetByPrefix(string text, string prefix, Action<string> setter)
        {
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
