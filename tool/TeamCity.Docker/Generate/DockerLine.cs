using System.Collections.Generic;

namespace TeamCity.Docker.Generate
{
    internal struct DockerLine
    {
        public readonly string Text;
        public readonly DockerLineType Type;
        public readonly IEnumerable<DockerVariable> Variables;

        public DockerLine(string text, DockerLineType type, IReadOnlyCollection<DockerVariable> variables)
        {
            Text = text;
            Type = type;
            Variables = variables;
        }
    }
}
