
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using FileInfo = Core.Models.FileInfo;
namespace Core.Extensions
{
    public static class FormFileExtensions
    {
        public static string GenerateRandomFileName(string fileExtension)
        {
            var randomFileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var uniquePrefix = "_" + Guid.NewGuid().ToString().Substring(0, 4);
            return $"{randomFileName}_{uniquePrefix}{fileExtension}";
        }
        public static FileInfo ToFileInfo(this IFormFile file)
        {

            var f = new FileInfo()
            {
                Name = Path.GetFileNameWithoutExtension(file.FileName),
                FileName = file.FileName,
                FileExtension = Path.GetExtension(file.FileName),
                ContentType = file.ContentType,
                Size = file.Length,

            };
            return f;
        }
    }
}
