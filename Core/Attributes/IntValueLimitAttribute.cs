using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Attributes
{

    public class IntValueLimitAttribute : ValidationAttribute
    {
        private readonly int[] _requiredValues;

        public IntValueLimitAttribute(params int[] requiredValues)
        {
            _requiredValues = requiredValues;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && int.TryParse(value.ToString(), out var val))
            //if (value is int val)
            {
                if (!_requiredValues.Contains(val)) return new ValidationResult(ErrorMessage);
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
