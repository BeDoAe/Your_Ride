using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Your_Ride.Models;
using Your_Ride.Models.Your_Ride.Models;
using Your_Ride.Repository.AppointmentRepo;
using Your_Ride.Repository.BookRepo;
using Your_Ride.Services.Generic;
using Your_Ride.ViewModels.AppointmentviewModel;

namespace Your_Ride.Services.AppointmentServ
{
    public class AppointmentService : IService<Appointment>, IAppointmentService
    {
        private readonly IMapper automapper;
        private readonly IAppointmentRepository appointmentRepository;
        private readonly IBookRepository bookRepository;

        public AppointmentService(IMapper automapper, IAppointmentRepository appointmentRepository)
        {
            this.automapper = automapper;
            this.appointmentRepository = appointmentRepository;
            this.bookRepository = bookRepository;
        }
        public async Task<List<AppointmentVM>> GetAllAppointments()
        {
            List<Appointment> appointments = await appointmentRepository.GetAllAppointments();

            List<AppointmentVM> appointmentVMs = automapper.Map<List<AppointmentVM>>(appointments);

            return appointmentVMs;
        }
        public async Task<List<AppointmentVM>> GetAllAppointments(string searchQuery, string sortOrder)
        {
            var appointments = await appointmentRepository.GetAllAppointments(searchQuery, sortOrder);
            return automapper.Map<List<AppointmentVM>>(appointments);
        }
        public async Task<AppointmentVM> GetAppointmentByID(int id)
        {
            Appointment appointment = await appointmentRepository.GetAppointmentByID(id);

            AppointmentVM appointmentVM = automapper.Map<AppointmentVM>(appointment);

            return appointmentVM;


        }
        //public async Task<List<AppointmentVM>> GetAppointmentsByBuisGuideID(string id)
        //{
        //    List<Appointment> appointments = await appointmentRepository.GetAppointmentsByBusGuideID(id);

        //    List<AppointmentVM> appointmentVMs = automapper.Map<List<AppointmentVM>>(appointments);

        //    return appointmentVMs;


        //}



        public async Task<AppointmentVM> CreateAppointment(AppointmentVM appointmentVM)
        {
       
            Appointment appointment = automapper.Map<Appointment>(appointmentVM);
            Appointment appointmentFromDB = await appointmentRepository.CreateAppointment(appointment);
            if (appointmentFromDB == null) return null;

            automapper.Map(appointment, appointmentVM);
            return appointmentVM;


        }

        public async Task<AppointmentVM> EditAppointment(AppointmentVM appointmentVM)
        {
            Appointment appointmentFromDB = await appointmentRepository.GetByIdAsync(appointmentVM.Id);
            if (appointmentFromDB == null) return null;



            //automapper.Map(appointmentVM, appointmentFromDB);

            appointmentFromDB.Date = appointmentVM.Date;
            automapper.Map(appointmentVM, appointmentFromDB);
            await appointmentRepository.UpdateAppointment(appointmentFromDB);
            automapper.Map(appointmentFromDB, appointmentVM);
            return appointmentVM;


        }
        public async Task<int> DeleteAppointment(int id)
        {

            int result = await appointmentRepository.DeleteAppointment(id);


            return result;


        }
        public async Task<AppointmentVM> CompleteAppointment(int id)
        {
            Appointment appointmentFromDB = await appointmentRepository.GetAppointmentByID(id);
            if (appointmentFromDB != null)
            {
                appointmentFromDB.HasCompleted = true;
                await appointmentRepository.SaveDB();
                return automapper.Map<AppointmentVM>(appointmentFromDB);
            }
            return null;
        }

    }
}
