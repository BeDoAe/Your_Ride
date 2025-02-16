namespace Your_Ride.Models
{
    public class UserNotification : BaseModel
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }

        public int NotificationID { get; set; }
        public Notification Notification { get; set; }


    }
}