using System;
using System.Collections.Generic;
using System.IO;
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker
{
    internal class FileSystem : IFileSystem
    {
        public string UniqueName =>
            Guid.NewGuid().ToString().Replace("-", string.Empty).Replace("{", string.Empty);

        public bool IsDirectoryExist(string path) => Directory.Exists(path);

        public bool IsFileExist(string path) => File.Exists(path);

        public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern) =>
            Directory.EnumerateFileSystemEntries(path, searchPattern, SearchOption.AllDirectories);

        public IEnumerable<string> ReadLines(string path) => File.ReadLines(path);

        public string ReadFile(string path) => File.ReadAllText(path);

        public void WriteFile(string path, string content)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, content);
        }

        public Stream OpenRead(string path) => File.OpenRead(path);

        public Stream OpenWrite(string path) => File.OpenWrite(path);
    }
}
