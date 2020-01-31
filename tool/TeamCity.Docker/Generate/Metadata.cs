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
        [NotNull] public readonly IEnumerable<Dependency> Dependencies;
        [NotNull] public readonly IEnumerable<string> Components;
        public readonly IReadOnlyCollection<string> Repos;

        public Metadata([NotNull] string imageId,
            [NotNull] IReadOnlyCollection<string> tags,
            [NotNull] IReadOnlyCollection<Dependency> dependencies,
            [NotNull] IReadOnlyCollection<string> components,
            [NotNull] IReadOnlyCollection<string> repos)
        {
            ImageId = imageId ?? throw new ArgumentNullException(nameof(imageId));
            Tags = (tags ?? throw new ArgumentNullException(nameof(tags))).Select(tag => $"{imageId}:{tag}");
            Dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
            Components = components ?? throw new ArgumentNullException(nameof(components));
            Repos = repos ?? throw new ArgumentNullException(nameof(repos));
        }
    }
}
