using System;
using System.Threading.Tasks;
using IoC;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable StaticMemberInGenericType

namespace TeamCity.Docker
{
    internal class TaskRunner<TState>: ITaskRunner<TState>, IDisposable
        where TState: IDisposable
    {
        [NotNull] private readonly IOptions _options;
        [NotNull] private readonly ILogger _logger;
        [NotNull] private readonly Func<TState> _stateFactory;
        [CanBeNull] private TState _state;
        private int _curAttempt;
        private bool _hasError;

        public TaskRunner(
            [NotNull] IOptions options,
            [NotNull] ILogger logger,
            [NotNull] Func<TState> stateFactory)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _stateFactory = stateFactory ?? throw new ArgumentNullException(nameof(stateFactory));
        }

        private int Attempts => _options.Retries > 0 ? _options.Retries : 1;

        public async Task<Result> Run(Func<TState, Task> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            var result = await Run(state =>
            {
                var task = handler(state);
                task.Wait();
                return Task.FromResult(0);
            });

            return result.State;
        }
        public async Task<Result<T>> Run<T>([NotNull] Func<TState, Task<T>> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            for (_curAttempt= 0; _curAttempt < Attempts; _curAttempt++)
            {
                _hasError = false;
                Exception lastError = null;
                try
                {
                    var result = await handler(GetState());
                    if (!_hasError)
                    {
                        return new Result<T>(result);
                    }
                }
                catch (Exception error)
                {
                    lastError = error;
                }

                var message = $"Attempt {_curAttempt + 1:00} of {Attempts:00} failed.";
                if (lastError != null)
                {
                    _logger.Log(lastError, message);
                }
                else
                {
                    _logger.Log(message, Result.Warning);
                }

                _state?.Dispose();
                _state = default(TState);
            }

            _logger.Log("Attempts have been exhausted.", Result.Error);
            return new Result<T>(default(T), Result.Error);
        }

        public void Dispose() => _state?.Dispose();

        public void Log(string text, Result result = Result.Success)
        {
            if (_curAttempt == Attempts - 1)
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
                        _hasError = true;
                        _logger.Log(text, Result.Warning);
                        break;

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
    }
}
