using System.ComponentModel.DataAnnotations;

namespace Your_Ride.ViewModels.Account
{
    public class ForgotPasswordVM
    {
        [Required (ErrorMessage ="Must Write UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Favorite Color is required")]
        public string FavoriteColor { get; set; }
        //public string MaskedEmail { get; set; }
        //public string MaskedPhone { get; set; }
        //public string OTP { get; set; }
        //public string SelectedMethod { get; set; } // "Email" or "Phone"
    }
}
