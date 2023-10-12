using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class PathResponse
    {
        public string PhysicalPath { get; set; }
        public string VirtualPath { get; set; }
    }
    public class FileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _uploadDirectory;
        private readonly string _developmentDirectory;
        private readonly string _productionDirectory;
        private readonly bool _isProduction;

        public FileService(IWebHostEnvironment environment)
        {
            _isProduction = environment.IsProduction();
            //tests only
            //_isProduction = true;

            _environment = environment;
            _developmentDirectory = environment.ContentRootPath;
            _productionDirectory = environment.ContentRootPath;
            _uploadDirectory = Path.Combine(_isProduction ? _productionDirectory : _developmentDirectory, "Uploads");

        }

        public async Task<PathResponse> SaveAsync(string folderPath, IFormFile file)
        {
            var folderDir = Path.Combine(_uploadDirectory, folderPath);

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
            var fileExtension = Path.GetExtension(file.FileName);
            var uniquePrefix = GetUniqueString();
            var uniqueFileName = fileNameWithoutExtension + uniquePrefix + fileExtension;

            var path = Path.Combine(folderDir, uniqueFileName);
            CreateIfMissing(path);

            await using var stream = File.Create(path);
            await file.CopyToAsync(stream).ConfigureAwait(false);
            return new PathResponse()
            {
                PhysicalPath = path,
                VirtualPath = ResolveVirtual(path),
            };

        }

        public Stream GetFile(string path)
        {
            return new FileStream(path ?? string.Empty, FileMode.Open, FileAccess.ReadWrite);
        }
        public string ResolveVirtual(string physicalPath)
        {

            string url = physicalPath.Substring(this._developmentDirectory.Length).Replace('\\', '/').Insert(0, "~");
            return (url);
        }
        public void CreateIfMissing(string path)
        {
            path = Path.GetDirectoryName(path);
     
            var folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);

        }

        public void Delete(params string[] paths)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            foreach (var path in paths)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
        private static string GetUniqueString()
        {
            return "_" + Guid.NewGuid().ToString().Substring(0, 4);
        }

    }
}
