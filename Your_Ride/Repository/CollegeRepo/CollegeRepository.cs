using Microsoft.EntityFrameworkCore;
using Your_Ride.Models;
using Your_Ride.Repository.Generic;

namespace Your_Ride.Repository.CollegeRepo
{
    public class CollegeRepository : Repository<College> , ICollegeRepository 
    {
        private readonly Context context;

        public CollegeRepository(Context context) : base(context)
        {
            this.context = context;
        }
        public async Task<List<College>> GetAllColleges ()
        {
            List<College> colleges = await context.Colleges.Include(x => x.university).ToListAsync();
            return colleges;
        }

        public async  Task<College> GetCollegeByID(int id)
        {
            College college = await context.Colleges.Include(x => x.university).FirstOrDefaultAsync(x => x.Id == id);

            return college;
        }

        public async Task<List<College>> GetCollegesByUniversityId(int universityID)
        {
            List<College> colleges =await context.Colleges.Include(x => x.university).Where(x => x.UniversityID == universityID).ToListAsync();

            return colleges;
        }
        public async Task<int> DeleteCollege(int id)
        {
            College college = await context.Colleges.FirstOrDefaultAsync(x => x.Id == id);
            if (college == null)
            {
                return -1;
            }
            else if (college.IsDeleted == true)
            {
                return 0;
            }
            else
            {
                college.IsDeleted = true;
                await SaveDB();
                return 1;
            }
        }

        //public async Task<College> CreateCollege(College college)
        //{
        //    College collegeFromDB = context.Colleges.FirstOrDefault(c => c.Id == college.Id);
        //    if (college == null)
        //    {
        //        return null;
        //    }
        //    if (collegeFromDB != null)
        //    {
        //        return collegeFromDB;
        //    }
        //    else
        //    {
        //        return college;
        //    }
        //}

    }
}
