using System.ComponentModel.DataAnnotations;

namespace SmartWallet.Attributes
{
    public class MaxFileSize : ValidationAttribute
    {
        private readonly long _maxFileSize;
        public MaxFileSize(long maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult($"Plik jest zbyt duży. Maksymalny rozmiar to {_maxFileSize / (1024 * 1024)} MB.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
