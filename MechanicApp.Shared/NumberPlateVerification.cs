using System.ComponentModel.DataAnnotations;

namespace MechanicApp.Shared
{
    public class NumberPlateVerification : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var numberPlate = value.ToString();
            
            if (numberPlate.Length != 7)
            {
                return new ValidationResult("Number plate must be 7 characters long.");
            }

            if (numberPlate[3] != '-')
            {
                return new ValidationResult("Number plate must have a dash at the 4th position.");
            }

            if (!char.IsLetter(numberPlate[0]) || !char.IsLetter(numberPlate[1]) || !char.IsLetter(numberPlate[2]) || 
                !char.IsUpper(numberPlate[0]) || !char.IsUpper(numberPlate[1]) || !char.IsUpper(numberPlate[2]) ||
                !char.IsDigit(numberPlate[4]) || !char.IsDigit(numberPlate[5]) || !char.IsDigit(numberPlate[6]))
            {
                return new ValidationResult("Number plate must have 3 capital letters, a dash, and 3 digits.");
            }
            return ValidationResult.Success;
        }

    }
}
