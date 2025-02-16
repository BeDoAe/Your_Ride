namespace Your_Ride.Models
{
    public class Notification : BaseModel
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime DateTime { get; set; }


    }
}