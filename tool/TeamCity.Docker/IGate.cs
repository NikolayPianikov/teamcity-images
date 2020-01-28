using System;
using System.Threading.Tasks;
using IoC;

namespace TeamCity.Docker
{
    internal interface IGate<out TState> : IDisposable, ILogger where TState : IDisposable
    {
        [NotNull] Task<Result> Run(Func<TState, Task<Result>> handler);

        [NotNull] Task<Result<T>> Run<T>([NotNull] Func<TState, Task<Result<T>>> handler);
    }
}