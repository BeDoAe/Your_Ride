using Your_Ride.ViewModels.University;
using Your_Ride.Models;


namespace Your_Ride.ViewModels.College
{
    public class UniversityCollegesVM
    {

       public CreateCollege? CreateCollege { get; set; }=new CreateCollege();

        //public College? College { get; set; } = new College();
        public List<UniversityVM>? Universities { get; set; }=new List<UniversityVM>();

    }
}
