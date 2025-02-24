using Your_Ride.Models;
using Your_Ride.Models.Your_Ride.Models;
using Your_Ride.Repository.Generic;

namespace Your_Ride.Repository.AppointmentRepo
{
    public interface IAppointmentRepository : IRepository<Appointment>  
    {
        public Task<List<Appointment>> GetAllAppointments();
        public Task<Appointment> GetAppointmentByID(int id);

        public Task<Appointment> CreateAppointment(Appointment appointment);

        public Task<int> DeleteAppointment(int id);
        public Task<Appointment> UpdateAppointment(Appointment appointment);
        public Task<List<Appointment>> GetAppointmentsByBusGuideID(string id);
        public Task<List<Appointment>> GetAllAppointments(string searchQuery, string sortOrder);

    }
}