using System;
using System.Collections.Generic;
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

        public Metadata GetMetadata(string dockerFileContent)
        {
            var priority = int.MaxValue;
            var imageId = "unknown";
            var tags = new List<string>();
            var baseImages = new List<string>();
            var components = new List<string>();
            foreach (var line in dockerFileContent.Split("\n"))
            {
                if (
                    TrySetByPrefix(
                        line,
                        PriorityPrefix, 
                        value =>
                        {
                            if (int.TryParse(value, out var curPriority))
                            {
                                priority = curPriority;
                            }
                        }) ||
                    TrySetByPrefix(line, IdPrefix, value => imageId = value) ||
                    TrySetByPrefix(line, TagPrefix, value => tags.Add(value)) ||
                    TrySetByPrefix(line, BaseImagePrefix, value => baseImages.Add(value)) ||
                    TrySetByPrefix(line, ComponentsPrefix, value => components.Add(value)))
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
