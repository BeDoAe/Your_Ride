using System.ComponentModel.DataAnnotations;

namespace Your_Ride.ViewModels.Account
{
    public class VerifyOTPVM
    {
        [Required(ErrorMessage = "Must Write UserName")]
        public string UserName { get; set; }

        [Required]
        public string OTP { get; set; }
    }
}
