using Microsoft.EntityFrameworkCore;
using Your_Ride.Models;
using Your_Ride.Models.Your_Ride.Models;
using Your_Ride.Repository.Generic;

namespace Your_Ride.Repository.TimeRepo
{
    public class TimeRepository : Repository<Time> , ITimeRepository
    {
        private readonly Context context;
        public TimeRepository ( Context context ):base( context )
        {
            this.context = context;
        }
        public async Task<List<Time>> GetAllTimes()
        {
            List<Time> times = await context.Times.Include(x=>x.LocationsWithPics).Include(x=>x.Bus).Include(x=>x.Appointment).ThenInclude(x=>x.BusGuide).ToListAsync();
            return times;
        }

        public async Task<Time> GetTimeByID(int id)
        {
            Time time = await context.Times.Include(x => x.LocationsWithPics).Include(x => x.Bus).Include(x => x.Appointment).ThenInclude(x => x.BusGuide).FirstOrDefaultAsync(x=>x.Id==id);
            return time;
        }
        public async Task<List<Time>> GetAllTimesByBusID(int id)
        {
            List<Time> times = await context.Times.Include(x => x.LocationsWithPics).Include(x=>x.Bus).Include(x=>x.Appointment).ThenInclude(x => x.BusGuide).Where(x=>x.BusID==id).ToListAsync();
            return times;
        }
        public async Task<List<Time>> GetAllTimesByAppointmentID(int id)
        {
            List<Time> times = await context.Times.Include(x => x.LocationsWithPics).Include(x => x.Bus).Include(x => x.Appointment).ThenInclude(x => x.BusGuide).Where(x => x.AppointmentId == id).ToListAsync();
            return times;
        }

        public async Task<Time> CreateTime(Time time)
        {
            // Check if `time.Appointment` is null to prevent NullReferenceException
            if (time.Appointment == null)
            {
                throw new ArgumentNullException(nameof(time.Appointment), "Time must have an associated Appointment.");
            }

            // Check if there is an appointment on the same date
            Appointment? existingAppointment = await context.Appointments
                .Include(x => x.Times)
                .ThenInclude(x => x.LocationsWithPics)
                .FirstOrDefaultAsync(x => x.Date == time.Appointment.Date);

            // If an appointment exists, check if the time already exists
            if (existingAppointment != null)
            {
                bool timeExists = existingAppointment.Times.Any(t => t.TimeOnly == time.TimeOnly);

                // Fix: Corrected condition
                if (timeExists)  // No need to check `!= null`, it's always true/false
                {
                    return null;
                }
            }

            await context.Times.AddAsync(time);
            await SaveDB();
            return time;
        }

        public async Task<int> DeleteTime(int id)
        {
            Time time = await context.Times.FirstOrDefaultAsync(x => x.Id == id);

            if (time == null)
            {
                return -1;

            }
            else if (time.IsDeleted == true)
            {
                return 0;
            }
            else
            {
                time.IsDeleted = true;
                await SaveDB();
                return 1;
            }
        }
        public async Task<int> DeleteLocationImage(int id)
        {
            LocationImage locationImage = await context.LocationImages.FirstOrDefaultAsync(x => x.Id == id);

            if (locationImage == null)
            {
                return -1;

            }
            else if (locationImage.IsDeleted == true)
            {
                return 0;
            }
            else
            {
                locationImage.IsDeleted = true;
                await SaveDB();
                return 1;
            }
        }
        public async Task<LocationImage> GetLocationImage(int id)
        {
            LocationImage locationImage = await context.LocationImages.FirstOrDefaultAsync(x => x.Id == id);
            if (locationImage == null) return null;

            return locationImage;

        }



    }
}
