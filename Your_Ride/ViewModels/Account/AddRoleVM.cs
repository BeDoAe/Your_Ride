using System.ComponentModel.DataAnnotations;

namespace Your_Ride.ViewModels.Account
{
    public class AddRoleVM
    {
        [Required(ErrorMessage = "Role Name is Required")]
        public string RoleName { get; set; }

    }
}
