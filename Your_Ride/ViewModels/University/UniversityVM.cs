using System.ComponentModel.DataAnnotations;

namespace Your_Ride.ViewModels.University
{
    public class UniversityVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
       
        public string Name { get; set; }
    }
}