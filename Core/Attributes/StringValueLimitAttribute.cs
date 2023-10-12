using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Attributes
{

    public class StringValueLimitAttribute : ValidationAttribute
    {
        private string[] _requiredValues;

        public StringValueLimitAttribute(params string[] requiredValues)
        {
            _requiredValues = requiredValues;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string val)
            {
                if (!_requiredValues.Contains(val.ToLower(), StringComparer.OrdinalIgnoreCase)) return new ValidationResult(ErrorMessage);
                return ValidationResult.Success;
            }
            else if (value is null)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
