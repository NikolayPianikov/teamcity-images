using System;
using IoC;

namespace TeamCity.Docker.Generate
{
    internal struct DockerVariable
    {
        [NotNull] public readonly string Name;
        [NotNull] public readonly string Value;

        public DockerVariable([NotNull] string name, [NotNull] string value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
