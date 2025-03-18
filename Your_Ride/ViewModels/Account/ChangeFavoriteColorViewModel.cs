using System.ComponentModel.DataAnnotations;

namespace Your_Ride.ViewModels.Account
{
    public class ChangeFavoriteColorViewModel
    {
        [Required(ErrorMessage = "Old color is required.")]
        public string OldColor { get; set; }

        [Required(ErrorMessage = "New color is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Color should only contain letters.")]
        public string NewColor { get; set; }
    }
}
