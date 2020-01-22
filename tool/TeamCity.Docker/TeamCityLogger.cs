using System;
using System.Collections.Generic;
using IoC;
using JetBrains.TeamCity.ServiceMessages.Write.Special;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker
{
    internal class TeamCityLogger: ILogger, IDisposable
    {
        private readonly Stack<ITeamCityWriter>_teamCityWriters = new Stack<ITeamCityWriter>();

        public TeamCityLogger([NotNull] ITeamCityWriter teamCityWriter)
        {
            if (teamCityWriter == null)
            {
                throw new ArgumentNullException(nameof(teamCityWriter));
            }

            _teamCityWriters.Push(teamCityWriter);
        }

        private ITeamCityWriter TeamCityWriter => _teamCityWriters.Peek();

        public void Log(string text, Result result = Result.Success)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            switch (result)
            {
                case Result.Success:
                    TeamCityWriter.WriteMessage(text);
                    break;

                case Result.Error:
                    TeamCityWriter.WriteError(text);
                    break;

                case Result.Warning:
                    TeamCityWriter.WriteWarning(text);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }
        }

        public IDisposable CreateBlock(string blockName)
        {
            if (blockName == null)
            {
                throw new ArgumentNullException(nameof(blockName));
            }

            _teamCityWriters.Push(TeamCityWriter.OpenBlock(blockName));
            return this;
        }

        public void Dispose()
        {
            if (_teamCityWriters.Count > 1)
            {
                _teamCityWriters.Pop().Dispose();
            }
        }
    }
}
