using System.Threading.Tasks;
// ReSharper disable UnusedTypeParameter

namespace TeamCity.Docker
{
    internal interface ICommand<in TOptions>
    {
        Task<Result> Run();
    }
}
