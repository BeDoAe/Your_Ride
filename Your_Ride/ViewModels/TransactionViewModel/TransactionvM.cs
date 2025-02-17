using System.ComponentModel.DataAnnotations;
using Your_Ride.Models;

namespace Your_Ride.ViewModels.TransactionViewModel
{
    public class TransactionvM : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }

        [Required]
        public int WalletID { get; set; }
        public Wallet? Wallet { get; set; }

        [Required]
        public string AdminId { get; set; }
        public ApplicationUser? Admin { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public double Amount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        public  DateTime? EditedTransactionDate { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public double? OptionAmount { get; set; }

        public TransactionType? OptionType { get; set; } // Only one option: Increase OR Decrease

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ( OptionAmount == null)
            {
                yield return new ValidationResult("You must provide an Amount value when selecting an option type.", new[] { "OptionAmount" });
            }else if (OptionType.HasValue == false )
            {
                yield return new ValidationResult("You must Choose an Option when inserting Amount value.", new[] { "OptionAmount" });

            }

            // Ensure OptionAmount cannot be greater than Amount when Decreasing
            if (OptionType == TransactionType.Decrease && OptionAmount.HasValue && OptionAmount.Value > Amount)
            {
                yield return new ValidationResult("Must not exceed Transaction amount if Decrease", new[] { "OptionAmount" });
            }
        }

    }
    public enum TransactionType
    {
        Increase, // "+"
        Decrease  // "-"
    }

}
