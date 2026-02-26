using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SmartWallet.Attributes
{
    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;   

        public MinimumAgeAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
            
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return ValidationResult.Success;

            DateTime birthDate = (DateTime)value;

            if (birthDate > DateTime.Today)
            {
                return new ValidationResult("Data urodzenia z przyszłości");
            }
            if (birthDate > DateTime.Today.AddYears(-_minimumAge))
            {
                return new ValidationResult($"Musisz mieć co najmniej {_minimumAge} lat.");

            }
            
            return ValidationResult.Success;
        }
    }
}
