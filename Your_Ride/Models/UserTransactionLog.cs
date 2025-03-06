using System.ComponentModel.DataAnnotations;
using Your_Ride.Models.Your_Ride.Models;

namespace Your_Ride.Models
{
    public class UserTransactionLog :BaseModel
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }

        [Required]
        public int TimeId { get; set; }
        public Time? Time { get; set; }

        [Required]
        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be positive.")]
        public double WithdrawalAmount { get; set; } = 0.0;

        [Required]
        public DateTime TransactionTime { get; set; } = DateTime.Now;
    }
}