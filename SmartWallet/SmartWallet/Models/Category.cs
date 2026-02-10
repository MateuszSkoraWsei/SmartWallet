using System.ComponentModel.DataAnnotations;

namespace SmartWallet.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryColor { get; set; } = string.Empty;
        public string CategoryIcon { get; set; } = string.Empty;
    }
}
