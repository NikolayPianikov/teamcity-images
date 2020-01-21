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
        private readonly ILogger _logger;

        public StreamService(ILogger logger) => _logger = logger;

        public async Task<Result> Copy(Stream sourceStream, Stream targetStream, string description)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(0xffff);
            while (true)
            {
                try
                {
                    var bytes = await sourceStream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytes <= 0)
                    {
                        break;
                    }

                    targetStream.Write(buffer, 0, bytes);
                }
                catch (Exception ex)
                {
                    _logger.Log($"{description}: {ex.Message}", Result.Error);
                    return Result.Error;
                }
            }

            return Result.Success;
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
