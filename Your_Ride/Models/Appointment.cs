namespace Your_Ride.Models
{
    namespace Your_Ride.Models
    {
        public class Appointment : BaseModel 
        {
            public int Id { get; set; }
            public DateOnly Date { get; set; }
            public List<Time> Times { get; set; } = new List<Time>(); // One-to-Many Relationship

            public int BookId { get; set; }
            public Book Book { get; set; }


        }
    }
}