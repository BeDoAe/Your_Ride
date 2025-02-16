using Your_Ride.Models;
using Your_Ride.ViewModels.College;
using Your_Ride.ViewModels.University;

namespace Your_Ride.ViewModels.Account
{
    public class AccountCollegeUniversityVM
    {
        public RegisterVM? RegisterVM { get; set; } = new RegisterVM();
        public List<UniversityVM>? UniversitiesVM { get; set; } = new List<UniversityVM>();
        public List<CollegeVM>? CollegesVM { get; set; } = new List<CollegeVM>();
        public List<string>? Batches { get; set; } = new List<string>();
    
    }
}
