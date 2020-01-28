using System;
using System.Threading.Tasks;
using IoC;
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker
{
    internal class Gate<TState>: IGate<TState> where TState: IDisposable
    {
        [NotNull] private readonly IOptions _options;
        [NotNull] private readonly ILogger _logger;
        [NotNull] private readonly Func<TState> _stateFactory;
        [CanBeNull] private TState _state;
        private int _cutAttempt;

        public Gate(
            [NotNull] IOptions options,
            [NotNull] ILogger logger,
            [NotNull] Func<TState> stateFactory)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _stateFactory = stateFactory ?? throw new ArgumentNullException(nameof(stateFactory));
        }

        private int Attempts => _options.Retries > 0 ? _options.Retries : 1;

        public async Task<Result> Run(Func<TState, Task<Result>> handler)
        {
            var result = await Run(async state =>
            {
                var res = await handler(state);
                return new Result<EmptyResult>(EmptyResult.Shared, res);
            });

            return result.State;
        }

        public async Task<Result<T>> Run<T>(Func<TState, Task<Result<T>>> handler)
        {
            Exception lastError = null;
            Result<T> lastResult = default;
            for (_cutAttempt= 0; _cutAttempt < Attempts; _cutAttempt++)
            {
                using (_logger.CreateBlock($"{_cutAttempt + 1} attempt"))
                {
                    try
                    {
                        lastResult = await handler(GetState());
                        if (lastResult.State != Result.Error)
                        {
                            return lastResult;
                        }
                    }
                    catch (Exception error)
                    {
                        lastError = error;
                        _logger.Log(error);
                    }

                    _state?.Dispose();
                    _state = default;
                }
            }

            if (lastError != null)
            {
                throw lastError;
            }

            return lastResult;
        }

        public void Dispose() => _state?.Dispose();

        public void Log(string text, Result result = Result.Success)
        {
            if (_cutAttempt == Attempts - 1)
            {
                _logger.Log(text, result);
            }
            else
            {
                switch (result)
                {
                    case Result.Success:
                        _logger.Log(text);
                        break;

                    case Result.Error:
                    case Result.Warning:
                        _logger.Log(text, Result.Warning);
                        break;
                }
            }
        }

        public IDisposable CreateBlock(string blockName) => _logger.CreateBlock(blockName);

        private TState GetState()
        {
            if (_state == null)
            {
                _state = _stateFactory();
            }

            return _state;
        }

        private class EmptyResult
        {
            public static EmptyResult Shared = new EmptyResult();
        }
    }
}
