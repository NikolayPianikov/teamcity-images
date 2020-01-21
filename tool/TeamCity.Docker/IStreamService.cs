using System;
using System.IO;
using System.Threading.Tasks;

namespace TeamCity.Docker
{
    internal interface IStreamService
    {
        Task<Result> Copy(Stream sourceStream, Stream targetStream, string description = "");

        void ProcessLines(Stream source, Action<string> handler);
    }
}
