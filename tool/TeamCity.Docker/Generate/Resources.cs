using System;
using System.IO;
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Generate
{
    internal class Resources : IResources
    {
        public string GetResource(string resourceName)
        {
            using var stream = GetType().Assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException($"Cannot find resource \"{resourceName}\".");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
