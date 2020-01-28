using System;
using System.Threading.Tasks;
using IoC;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable StaticMemberInGenericType

namespace TeamCity.Docker
{
    internal class TaskRunner<TState>: ITaskRunner<TState> where TState: IDisposable
    {
        [NotNull] private static readonly Exception DefaultException = new InvalidOperationException("");
        [NotNull] private readonly IOptions _options;
        [NotNull] private readonly ILogger _logger;
        [NotNull] private readonly Func<TState> _stateFactory;
        [CanBeNull] private TState _state;
        private int _curAttempt;

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

        public async Task Run(Func<TState, Task> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            await Run(state =>
            {
                handler(state).Wait();
                return Task.FromResult(0);
            });
        }
        public async Task<T> Run<T>([NotNull] Func<TState, Task<T>> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            Exception lastError = null;
            for (_curAttempt= 0; _curAttempt < Attempts; _curAttempt++)
            {
                try
                {
                    return await handler(GetState());
                }
                catch (Exception error)
                {
                    lastError = error;
                    _logger.Log(error, $"Attempt #{_curAttempt + 1:00}/{Attempts:00} failed.");
                }

                _state?.Dispose();
                _state = default;
            }

            _logger.Log("Attempts have been exhausted.", Result.Error);
            throw lastError ?? DefaultException;
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
