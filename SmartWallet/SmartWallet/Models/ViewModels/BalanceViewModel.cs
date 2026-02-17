namespace SmartWallet.Models.ViewModels
{
    public class BalanceViewModel
    {
        public decimal TotalBalance { get; set; }
        
        public decimal MonthlyIncomes { get; set; }

        public decimal MonthlyExpenses { get; set; }

        public string AccountNumber { get; set; }
    }
}
