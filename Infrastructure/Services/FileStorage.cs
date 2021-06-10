using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class FileStorage : IFileStorage
    {
        private const string BASE_FILE_PATH_NAME = "DeratFolder";

        private readonly IConfiguration configuration;

        public FileStorage(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<byte[]> ReadFile(string path)
        {
            var sections = path.Split(Path.DirectorySeparatorChar);
            var fileName = sections[^1];

            var fullPath = Path.Combine(configuration[BASE_FILE_PATH_NAME], path.Replace(fileName, string.Empty));

            if (Directory.Exists(fullPath) == false)
                return Task.FromResult(Array.Empty<byte>());

            if (File.Exists(Path.Combine(fullPath, fileName)))
                return File.ReadAllBytesAsync(Path.Combine(configuration[BASE_FILE_PATH_NAME], path));

            return Task.FromResult(Array.Empty<byte>());
        }

        public Task Remove(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return Task.CompletedTask;

            var fullPath = Path.Combine(configuration[BASE_FILE_PATH_NAME], path);

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }

        public Task SaveFile(string path, byte[] file)
        {
            if (file is null)
                throw new ArgumentNullException(nameof(file));

            var sections = path.Split(Path.DirectorySeparatorChar);
            var fileName = sections[^1];

            var fullPath = Path.Combine(configuration[BASE_FILE_PATH_NAME], path.Replace(fileName, string.Empty));

            if (Directory.Exists(fullPath) == false)
                Directory.CreateDirectory(fullPath);

            return File.WriteAllBytesAsync(Path.Combine(fullPath, fileName), file);
        }
    }
}
