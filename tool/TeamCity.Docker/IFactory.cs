namespace TeamCity.Docker
{
    internal interface IFactory<T, in TState>
    {
        Result<T> Create(TState state);
    }
}
