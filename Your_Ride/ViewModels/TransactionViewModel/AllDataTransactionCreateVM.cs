using System.ComponentModel.DataAnnotations;
using Your_Ride.Models;

namespace Your_Ride.ViewModels.TransactionViewModel
{
    public class AllDataTransactionCreateVM
    {
        public string? UserId { get; set; }
        public ApplicationUser User { get; set; }


        public int? WalletId { get; set; }
        public Wallet? Wallet { get; set; }

        public string? AdminId { get; set; }
        public ApplicationUser? Admin { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
    }
}
