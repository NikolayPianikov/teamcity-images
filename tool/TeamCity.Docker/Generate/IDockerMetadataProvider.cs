namespace TeamCity.Docker.Generate
{
    internal interface IDockerMetadataProvider
    {
        Metadata GetMetadata(string dockerFileContent);
    }
}