using Microsoft.EntityFrameworkCore;
using Your_Ride.Models;
using Your_Ride.Models.Your_Ride.Models;
using Your_Ride.Repository.BusRepo;
using Your_Ride.Repository.Generic;
using Your_Ride.ViewModels.TimeViewModel;

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
            List<Time> times = await context.Times.Include(x=>x.LocationsWithPics).Include(x=>x.BusGuide).Include(x=>x.Bus).Include(x=>x.Appointment).ToListAsync();
            return times;
        }
        public async Task<List<Time>> GetAllAvailableTimes()
        {
            List<Time> times = await context.Times.Include(x => x.LocationsWithPics).Include(x => x.BusGuide)
                .Include(x => x.Bus)
                .Include(x => x.Appointment)
                .Where(x => x.HasCompleted != true && 
                     (x.DueDateArrivalSubmission == null || x.DueDateArrivalSubmission <= DateTime.Now ||
                      x.DueDateDepartureSubmission == null || x.DueDateDepartureSubmission <= DateTime.Now))
        .       ToListAsync();            
            return times;
        }
        public async Task<Time> GetTimeByID(int id)
        {
            Time time = await context.Times.Include(x => x.LocationsWithPics).Include(x => x.BusGuide).Include(x => x.Bus).Include(x => x.Appointment).FirstOrDefaultAsync(x=>x.Id==id);
            return time;
        }
        public async Task<List<Time>> GetAllTimesByBusID(int id)
        {
            List<Time> times = await context.Times.Include(x => x.LocationsWithPics).Include(x => x.BusGuide).Include(x=>x.Bus).Include(x=>x.Appointment).Where(x=>x.BusID==id).ToListAsync();
            return times;
        }
        public async Task<List<Time>> GetAllTimesByAppointmentID(int id)
        {
            List<Time> times = await context.Times.Where(x => x.AppointmentId == id).ToListAsync();
            return times;
        }
        public async Task<List<Time>> GetAppointmentsByBusGuideID(string id)
        {
            List<Time> times = await context.Times.Include(x => x.LocationsWithPics).Include(x => x.BusGuide).Include(x => x.Bus).Include(x => x.Appointment).Where(x => x.BusGuideId == id).ToListAsync();
            return times;
        }
        public async Task<List<LocationImage>> GetLocationImagessByTimeID(int id)
        {
            List<LocationImage> locationImages = await context.LocationImages.Where(x => x.TimeId == id).ToListAsync();
            return locationImages;
        }

        public async Task<Time> CreateTime(Time time)
        {
            // Check if `time.Appointment` is null to prevent NullReferenceException
            if (time.Appointment == null)
            {
                return null;
            }

            // Check if there is an appointment on the same date
            Appointment? existingAppointment = await context.Appointments
                .Include(x => x.Times)
                .ThenInclude(x => x.LocationsWithPics)
                .FirstOrDefaultAsync(x => x.Date == time.Appointment.Date && x.IsDeleted == false);

            // If an appointment exists, check if the time already exists with same category !
            if (existingAppointment != null)
            {
                bool timeExists = existingAppointment.Times.Any(t => t.BusID == time.BusID && t.IsDeleted ==false);

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
        public async Task<Time> UpdateTime(Time time)
        {
            Time timeFromDB = await context.Times.FirstOrDefaultAsync(x => x.Id == time.Id);
            if (timeFromDB == null) return null;
            else
            {
                // Check if an appointment exists on the same date
                Appointment? existingAppointment = await context.Appointments
                    .Include(x => x.Times)
                    .FirstOrDefaultAsync(x => x.Date == time.Appointment.Date  && x.IsDeleted == false);

                if (existingAppointment != null)
                {
                    // Check if another time exists with the same BusID in the appointment
                    bool timeExists = existingAppointment.Times
                          .Any(t => t.BusID == time.BusID && t.IsDeleted == false && t.Id != time.Id);

                    if (timeExists)
                    {
                        return null; // Another Time with the same BusID exists on this appointment date
                    }
                }

                await context.SaveChangesAsync();

                //context.Times.Update(time);
                //await context.SaveChangesAsync();
                    return time;
            }


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
                List<Seat> seats = await context.Seats.Where(b => b.BusId == time.BusID).ToListAsync();

                    foreach (Seat s in seats)
                    {
                        s.IsAvailable = true;

                    }
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
        public async Task<bool> AddLocationImages(int id, List<LocationImage> locationImages)
        {
            Time timeFromDB = await context.Times
                .Include(t => t.LocationsWithPics)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (timeFromDB == null) return false;

            foreach (var locationImage in locationImages)
            {
                bool exists = timeFromDB.LocationsWithPics.Any(x => x.Location == locationImage.Location);
                if (!exists)
                {
                    locationImage.TimeId = id;
                    await context.LocationImages.AddAsync(locationImage);
                }
                else
                {
                    return false;
                }
            }

            await context.SaveChangesAsync();
            return true;
        }


        

        public async Task<LocationImage> GetLocationImage(int id)
        {
            LocationImage locationImage = await context.LocationImages.FirstOrDefaultAsync(x => x.Id == id);
            if (locationImage == null) return null;

            return locationImage;

        }

        public async Task<List<int>> GetTimeLocationOrder(int timeId)
        {
            return await context.LocationImages
                .Where(l => l.TimeId == timeId)
                .OrderBy(l => l.LocationOrder)
                .Select(l => l.LocationOrder)
                .ToListAsync();
        }



    }
}
