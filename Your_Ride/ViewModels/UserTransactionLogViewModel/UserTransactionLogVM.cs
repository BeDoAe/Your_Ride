using System.ComponentModel.DataAnnotations;
using Your_Ride.Models.Your_Ride.Models;
using Your_Ride.Models;

namespace Your_Ride.ViewModels.UserTransactionLogViewModel
{
    public class UserTransactionLogVM
    {
        public int Id { get; set; }

        [Required (ErrorMessage ="User Is Required !")]
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }

        [Required(ErrorMessage = "Please Select Time !")]
        public int TimeId { get; set; }
        public Time? Time { get; set; }

        [Required(ErrorMessage = "Please Select Date !")]
        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be positive.")]
        public double WithdrawalAmount { get; set; } = 0.0;

        public DateTime TransactionTime { get; set; } = DateTime.Now;
    }
}