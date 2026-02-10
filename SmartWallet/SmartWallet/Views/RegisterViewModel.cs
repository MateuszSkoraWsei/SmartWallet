using System.ComponentModel.DataAnnotations;

namespace SmartWallet.Views
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Imie i Nazwisko")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

           [Required]
           [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hasła nie są takie same.")]
        [Display(Name = "Potwierdź Hasło")]
        public string ConfirmPassword { get; set; }
    }
}
