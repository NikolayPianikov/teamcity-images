using System;

namespace TeamCity.Docker
{
    internal interface ILogger
    {
        void Log(string text, Result result = Result.Success);

        IDisposable CreateBlock(string blockName);
    }
}
