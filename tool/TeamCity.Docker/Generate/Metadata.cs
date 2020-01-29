using System;
using System.Collections.Generic;
using System.Linq;
using IoC;

// ReSharper disable ParameterTypeCanBeEnumerable.Local

namespace TeamCity.Docker.Generate
{
    internal struct Metadata
    {
        [NotNull] public readonly string ImageId;
        [NotNull] public readonly IEnumerable<string> Tags;
        [NotNull] public readonly IEnumerable<string> BaseImages;
        [NotNull] public readonly IEnumerable<string> Components;
        public readonly IReadOnlyCollection<string> Repos;

        public Metadata([NotNull] string imageId,
            [NotNull] IReadOnlyCollection<string> tags,
            [NotNull] IReadOnlyCollection<string> baseImages,
            [NotNull] IReadOnlyCollection<string> components,
            [NotNull] IReadOnlyCollection<string> repos)
        {
            ImageId = imageId ?? throw new ArgumentNullException(nameof(imageId));
            Tags = (tags ?? throw new ArgumentNullException(nameof(tags))).Select(tag => $"{imageId}:{tag}");
            BaseImages = baseImages ?? throw new ArgumentNullException(nameof(baseImages));
            Components = components ?? throw new ArgumentNullException(nameof(components));
            Repos = repos ?? throw new ArgumentNullException(nameof(repos));
        }
    }
}
