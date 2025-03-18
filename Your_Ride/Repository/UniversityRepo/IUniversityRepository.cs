using Your_Ride.Models;
using Your_Ride.Repository.Generic;

namespace Your_Ride.Repository.UniversityRepo
{
    public interface IUniversityRepository:IRepository<University>
    {

        public Task<int> DeleteUniversity(int id);
        public Task<bool> CheckUniversityExisted(string name);


    }
}