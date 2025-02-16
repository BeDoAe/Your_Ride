using AutoMapper;
using Your_Ride.Models;
using Your_Ride.Repository.CollegeRepo;
using Your_Ride.Repository.UniversityRepo;
using Your_Ride.Services.Generic;
using Your_Ride.ViewModels.College;
using Your_Ride.ViewModels.University;

namespace Your_Ride.Services.CollegeServ
{
    public class CollegeService : Service<College> , ICollegeService 
    {
        private readonly ICollegeRepository collegeRepository;
        private readonly IMapper autoMapper;

        public CollegeService(ICollegeRepository collegeRepository, IMapper autoMapper)
        {
            this.collegeRepository = collegeRepository;
            this.autoMapper = autoMapper;
        }



        public async Task<List<CollegeVM>> GetAllCollege()
        {
            List<College> colleges = await collegeRepository.GetAllColleges();
            List<CollegeVM> collegeVMs = autoMapper.Map<List<CollegeVM>>(colleges);
            return collegeVMs;
        }

        // Get a single College by ID
        public async Task<CollegeVM> GetCollegeById(int id)
        {
            var college = await collegeRepository.GetCollegeByID(id);
            return autoMapper.Map<CollegeVM>(college);
        }

        // Get college by University 
        public async Task<List<CollegeVM>> GetCollegesByUniversityId (int universityId)
        {
            List<College> colleges = await collegeRepository.GetCollegesByUniversityId(universityId);
            List<CollegeVM> collegeVMs = autoMapper.Map<List<CollegeVM>>(colleges);
            return collegeVMs;
        }

        // Create a new College
        public async Task<CollegeVM> CreateUniversity(CreateCollege createCollege)
        {
            var college = autoMapper.Map<College>(createCollege);
            await collegeRepository.AddAsync(college);
            return autoMapper.Map<CollegeVM>(college);
        }

        // Update an existing College
        public async Task<CollegeVM> UpdateCollege(int id, CollegeVM collegeVM)
        {
            var CollegeFromDB = await collegeRepository.GetByIdAsync(id);
            if (CollegeFromDB == null) return null;

            //College college = autoMapper.Map<College>(collegeVM);
            //college.Name = collegeVM.Name;

            // Use AutoMapper to update existing entity instead of creating a new one
            autoMapper.Map(collegeVM, CollegeFromDB);

            await collegeRepository.UpdateAsync(CollegeFromDB);

            return autoMapper.Map<CollegeVM>(CollegeFromDB);
        }

        //Delete College
        public async Task<int> DeleteCollege(int id)
        {
            int result = await collegeRepository.DeleteCollege(id);
            return result;
        }


    }
}
