using Your_Ride.Models;
using Your_Ride.Services.Generic;
using Your_Ride.ViewModels.College;

namespace Your_Ride.Services.CollegeServ
{
    public interface ICollegeService : IService<College>
    {
        public  Task<List<CollegeVM>> GetAllCollege();

        public  Task<CollegeVM> GetCollegeById(int id);

        public Task<List<CollegeVM>> GetCollegesByUniversityId(int universityId);


        public Task<CollegeVM> CreateUniversity(CreateCollege createCollege);

        public  Task<CollegeVM> UpdateCollege(int id, CollegeVM collegeVM);

        public  Task<int> DeleteCollege(int id);


    }
}