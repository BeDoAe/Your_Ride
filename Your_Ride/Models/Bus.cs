using System.ComponentModel.DataAnnotations;

namespace Your_Ride.Models
{
    public class Bus : BaseModel
    {
        public int Id { get; set; }
        public string? PlateNumber { get; set; }
        public int NumberOfSeats { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhone { get; set; }

        //public bool IsAvailable { get; set; } = true;

        [Required]
        public string BusIdentifier { get; set; } 

        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}