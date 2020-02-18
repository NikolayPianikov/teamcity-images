﻿using IoC;

namespace TeamCity.Docker
{
    internal interface IGenerateOptions: IOptions
    {
        [NotNull] string TargetPath { get; }

        [NotNull] string TeamCityDslPath { get; }

        [NotNull] string TeamCityBuildConfigurationId { get; }
    }
}
