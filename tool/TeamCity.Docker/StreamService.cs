using System;
using System.Buffers;
using System.IO;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker
{
    internal class StreamService : IStreamService
    {
        public async Task Copy(Stream sourceStream, Stream targetStream)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(0xffff);
            while (true)
            {
                var bytes = await sourceStream.ReadAsync(buffer, 0, buffer.Length);
                if (bytes <= 0)
                {
                    break;
                }

                targetStream.Write(buffer, 0, bytes);
            }
        }

        public void ProcessLines(Stream source, Action<string> handler)
        {
            using var streamReader = new StreamReader(source, Encoding.UTF8);
            do
            {
                var line = streamReader.ReadLine();
                if (line != null)
                {
                    handler(line);
                }
            }
            while (!streamReader.EndOfStream);
        }
    }
}
