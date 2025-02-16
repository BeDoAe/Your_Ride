namespace Your_Ride.ViewModels.Account
{
    public class UserVM
    {
        public string UserId { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }=new List<string>();
    }
}
