using Microsoft.VisualBasic;
using Your_Ride.Models.Your_Ride.Models;

namespace Your_Ride.Models
{
    public class Time : BaseModel
    {
        public int Id { get; set; }
        public TimeOnly TimeOnly { get; set; }


        //public List<string> Locations { get; set; }

        //public List<string>? LocationsPics { get; set; }
        //public Dictionary<string, string?> LocationsWithPics { get; set; } = new();

        public List<LocationImage> LocationsWithPics { get; set; } = new List<LocationImage>();

        public DateTime? DueDateArrivalSubmission { get; set; }
        public DateTime? DueDateDepartureSubmission { get; set; }
  

        public double Fee {  get; set; }




        [Flags]
        public enum TripCategory
        {
            Arrival=0,
            Departure=1
        }

        public TripCategory Category { get; set; }

        public int BusID { get; set; }
        public Bus? Bus { get; set; }


        // Bus Guide assigned to the appointment
        public string BusGuideId { get; set; }
        public ApplicationUser? BusGuide { get; set; }

        // Foreign key for Appointment
        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }


    }
}