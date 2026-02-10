using Microsoft.AspNetCore.Identity;

namespace SmartWallet.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public decimal  Balance { get; set; }   
        public string AccountNumber { get; set; } 

        public virtual ICollection<Transactions> SentTransaction { get; set; } 
        public virtual ICollection<Transactions> ReceivedTransaction { get; set; }

    }
}
