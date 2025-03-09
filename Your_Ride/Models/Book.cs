using Your_Ride.Models.Your_Ride.Models;

namespace Your_Ride.Models
{
    public class Book : BaseModel
    {
        public int Id { get; set; }

        public string UserID { get; set; }
        public ApplicationUser? User { get; set; }

        public int timeId { get; set; }
        public Time? Time { get; set; } 


        public int SeatId { get; set; }
        public Seat? Seat { get; set; }

        public DateTime BookeDateTime { get; set; } = DateTime.Now;
    }
}