using IoC;

namespace TeamCity.Docker.Push
{
    internal interface IOptions: Docker.IOptions
    {
        [NotNull] string ServerAddress { get; }

        [NotNull] string Username { get; }

        [NotNull] string Password { get; }

        bool Clean { get; }
    }
}
