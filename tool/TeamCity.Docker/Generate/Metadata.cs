using System.Collections.Generic;
using System.Linq;
// ReSharper disable ParameterTypeCanBeEnumerable.Local

namespace TeamCity.Docker.Generate
{
    internal struct Metadata
    {
        public readonly int Priority;
        public readonly string ImageId;
        public readonly IEnumerable<string> Tags;
        public readonly IEnumerable<string> BaseImages;
        public readonly IEnumerable<string> Components;

        public Metadata(
            int priority,
            string imageId,
            IReadOnlyCollection<string> tags,
            IReadOnlyCollection<string> baseImages,
            IReadOnlyCollection<string> components)
        {
            Priority = priority;
            ImageId = imageId;
            Tags = tags.Select(tag => $"{imageId}:{tag}");
            BaseImages = baseImages;
            Components = components;
        }
    }
}
