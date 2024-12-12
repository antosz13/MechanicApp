using System.ComponentModel.DataAnnotations;

namespace MechanicApp.Shared
{
    public class WhiteSpaceAndNullVerification : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string)
            {
                if (string.IsNullOrWhiteSpace(value as string))
                {
                    return new ValidationResult("This field can't be empty or contain only white spaces.");
                }
            }
            return ValidationResult.Success;
            
        }

    }
}
