using Microsoft.EntityFrameworkCore;
using Your_Ride.Models;
using Your_Ride.Repository.Generic;

namespace Your_Ride.Repository.UniversityRepo
{
    public class UniversityRepository : Repository<University>, IUniversityRepository
    {
        private readonly Context context;
        public UniversityRepository(Context context) : base(context)
        {
            this.context = context;
        }
        public async Task<int> DeleteUniversity(int id)
        {
            University university = context.Universities.FirstOrDefault(x => x.Id == id);
            if (university == null)
            {
                return -1;
            }
            else if (university.IsDeleted == true)
            {
                return 0;
            }
            else
            {
                university.IsDeleted = true;
               await SaveDB();
                return 1;
            }
        }

        public University AssignUniversity(int  universityId)
        {
            return null;
        }
        public async Task<bool> CheckUniversityExisted(string name)
        {
            University UniversityFromDB = await context.Universities.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower() );
            if (UniversityFromDB == null)
            {
                return false;
            }

            else
            {
                return true;
            }
        }
    }
}
