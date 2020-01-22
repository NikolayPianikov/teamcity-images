using System;
using IoC;

namespace TeamCity.Docker
{
    internal class Logger: ILogger
    {
        private readonly ILogger _logger;

        public Logger(
            [NotNull] IEnvironment environment,
            [NotNull] ILogger consoleLogger,
            [NotNull] ILogger teamCityLogger)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            if (consoleLogger == null)
            {
                throw new ArgumentNullException(nameof(consoleLogger));
            }

            if (teamCityLogger == null)
            {
                throw new ArgumentNullException(nameof(teamCityLogger));
            }

            _logger = environment.HasEnvironmentVariable("TEAMCITY_VERSION") ? teamCityLogger : consoleLogger;
        }

        public void Log(string text, Result result = Result.Success)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            _logger.Log(text, result);
        }

        public IDisposable CreateBlock(string blockName)
        {
            if (blockName == null)
            {
                throw new ArgumentNullException(nameof(blockName));
            }

            return _logger.CreateBlock(blockName);
        }
    }
}
