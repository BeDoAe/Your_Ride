namespace Your_Ride.Models
{
    public class University : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<College> Colleges { get; set; } = new List<College>();


    }
}