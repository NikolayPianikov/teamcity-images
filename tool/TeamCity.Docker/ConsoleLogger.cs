using System;
using System.Linq;
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker
{
    internal class ConsoleLogger : ILogger, IDisposable
    {
        private readonly object _lockObject = new object();
        private int _blockCount;

        public void Log(string text, Result result = Result.Success)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            lock (_lockObject)
            {
                var foregroundColor = Console.ForegroundColor;
                try
                {
                    switch (result)
                    {
                        case Result.Error:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case Result.Warning:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                    }

                    var prefix = new string(Enumerable.Repeat(' ', _blockCount << 1).ToArray());
                    if (result == Result.Error)
                    {
                        Console.Error.WriteLine(prefix + text);
                    }
                    else
                    {
                        Console.WriteLine(prefix + text);
                    }
                }
                finally
                {
                    Console.ForegroundColor = foregroundColor;
                }
            }
        }

        public IDisposable CreateBlock(string blockName)
        {
            if (blockName == null)
            {
                throw new ArgumentNullException(nameof(blockName));
            }

            lock (_lockObject)
            {
                Log(blockName);
                _blockCount++;
                return this;
            }
        }

        public void Dispose()
        {
            lock (_lockObject)
            {
                _blockCount--;
            }
        }
    }
}