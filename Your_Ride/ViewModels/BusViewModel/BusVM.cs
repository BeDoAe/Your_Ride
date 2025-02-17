using System.ComponentModel.DataAnnotations;

namespace Your_Ride.ViewModels.BusViewModel
{
    public class BusVM
    {
        [Required]
        public int Id { get; set; }

        [StringLength(10, MinimumLength = 2, ErrorMessage = "Plate Number must be between 2 and 10 characters long")]
        public string? PlateNumber { get; set; }


        [Required(ErrorMessage ="Must State number of Seats")]
        [Range(1, int.MaxValue)]
        public int NumberOfSeats { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhone { get; set; }
    }
}
