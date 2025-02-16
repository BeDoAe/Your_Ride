using System.ComponentModel.DataAnnotations;
using Your_Ride.Models;

namespace Your_Ride.ViewModels.TransactionViewModel
{
    public class TransactionvM
    {
        public int Id { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        public Wallet Wallet { get; set; }

        [Required]
        public ApplicationUser Admin { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public double Amount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        public  DateTime? EditedTransactionDate { get; set; }
    }
}
