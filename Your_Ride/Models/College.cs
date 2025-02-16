using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Your_Ride.Models
{
    public class College : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "College name is required.")]
        [StringLength(100, ErrorMessage = "College name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select a university.")]
        [ForeignKey("University")]
        public int UniversityID { get; set; }

        public University university { get; set; }

        public List<ApplicationUser> Students { get; set; } = new List<ApplicationUser>();

        public List<string>? Batches { get; set; }


    }
}
