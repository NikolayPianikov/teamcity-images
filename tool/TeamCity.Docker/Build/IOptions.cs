﻿using System.Collections.Generic;
using IoC;

namespace TeamCity.Docker.Build
{
    internal interface IOptions : Docker.IOptions
    {
        [NotNull] string ContextPath { get; }

        [NotNull] IEnumerable<string> Tags { get; }

        [NotNull] string ServerAddress { get; }

        [NotNull] string Username { get; }

        [NotNull] string Password { get; }

        bool Clean { get; }
    }
}
