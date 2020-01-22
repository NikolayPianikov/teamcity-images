using System;
using System.Collections.Generic;
using IoC;

namespace TeamCity.Docker.Generate
{
    internal struct DockerLine
    {
        [NotNull] public readonly string Text;
        public readonly DockerLineType Type;
        [NotNull] public readonly IEnumerable<DockerVariable> Variables;

        public DockerLine([NotNull] string text, DockerLineType type, [NotNull] IReadOnlyCollection<DockerVariable> variables)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Type = type;
            Variables = variables ?? throw new ArgumentNullException(nameof(variables));
        }
    }
}
