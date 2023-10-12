using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Core.Attributes
{
    public class MinCountOfElementsAttribute : ValidationAttribute
    {
        private readonly int _minElements;

        public MinCountOfElementsAttribute(int minElements)
        {
            this._minElements = minElements;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var list = value as IList;
            var result = list?.Count >= _minElements;

            return result
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage);
        }
    }
}
