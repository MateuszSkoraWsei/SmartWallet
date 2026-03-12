using SmartWallet.Models;
using System.ComponentModel.DataAnnotations;
using SmartWallet.Attributes;

namespace SmartWallet.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
       
        [Required]
        [MaxLength(32)]
        [Display(Name = "Imię")]
        public string Name { get; set; }

        [Required]
        [MaxLength(51)]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Data urodzenia jest wymagana")]
        [DataType(DataType.Date)]
        [Display(Name = "Data urodzenia")]
        [MinimumAge(18)]
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

        [MustBeTrue(ErrorMessage = "Musisz zaakceptować politykę prywatności")]
        [Display(Name = "Akceptuję politykę prywatności")]
        public bool PrivacyPolicy { get; set; }
    }
}
