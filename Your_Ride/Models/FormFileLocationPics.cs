
using System.ComponentModel.DataAnnotations;

namespace Your_Ride.Models
{
    public class FormFileLocationPics
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public IFormFile? ImagePath { get; set; }

        public string? PathURL { get; set; }

        [Required(ErrorMessage = "Must Specify the Location Order")]
        public int LocationOrder { get; set; }

        // Foreign Key to Time
        public int TimeId { get; set; }
        public Time? Time { get; set; }

    }
}
