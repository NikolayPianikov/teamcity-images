using System;
using System.IO;
using System.Threading.Tasks;

namespace TeamCity.Docker
{
    internal interface IStreamService
    {
        Task Copy(Stream sourceStream, Stream targetStream);

        void ProcessLines(Stream source, Action<string> handler);
    }
}