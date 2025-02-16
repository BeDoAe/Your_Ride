using Your_Ride.Models;
using Your_Ride.Repository.UniversityRepo;
using Your_Ride.Services.Generic;
using AutoMapper;
using Your_Ride.ViewModels.University;
using System.Collections.Generic;

namespace Your_Ride.Services.UniversityServ
{
    public class UniversityService : Service<University>, IUniversityService
    {
        private readonly IUniversityRepository universityRepository;
        private readonly IMapper autoMapper;

        public UniversityService(IUniversityRepository universityRepository, IMapper autoMapper)
        {
            this.universityRepository = universityRepository;
            this.autoMapper = autoMapper;
        }

        public async Task<List<UniversityVM>> GetAllUniversity()
        {
            List<University> universities = await universityRepository.GetAllAsync();
            List<UniversityVM> universitiesVM = autoMapper.Map<List<UniversityVM>>(universities);
            return universitiesVM;
        }

        // Get a single university by ID
        public async Task<UniversityVM> GetUniversityById(int id)
        {
            var university = await universityRepository.GetByIdAsync(id);
            return autoMapper.Map<UniversityVM>(university);
        }

        // Create a new university
        public async Task<UniversityVM> CreateUniversity(CreateUniversityVM universityVM)
        {
            var university = autoMapper.Map<University>(universityVM);
            await universityRepository.AddAsync(university);
            return autoMapper.Map<UniversityVM>(university);
        }

        // Update an existing university
        public async Task<UniversityVM> UpdateUniversity(int id, UniversityVM universityVM)
        {
            var universityFromDB = await universityRepository.GetByIdAsync(id);
            if (universityFromDB == null) return null;

            //University university = autoMapper.Map<University>(universityVM);
            //university.Name = universityVM.Name;

            // Use AutoMapper to update existing entity instead of creating a new one
            autoMapper.Map(universityVM, universityFromDB);

            await universityRepository.UpdateAsync(universityFromDB);

            return autoMapper.Map<UniversityVM>(universityFromDB);
        }

        //Delete 
        public async Task<int> DeleteUniversity(int id)
        {
            int result = await universityRepository.DeleteUniversity(id);
            return result;
        }


    }
}
