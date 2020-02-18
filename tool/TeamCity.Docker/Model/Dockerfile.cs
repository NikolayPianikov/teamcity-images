﻿using System;
using System.Collections.Generic;
using IoC;

namespace TeamCity.Docker.Model
{
    internal struct Dockerfile
    {
        [NotNull] public readonly string Path;
        [NotNull] public readonly string ImageId;
        [NotNull] public readonly IEnumerable<string> Tags;
        [NotNull] public readonly IEnumerable<string> Components;
        [NotNull] public readonly IEnumerable<string> Repositories;
        [NotNull] public readonly IReadOnlyCollection<Reference> References;
        public readonly Weight Weight;
        [NotNull] public readonly IEnumerable<Line> Lines;

        public Dockerfile(
            [NotNull] string path,
            [NotNull] string imageId,
            [NotNull] IReadOnlyCollection<string> tags,
            [NotNull] IReadOnlyCollection<string> components,
            [NotNull] IReadOnlyCollection<string> repositories,
            [NotNull] IReadOnlyCollection<Reference> references,
            [NotNull] Weight weight,
            [NotNull] IReadOnlyCollection<Line> lines)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
            ImageId = imageId ?? throw new ArgumentNullException(nameof(imageId));
            Tags = tags ?? throw new ArgumentNullException(nameof(tags));
            Components = components ?? throw new ArgumentNullException(nameof(components));
            Repositories = repositories ?? throw new ArgumentNullException(nameof(repositories));
            References = references ?? throw new ArgumentNullException(nameof(references));
            Weight = weight;
            Lines = lines ?? throw new ArgumentNullException(nameof(lines));
        }

        public override string ToString() => $"{ImageId}:{string.Join(",", Tags)}";
    }
}