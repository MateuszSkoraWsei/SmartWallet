using System.ComponentModel.DataAnnotations;

namespace SmartWallet.Views
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email jest wymagany")]
        [EmailAddress(ErrorMessage ="to nie jest poprawny adress email")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Hasło jest wymagane")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
         [Display(Name = "Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
    }
}
