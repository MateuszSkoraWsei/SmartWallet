using System.ComponentModel.DataAnnotations;

namespace SmartWallet.Attributes
{
    public class AllowExtensionsAttribure : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowExtensionsAttribure(string[] extensions)
        {
            _extensions = extensions;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if( value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                if(!_extensions.Contains(extension))
                {
                    return new ValidationResult("Niedozwolony format pliku.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
