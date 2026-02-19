using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SmartWallet.Models
{
    public enum GenderType
    {
        [Display(Name = "Mężczyzna")] Male,
        [Display(Name = "Kobieta")] Female,
        [Display(Name = "Inna")] Other,
        [Display(Name = "Nie podano")] NotSpecified
    }
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public GenderType Gender { get; set; } = GenderType.NotSpecified;

        public DateTime DateOfBirth { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public string? address { get; set; } 
        public decimal  Balance { get; set; }   
        public string? AccountNumber { get; set; } 

        public virtual ICollection<Transactions> SentTransaction { get; set; } 
        public virtual ICollection<Transactions> ReceivedTransaction { get; set; }

    }
}
