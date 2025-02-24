using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Your_Ride.Models.Your_Ride.Models;

namespace Your_Ride.Models
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<College> Colleges { get; set; }

        public DbSet<University> Universities {  get; set; }
        
        public DbSet<Book> Books { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<Time> Times { get; set; }  

        public DbSet<Bus> Buses { get; set; }

        public DbSet<Seat> Seats { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<UserNotification> userNotifications { get; set; }  


        public Context(DbContextOptions<Context> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One-to-Many: Appointment has multiple Time slots
            modelBuilder.Entity<Appointment>()
                .HasMany(a => a.Times)
                .WithOne(t => t.Appointment)
                .HasForeignKey(t => t.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade); // Delete times if appointment is deleted

            // One-to-Many: A User can have multiple Bookings
            modelBuilder.Entity<Book>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-One: Each Booking is linked to one Appointment
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Appointment)
                .WithMany() // Appointments exist independently
                .HasForeignKey(b => b.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-One: Each Booking reserves one Seat
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Seat)
                .WithMany()
                .HasForeignKey(b => b.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many: Bus Guide is a User
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.BusGuide)
                .WithMany()
                .HasForeignKey(a => a.BusGuideId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure One-to-One relationship between ApplicationUser and Wallet
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Wallet)
                .WithOne(w => w.User)
                .HasForeignKey<Wallet>(w => w.UserId)  // Wallet depends on ApplicationUser
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Transaction>()
       .HasOne(t => t.Wallet)  // Each Transaction has one Wallet
       .WithMany(w => w.Transactions)  // One Wallet has many Transactions
       .HasForeignKey(t => t.WalletId)  // Foreign Key
       .OnDelete(DeleteBehavior.Restrict); // Optional: Restrict delete

            // Transaction to ApplicationUser (Admin)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Admin)
                .WithMany()
                .HasForeignKey(t => t.AdminId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete

            // Wallet to ApplicationUser (User)
            modelBuilder.Entity<Wallet>()
                .HasOne(w => w.User)
                .WithOne(u => u.Wallet)
                .HasForeignKey<Wallet>(w => w.UserId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete


            modelBuilder.Entity<ApplicationUser>()
                .HasQueryFilter(x => x.IsLocked != true)
                .HasQueryFilter(x => x.IsDeleted != true);  

            base.OnModelCreating(modelBuilder);

            // Apply global query filter for entities that inherit BaseModel
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseModel).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(Context)
                        .GetMethod(nameof(SetGlobalQueryFilter), BindingFlags.NonPublic | BindingFlags.Static)
                        ?.MakeGenericMethod(entityType.ClrType);
                    method?.Invoke(null, new object[] { modelBuilder });
                }
            }
        }

        private static void SetGlobalQueryFilter<T>(ModelBuilder modelBuilder) where T : BaseModel
        {
            modelBuilder.Entity<T>().HasQueryFilter(m => !m.IsDeleted);
        }
    }


}

