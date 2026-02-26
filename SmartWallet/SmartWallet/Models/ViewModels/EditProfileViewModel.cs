using SmartWallet.Attributes;
using SmartWallet.Models;
using System.ComponentModel.DataAnnotations;

namespace SmartWallet.Models.ViewModels
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Imię jest wymagane")]
        [Display(Name = "Imię")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [Display(Name = "Adres")]
        public string? Address { get; set; }

        [Display(Name = "Płeć")]
        public GenderType Gender { get; set; }

        

        public string? ExistingAvatarUrl { get; set; }

        [MaxFileSize(2 * 1024 * 1024)]
        [AllowExtensionsAttribure(new string[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile? AvatarFile { get; set; }
    }
}
