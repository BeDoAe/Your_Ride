namespace Your_Ride.Models
{
    public class Wallet : BaseModel
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        // Foreign key to ApplicationUser
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        // One-to-Many Relationship with Transactions
        public List<Transaction>? Transactions { get; set; } = new List<Transaction>();


    }
}