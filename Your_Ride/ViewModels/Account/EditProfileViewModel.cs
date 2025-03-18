using System.ComponentModel.DataAnnotations;
using Your_Ride.Models;

namespace Your_Ride.ViewModels.Account
{
    public class EditProfileViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email .")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression("^(010|011|012|015)\\d{8}$", ErrorMessage = "Invalid  mobile number. Must be 11 digits starting with 010, 011, 012, or 015.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "National ID is required")]
        [RegularExpression("^(2|3)\\d{13}$", ErrorMessage = "Invalid  National ID. Must be 14 digits and start with 2 or 3.")]
        public string NationalID { get; set; }
        public string? Pic_URL { get; set; }

        public IFormFile? ImgFile { get; set; }

        public Wallet? Wallet { get; set; }

        public bool IsLocked { get; set; } = false;

        //[Required(ErrorMessage = "University selection is required")]
        public int? UniversityID { get; set; }
        public Your_Ride.Models.University? university { get; set; }


        [Required(ErrorMessage = "College selection is required")]
        public int CollegeID { get; set; }
        public Your_Ride.Models.College? college { get; set; }

        [Required(ErrorMessage = "Batch selection is required")]
        public string batch { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Gender? gender { get; set; }

        [Flags]
        public enum Gender
        {
            Male = 0,
            Female = 1
        }
        
     


        public string? OTPCode { get; set; }
        public DateTime? OTPExpiry { get; set; }

        public string? FavoriteColor { get; set; }
    }
}
