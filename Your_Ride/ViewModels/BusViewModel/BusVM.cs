using System.ComponentModel.DataAnnotations;
using Your_Ride.Models;

namespace Your_Ride.ViewModels.BusViewModel
{
    public class BusVM
    {
        [Required]
        public int Id { get; set; }

        [StringLength(10, MinimumLength = 2, ErrorMessage = "Plate Number must be between 2 and 10 characters long")]
        public string? PlateNumber { get; set; }


        [Required(ErrorMessage ="Must State number of Seats")]
        [Range(1, int.MaxValue, ErrorMessage = "Must be More than 0")]
        //[PositiveNumber(ErrorMessage = "Number of seats must be a positive integer.")]
        public int NumberOfSeats { get; set; }

        public string? BusIdentifier { get; set; }
        public ICollection<Seat>? Seats { get; set; }

        public string? DriverName { get; set; }
        public string? DriverPhone { get; set; }
        //public bool IsAvailable { get; set; } = true;

    }
    //public class PositiveNumberAttribute : ValidationAttribute
    //{
    //    public override bool IsValid(object value)
    //    {
    //        if (value == null)
    //            return false;

    //        int number = (int)value;
    //        return number > 0;
    //    }

    //    public override string FormatErrorMessage(string name)
    //    {
    //        return $"{name} must be a positive integer.";
    //    }
    //}

}
