namespace Your_Ride.Models
{
    public class Bus : BaseModel
    {
        public int Id { get; set; }
        public string? PlateNumber { get; set; }
        public int NumberOfSeats { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhone { get; set; }


    }
}