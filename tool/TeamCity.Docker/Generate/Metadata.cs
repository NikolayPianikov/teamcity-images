using System;
using System.Collections.Generic;
using System.Linq;
using IoC;

// ReSharper disable ParameterTypeCanBeEnumerable.Local

namespace TeamCity.Docker.Generate
{
    internal struct Metadata
    {
        public readonly int Priority;
        [NotNull] public readonly string ImageId;
        [NotNull] public readonly IEnumerable<string> Tags;
        [NotNull] public readonly IEnumerable<string> BaseImages;
        [NotNull] public readonly IEnumerable<string> Components;

        public Metadata(
            int priority,
            [NotNull] string imageId,
            [NotNull] IReadOnlyCollection<string> tags,
            [NotNull] IReadOnlyCollection<string> baseImages,
            [NotNull] IReadOnlyCollection<string> components)
        {
            Priority = priority;
            ImageId = imageId ?? throw new ArgumentNullException(nameof(imageId));
            Tags = (tags ?? throw new ArgumentNullException(nameof(tags))).Select(tag => $"{imageId}:{tag}");
            BaseImages = baseImages ?? throw new ArgumentNullException(nameof(baseImages));
            Components = components ?? throw new ArgumentNullException(nameof(components));
        }
    }
}
