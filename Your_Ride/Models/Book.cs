using Your_Ride.Models.Your_Ride.Models;

namespace Your_Ride.Models
{
    public class Book : BaseModel
    {
        public int Id { get; set; }

        // one -to-Many Relationship between User and Appointment
        public string? UserID { get; set; }
        public ApplicationUser? User { get; set; }

        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; } // One-to-One Relationship


        // Each Booking reserves one Seat
        public int SeatId { get; set; }
        public Seat? Seat { get; set; }
    }
}