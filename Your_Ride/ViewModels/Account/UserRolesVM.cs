using Your_Ride.Helper;

namespace Your_Ride.ViewModels.Account
{
    public class UserRolesVM
    {
        public List<UserVM> Users { get; set; }
        public List<string> Roles { get; set; }
        public PaginatedList<UserVM> Pagination { get; set; } 
    }
}
