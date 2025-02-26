using Your_Ride.Models.Your_Ride.Models;
using Your_Ride.Models;
using System.ComponentModel.DataAnnotations;

namespace Your_Ride.ViewModels.TimeViewModel
{
    public class IFormFileTimeVM
    {
        public int Id { get; set; }
        public TimeOnly TimeOnly { get; set; }


        //public List<string> Locations { get; set; }

        //public List<string>? LocationsPics { get; set; }
        //public Dictionary<string, IFormFile?> LocationsWithPics { get; set; } = new();

        //public List<LocationImage>? locationImages { get; set; }=new List<LocationImage>();
        public List<FormFileLocationPics> FormFileLocationsWithPics { get; set; }=new List<FormFileLocationPics> { };

        public DateTime? DueDateArrivalSubmission { get; set; }
        public DateTime? DueDateDepartureSubmission { get; set; }


        public double Fee { get; set; }




        [Flags]
        public enum TripCategory
        {
            Arrival = 0,
            Departure = 1
        }

        public TripCategory Category { get; set; }

        [Required(ErrorMessage ="Must Have Bus ")]
        public int BusID { get; set; }
        public Bus? Bus { get; set; }


        // Bus Guide assigned to the appointment
        [Required(ErrorMessage = "Must Have Bus Guide")]
        public string BusGuideId { get; set; }
        public ApplicationUser? BusGuide { get; set; }

        [Required(ErrorMessage = "Must Have An Appointment ")]
        // Foreign key for Appointment
        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }


    }
}
