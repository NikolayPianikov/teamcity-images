using Docker.DotNet.Models;

namespace TeamCity.Docker
{
    internal interface IMessageLogger
    {
        Result Log(JSONMessage message);

        Result Log(string jsonMessage);
    }
}
