namespace Your_Ride.Models
{
    public class Transaction : BaseModel
    {
        public int Id { get; set; }

        // The user for whom the transaction is made
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }


        // Foreign key for Wallet (Each Transaction belongs to one Wallet)
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }  // Navigation property

        // The admin who processed the transaction
        public string AdminId { get; set; }
        public ApplicationUser Admin { get; set; }

        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; }

        public DateTime? EditedTransactionDate { get; set; }



    }
}