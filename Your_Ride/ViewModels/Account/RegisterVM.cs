using System.ComponentModel.DataAnnotations;

namespace Your_Ride.ViewModels.Account
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "National ID is required")]
        public string NationalID { get; set; }

        [Required(ErrorMessage = "University selection is required")]
        public int UniversityID { get; set; }

        [Required(ErrorMessage = "College selection is required")]
        public int CollegeID { get; set; }

        [Required(ErrorMessage = "Batch selection is required")]
        public string Batch { get; set; }
    }
}
