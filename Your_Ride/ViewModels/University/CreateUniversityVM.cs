using System.ComponentModel.DataAnnotations;

namespace Your_Ride.ViewModels.University
{
    public class CreateUniversityVM
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

    }
}
