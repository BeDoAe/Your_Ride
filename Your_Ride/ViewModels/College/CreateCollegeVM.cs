using System.ComponentModel.DataAnnotations;
using Your_Ride.Models;

namespace Your_Ride.ViewModels.College
{
    public class CreateCollege
    {
        [Required(ErrorMessage = "College name is required.")]
        [StringLength(100, ErrorMessage = "College name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select a university.")]
        public int UniversityID { get; set; }
        public List<string>? Batches { get; set; }

    }
}
