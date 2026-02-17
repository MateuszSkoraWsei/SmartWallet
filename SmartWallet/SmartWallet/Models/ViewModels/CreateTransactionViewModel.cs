using System.ComponentModel.DataAnnotations;

namespace SmartWallet.Models.ViewModels
{
    public class CreateTransactionViewModel
    {
        [Required]
        [Display(Name = "Numer Konta Odbiorcy")]
        public string AccountNumber { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Kwota musi być większa niż 0.")]
        public decimal Amount { get; set; }
        [Required]
        [Display(Name = "Tytuł Transakcji")]
        public string TransactionName { get; set; }


        [Display(Name = "Opis Transakcji")]
        public string Description { get; set; }
    }
}
