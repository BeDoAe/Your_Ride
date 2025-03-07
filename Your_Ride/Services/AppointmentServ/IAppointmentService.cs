using Your_Ride.Models.Your_Ride.Models;
using Your_Ride.Services.Generic;
using Your_Ride.ViewModels.AppointmentviewModel;

namespace Your_Ride.Services.AppointmentServ
{
    public interface IAppointmentService :IService<Appointment>
    {
        public Task<List<AppointmentVM>> GetAllAppointments();
        public Task<List<AppointmentVM>> GetAllAppointments(string searchQuery, string sortOrder);

        public Task<AppointmentVM> GetAppointmentByID(int id);
        public Task<AppointmentVM> CreateAppointment(AppointmentVM appointmentVM);
        public Task<AppointmentVM> EditAppointment(AppointmentVM appointmentVM);
        public Task<int> DeleteAppointment(int id);

        //public Task<List<AppointmentVM>> GetAppointmentsByBuisGuideID(string id);
        //public Task<AppointmentVM> CompleteAppointment(int id);



    }
}