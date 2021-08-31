using System;
using System.IO;

namespace Sat.Recruitment.ResourceAccess.FileManager
{
    public interface IFileManager
    {
        StreamReader ReadFile(string directory);
    }

    public class FileManager : IFileManager
    {
        public StreamReader ReadFile(string directory)
        {
            var path = $"{Directory.GetCurrentDirectory()}{directory}";

            FileStream fileStream = new FileStream(path, FileMode.Open);

            StreamReader reader = new StreamReader(fileStream);
            return reader;
        }
    }
}
