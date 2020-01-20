namespace TeamCity.Docker
{
    internal interface IDockerConverter
    {
        string? TryConvertRepoTagToTag(string repoTag);

        string? TryConvertRepoTagToRepositoryName(string repoTag);

        string? TryConvertConvertHashToImageId(string hash);
    }
}