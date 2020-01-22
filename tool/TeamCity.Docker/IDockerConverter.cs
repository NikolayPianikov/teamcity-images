﻿using IoC;

namespace TeamCity.Docker
{
    internal interface IDockerConverter
    {
        [CanBeNull] string TryConvertRepoTagToTag(string repoTag);

        [CanBeNull] string TryConvertRepoTagToRepositoryName(string repoTag);

        [CanBeNull] string TryConvertConvertHashToImageId(string hash);
    }
}