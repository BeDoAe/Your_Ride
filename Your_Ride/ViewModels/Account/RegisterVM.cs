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
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email .")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
             ErrorMessage = "Password must be at least 8 characters long and contain an uppercase letter, lowercase letter, number, and special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression("^(010|011|012|015)\\d{8}$", ErrorMessage = "Invalid  mobile number. Must be 11 digits starting with 010, 011, 012, or 015.")]
        public string PhoneNumber { get; set; } 

        [Required(ErrorMessage = "National ID is required")]
        [RegularExpression("^(2|3)\\d{13}$", ErrorMessage = "Invalid  National ID. Must be 14 digits and start with 2 or 3.")]
        public string NationalID { get; set; }


        [Required(ErrorMessage = "University selection is required")]
        public int UniversityID { get; set; }

        [Required(ErrorMessage = "College selection is required")]
        public int CollegeID { get; set; }

        [Required(ErrorMessage = "Batch selection is required")]
        public string Batch { get; set; }

        [Required(ErrorMessage = "Favorite Color is required")] 
        public string FavoriteColor { get; set; }
    }
}
