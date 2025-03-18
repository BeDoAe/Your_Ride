using Your_Ride.Models;
using Your_Ride.Repository.Generic;

namespace Your_Ride.Repository.CollegeRepo
{
    public interface ICollegeRepository:IRepository<College>
    {
        public Task<int> DeleteCollege(int id);

        public Task<List<College>> GetAllColleges();

        public Task<College> GetCollegeByID(int id);

        public Task<List<College>> GetCollegesByUniversityId(int universityID);

        public Task<bool> CheckCollegeExisted(string name, int UniversityID);



    }
}