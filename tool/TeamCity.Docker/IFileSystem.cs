using System.Collections.Generic;
using System.IO;
// ReSharper disable UnusedMember.Global

namespace TeamCity.Docker
{
    internal interface IFileSystem
    {
        string UniqueName { get; }

        bool IsDirectoryExist(string path);

        bool IsFileExist(string path);

        IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern);

        IEnumerable<string> ReadLines(string path);

        string ReadFile(string path);

        void WriteFile(string path, string content);

        Stream OpenRead(string path);

        Stream OpenWrite(string path);
    }
}