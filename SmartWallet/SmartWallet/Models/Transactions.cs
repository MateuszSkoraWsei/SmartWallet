namespace SmartWallet.Models
{
    public class Transactions
    {
        public int TransactionID { get; set; }
        public string AccountNumber { get; set; }

        public string TransactionName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public string SenderId { get; set; } = string.Empty;
        public virtual ApplicationUser Sender { get; set; }

        public string ReceiverId { get; set; } = string.Empty;
        public virtual ApplicationUser Receiver { get; set; }

        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }

    }
}
