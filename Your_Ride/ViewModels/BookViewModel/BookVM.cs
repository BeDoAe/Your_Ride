using Your_Ride.Models.Your_Ride.Models;
using Your_Ride.Models;
using System.ComponentModel.DataAnnotations;

namespace Your_Ride.ViewModels.BookViewModel
{
    public class BookVM
    {
        public int Id { get; set; }

        // one -to-Many Relationship between User and Appointment
        public string UserID { get; set; }
        public ApplicationUser? User { get; set; }

        public Appointment? Appointment { get; set; } // One-to-One Relationship

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public double MaxAmount { get; set; }
    }
}
