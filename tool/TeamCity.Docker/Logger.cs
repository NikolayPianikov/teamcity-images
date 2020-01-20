using System;

namespace TeamCity.Docker
{
    internal class Logger: ILogger
    {
        private readonly ILogger _logger;

        public Logger(
            IEnvironment environment,
            ILogger consoleLogger,
            ILogger teamCityLogger) =>
            _logger = environment.HasEnvironmentVariable("TEAMCITY_VERSION") ? teamCityLogger : consoleLogger;

        public void Log(string text, Result result = Result.Success) => _logger.Log(text, result);

        public IDisposable CreateBlock(string blockName) => _logger.CreateBlock(blockName);
    }
}
