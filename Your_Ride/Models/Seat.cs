namespace Your_Ride.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public int BusId { get; set; }
        public string SeatLabel { get; set; } 

        public bool IsDeleted {  get; set; }=false;
    }
}
