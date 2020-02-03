using System.Collections.Generic;

namespace TeamCity.Docker.Model
{
    internal class Readme: IArtifact
    {
        public readonly string Path;
        public readonly IEnumerable<string> Lines;

        public Readme(string path, IEnumerable<string> lines)
        {
            Path = path;
            Lines = lines;
        }

        public Weight Weight => Weight.Empty;

        public override string ToString() => Path;
    }
}
