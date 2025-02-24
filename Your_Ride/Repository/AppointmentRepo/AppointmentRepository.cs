using Microsoft.EntityFrameworkCore;
using Your_Ride.Models;
using Your_Ride.Models.Your_Ride.Models;
using Your_Ride.Repository.Generic;

namespace Your_Ride.Repository.AppointmentRepo
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        private readonly Context context;

        public AppointmentRepository(Context context) : base(context)
        {
            this.context = context;
        }
        public async Task<List<Appointment>> GetAllAppointments()
        {
            List<Appointment> appointments = await context.Appointments.Include(x => x.Times).Include(x => x.BusGuide).ToListAsync();
            return appointments;
        }
        public async Task<List<Appointment>> GetAllAppointments(string searchQuery, string sortOrder)
        {
            IQueryable<Appointment> query = context.Appointments
                .Include(a => a.Times)
                .Include(a => a.BusGuide);

            // Search by date or admin name
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(a =>
                    a.Date.ToString().Contains(searchQuery) ||
                    (a.BusGuide != null && a.BusGuide.UserName.Contains(searchQuery))
                );
            }

            // Sort ASC/DESC (default DESC)
            query = sortOrder == "asc" ? query.OrderBy(a => a.Date) : query.OrderByDescending(a => a.Date);

            return await query.ToListAsync();
        }
        public async Task<Appointment> GetAppointmentByID(int id)
        {
            Appointment appointment = await context.Appointments.Include(x => x.Times).Include(x => x.BusGuide).FirstOrDefaultAsync(x => x.Id == id);
            return appointment;
        }
        public async Task<List<Appointment>> GetAppointmentsByBusGuideID(string id)
        {
           List<Appointment> appointments = await context.Appointments.Include(x => x.Times).Include(x => x.BusGuide).Where(x => x.BusGuideId == id).ToListAsync();
            return appointments;
        }

        public async Task<Appointment> CreateAppointment(Appointment appointment)
        {
            bool appointmentExists = await context.Appointments.AnyAsync(x => x.Date == appointment.Date);

            if (appointmentExists)
            {
                return null;
            }

            await context.Appointments.AddAsync(appointment);
            return appointment;
        }
        public async Task<int> DeleteAppointment(int id)
        {
            Appointment appointment = await context.Appointments.FirstOrDefaultAsync(x => x.Id == id);

            if (appointment == null)
            {
                return -1;

            }
            else if (appointment.IsDeleted == true)
            {
                return 0;
            }
            else
            {
                appointment.IsDeleted = true;
                List<Time> times = await context.Times.Where(x=>x.AppointmentId == id).ToListAsync();
                if (times != null)
                {
                    foreach (Time time in times)
                    {
                        time.IsDeleted = true ;
                    }
                }
                await SaveDB();
                return 1;
            }
        }
        public async Task<Appointment> UpdateAppointment(Appointment appointment)
        {
            context.Appointments.Update(appointment);
            await context.SaveChangesAsync();
            return appointment;

        }
      
    }
}
