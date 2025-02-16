using Your_Ride.ViewModels.University;

namespace Your_Ride.Services.UniversityServ
{
    public interface IUniversityService
    {
        public Task<List<UniversityVM>> GetAllUniversity();
        public Task<UniversityVM> GetUniversityById(int id);

        public Task<UniversityVM> CreateUniversity(CreateUniversityVM universityVM);

        public  Task<UniversityVM> UpdateUniversity(int id, UniversityVM universityVM);

        public Task<int> DeleteUniversity(int id);


    }
}