using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using IoC;
using TeamCity.Docker.Generate;

namespace TeamCity.Docker.Build
{
    internal interface IContextFactory
    {
        [NotNull] Task<Result<Stream>> Create([NotNull] string dockerFilesRootPath, [NotNull] IEnumerable<DockerFile> dockerFiles);
    }
}