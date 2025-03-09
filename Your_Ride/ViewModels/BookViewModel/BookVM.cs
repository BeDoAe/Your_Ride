using Your_Ride.Models.Your_Ride.Models;
using Your_Ride.Models;
using System.ComponentModel.DataAnnotations;

namespace Your_Ride.ViewModels.BookViewModel
{
    public class BookVM
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
