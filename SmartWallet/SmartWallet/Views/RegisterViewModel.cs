using SmartWallet.Models;
using System.ComponentModel.DataAnnotations;

namespace SmartWallet.Views
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
       
        [Required]
        [Display(Name = "Imię")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data urodzenia")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Płeć")]
        public GenderType Gender { get; set; }
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
