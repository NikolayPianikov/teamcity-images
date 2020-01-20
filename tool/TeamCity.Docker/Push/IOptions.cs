namespace TeamCity.Docker.Push
{
    internal interface IOptions: Docker.IOptions
    {
        string ServerAddress { get; }

        string Username { get; }

        string Password { get; }

        bool Clean { get; }
    }
}
