using LaXiS.MyLibrary.Models;
using LaXiS.MyLibrary.Repositories;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Security.Cryptography;

namespace LaXiS.MyLibrary.Jobs
{
    public class ScanJob : IJob
    {
        private readonly ILogger _logger;
        private readonly MyLibraryOptions _options;
        private readonly FileRepository _fileRepository;

        public ScanJob(
            ILogger<ScanJob> logger,
            IOptions<MyLibraryOptions> options,
            FileRepository fileRepository)
        {
            _logger = logger;
            _options = options.Value;
            _fileRepository = fileRepository;
        }

        public async Task ExecuteAsync()
        {
            int totalFiles = 0;
            int oldFiles = 0;

            _logger.LogInformation("Scan started");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var dirInfo = new DirectoryInfo(_options.ContentPath);
            var enumOptions = new EnumerationOptions
            {
                RecurseSubdirectories = true
            };
            foreach (var fileInfo in dirInfo.EnumerateFiles("*", enumOptions))
            {
                // TODO filter only supported files
                // TODO to better handle moved files, the parent directory could be saved as a a separate document and then added as a DbRef to the file
                // TODO parallelize

                totalFiles++;

                var file = new Models.File
                {
                    Name = fileInfo.Name,
                    Path = fileInfo.DirectoryName
                };

                if (_fileRepository.Exists(file))
                {
                    oldFiles++;
                    continue;
                }

                // Calculate MD5 hash
                var bytes = await System.IO.File.ReadAllBytesAsync(fileInfo.FullName);
                file.Md5Hash = MD5.HashData(bytes);

                _fileRepository.Add(file);
            }
            stopwatch.Stop();

            _logger.LogInformation("Scan finished in {Elapsed}. {New} new out of {Total}", stopwatch.Elapsed, totalFiles - oldFiles, totalFiles);
        }
    }
}
