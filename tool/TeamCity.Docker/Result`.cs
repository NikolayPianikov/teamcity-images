namespace TeamCity.Docker
{
    internal struct Result<T>
    {
        public readonly T Value;
        public readonly Result State;

        public Result(T value, Result state = Result.Success)
        {
            Value = value;
            State = state;
        }
    }
}
