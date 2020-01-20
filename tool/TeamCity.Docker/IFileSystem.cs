using System.Collections.Generic;
using System.IO;

namespace TeamCity.Docker
{
    internal interface IFileSystem
    {
        string UniqueName { get; }

        bool IsDirectoryExists(string path);

        IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern);

        IEnumerable<string> ReadLines(string path);

        string ReadFile(string path);

        void WriteFile(string path, string content);

        Stream OpenRead(string path);

        Stream OpenWrite(string path);
    }
}