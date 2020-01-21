using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TeamCity.Docker.Generate;

namespace TeamCity.Docker.Build
{
    internal interface IContextFactory
    {
        Task<Result<Stream>> Create(string dockerFilesRootPath, IEnumerable<DockerFile> dockerFiles);
    }
}