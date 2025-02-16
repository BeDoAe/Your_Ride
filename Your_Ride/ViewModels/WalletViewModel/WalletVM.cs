using System.ComponentModel.DataAnnotations;
using Your_Ride.Models;

namespace Your_Ride.ViewModels.WalletViewModel
{
    public class WalletVM
    {
        public int Id { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        public double Amount { get; set; }
        public ApplicationUser User { get; set; }

    }
}
