namespace SmartWallet.Models
{
    public class DashboardViewModel
    {
        public string FullName { get; set; }
        public decimal Balance { get; set; }
        public string AccountNumber { get; set; }
        public List<Transactions> RecentTransactions { get; set; }

    }
}
