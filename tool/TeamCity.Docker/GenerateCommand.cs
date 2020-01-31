// ReSharper disable ClassNeverInstantiated.Global

using System;
using System.Threading.Tasks;
using IoC;

namespace TeamCity.Docker
{
    internal class GenerateCommand: ICommand<IGenerateOptions>
    {
        private readonly ILogger _logger;

        public GenerateCommand(
            [NotNull] ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<Result> Run()
        {
            return Task.FromResult(Result.Success);
        }
    }
}
