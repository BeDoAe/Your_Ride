namespace Your_Ride.Models
{
    public class LocationImage
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string? ImagePath { get; set; }

        public string? PathURL { get; set; }

        public int LocationOrder { get; set; }

        // Foreign Key to Time
        public int TimeId { get; set; }
        public Time? Time { get; set; }
    }
}
