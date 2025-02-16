using Your_Ride.Models;
using Your_Ride.ViewModels.University;

namespace Your_Ride.ViewModels.College
{
    public class CollegeVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int UniversityID { get; set; }

        public UniversityVM? University { get; set; }



        public List<string>? Batches { get; set; }
    }
}
