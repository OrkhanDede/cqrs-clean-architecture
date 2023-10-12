using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Core.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Core.Attributes
{
    public class AllowedFileFormatAttribute : ValidationAttribute
    {
        private readonly string[] _formats;

        public AllowedFileFormatAttribute(params string[] formats)
        {
            _formats = formats;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_formats.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage(validationContext));
                }
            }
            else if (value is IEnumerable<IFormFile> files)
            {
                foreach (var fileItem in files)
                {
                    var extension = Path.GetExtension(fileItem.FileName);
                    if (!_formats.Contains(extension.ToLower()))
                    {
                        return new ValidationResult(GetErrorMessage(validationContext));
                    }
                }
            }
            return ValidationResult.Success;
        }
        private string GetErrorMessage(ValidationContext validationContext)
        {

            var stringLocalizer = validationContext.GetService(typeof(IStringLocalizer<Resource>)) as IStringLocalizer<Resource>;
            var msg = stringLocalizer[ErrorMessage];
            var isResourceMsg = !string.IsNullOrEmpty(msg) && msg != ErrorMessage;
            if (isResourceMsg)
                return string.Format(msg,string.Join(" , " , _formats));

            return string.IsNullOrEmpty(ErrorMessage) ? "This file extension is not allowed!" : ErrorMessage;
        }

    }
}
