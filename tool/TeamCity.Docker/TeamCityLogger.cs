using System;
using System.Collections.Generic;
using JetBrains.TeamCity.ServiceMessages.Write.Special;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker
{
    internal class TeamCityLogger: ILogger, IDisposable
    {
        private readonly Stack<ITeamCityWriter>_teamCityWriters = new Stack<ITeamCityWriter>();

        public TeamCityLogger(ITeamCityWriter teamCityWriter) => _teamCityWriters.Push(teamCityWriter);

        private ITeamCityWriter TeamCityWriter => _teamCityWriters.Peek();

        public void Log(string text, Result result = Result.Success)
        {
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
