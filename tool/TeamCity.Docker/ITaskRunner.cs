using System;
using System.Threading.Tasks;
using IoC;

namespace TeamCity.Docker
{
    internal interface ITaskRunner<out TState> : IDisposable, ILogger where TState : IDisposable
    {
        [NotNull] Task Run([NotNull] Func<TState, Task> handler);

        [NotNull] Task<T> Run<T>(Func<TState, Task<T>> handler);
    }
}