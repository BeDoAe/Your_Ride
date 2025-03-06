using Microsoft.AspNetCore.Identity;

namespace Your_Ride.Models
{
    public class ApplicationUser : IdentityUser 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Pic_URL { get; set; }
        public string MobileNumber { get; set; } 
        public string NationalID { get; set; }

        public bool IsDeleted { get; set; }=false;

        public bool IsLocked { get; set; }=false ;

        public string? batch { get; set; }
        // One-to-One relationship with Wallet
        public Wallet Wallet { get; set; }

        public int? CollegeID { get; set; }
        public College? college { get; set; }

        // Many-to-Many with Appointments
        public List<Book> Bookings { get; set; } = new List<Book>();

        // Many-to-Many with Notifications
        public List<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();

        // One-to-Many with UserTransactionLog
        public List<UserTransactionLog> userTransactions { get; set; } = new List<UserTransactionLog>();
        
    }
}