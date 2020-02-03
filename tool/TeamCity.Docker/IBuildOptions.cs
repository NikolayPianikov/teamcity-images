using System.Collections.Generic;
using IoC;

namespace TeamCity.Docker
{
    internal interface IBuildOptions : IOptions
    {
        [NotNull] IEnumerable<string> Tags { get; }

        [NotNull] string ServerAddress { get; }

        [NotNull] string Username { get; }

        [NotNull] string Password { get; }

        bool Clean { get; }
    }
}
