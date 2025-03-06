namespace Your_Ride.Models
{
    namespace Your_Ride.Models
    {
        public class Appointment : BaseModel 
        {
            public int Id { get; set; }
            public DateOnly Date { get; set; }
            public List<Time> Times { get; set; } = new List<Time>(); // One-to-Many Relationship

            public double MaxAmount { get; set; }


      

            // Mark as completed when the appointment finishes
            public bool HasCompleted { get; set; } = false;


            // One-to-Many with UserTransactionLog
            public List<UserTransactionLog> userTransactions { get; set; } = new List<UserTransactionLog>();
        }
    }
}