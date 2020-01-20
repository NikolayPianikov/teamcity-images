using System.Threading.Tasks;
using Docker.DotNet;

namespace TeamCity.Docker
{
    internal interface IDockerClientFactory
    {
        Task<IDockerClient> Create();
    }
}
