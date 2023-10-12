using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Core.Attributes
{

    public class AllowedFileMimeType : ValidationAttribute
    {
        private readonly string[] _contentTypes;

        public AllowedFileMimeType(params string[] contentTypes)
        {
            _contentTypes = contentTypes;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var contentType = file.ContentType;
                if (!_contentTypes.Contains(contentType))
                {
                    return new ValidationResult(GetErrorMessage(validationContext));
                }
            }
            else if (value is IEnumerable<IFormFile> files)
            {
                foreach (var fileItem in files)
                {
                    var contentType = fileItem.ContentType;
                    if (!_contentTypes.Contains(contentType))
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
                return string.Format(msg, string.Join(" , ", _contentTypes));

            return string.IsNullOrEmpty(ErrorMessage) ? "This file extension is not allowed!" : ErrorMessage;
        }
    }
}
