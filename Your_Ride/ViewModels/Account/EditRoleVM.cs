using System.ComponentModel.DataAnnotations;

namespace Your_Ride.ViewModels.Account
{
    public class EditRoleVM
    {
        public string RoleName { get; set; }
        [Required(ErrorMessage = "Role Name is Required")]
        public string NewRoleName { get; set; }
    }
}
