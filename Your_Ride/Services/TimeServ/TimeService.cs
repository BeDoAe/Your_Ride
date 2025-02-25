using AutoMapper;
using Your_Ride.Helper;
using Your_Ride.Models;
using Your_Ride.Models.Your_Ride.Models;
using Your_Ride.Repository.AppointmentRepo;
using Your_Ride.Repository.BusRepo;
using Your_Ride.Repository.TimeRepo;
using Your_Ride.Services.Generic;
using Your_Ride.ViewModels.TimeViewModel;

namespace Your_Ride.Services.TimeServ
{
    public class TimeService :Service<Time> , ITimeService
    {
        private readonly IMapper automapper;
        private readonly ITimeRepository timeRepository;
        private readonly IBusRepository busRepository;
        private readonly IAppointmentRepository appointmentRepository;

        public TimeService(IMapper automapper , ITimeRepository timeRepository , IBusRepository busRepository , IAppointmentRepository appointmentRepository)
        {
            this.automapper = automapper;
            this.timeRepository = timeRepository;
            this.busRepository = busRepository;
            this.appointmentRepository = appointmentRepository;
        }

        public async Task<List<TimeVM>> GetAllTimes()
        {
            List<Time> times = await timeRepository.GetAllTimes();

            List<TimeVM> timeVMs = automapper.Map<List<TimeVM>>(times);
            return timeVMs;
        }
        public async Task<List<TimeVM>> GetAllTimesByBusID(int id)
        {
            List<Time> times = await timeRepository.GetAllTimesByBusID(id);
            List<TimeVM> timeVMs = automapper.Map<List<TimeVM>>(times);
            return timeVMs;
        }

        public async Task<List<TimeVM>> GetAllTimesByAppointmentID(int id)
        {
            List<Time> times = await timeRepository.GetAllTimesByBusID(id);
            List<TimeVM> timeVMs = automapper.Map<List<TimeVM>>(times);
            return timeVMs;
        }

        public async Task<TimeVM> GetTimeByID(int id)
        {
            Time time = await timeRepository.GetTimeByID(id);
            if (time == null) return null;
            TimeVM timeVM = automapper.Map<TimeVM>(time);
            return timeVM;

        }
        public async Task<TimeVM> CreateTime(IFormFileTimeVM formFileTimeVM)
        {
            Bus bus = await busRepository.GetByIdAsync(formFileTimeVM.BusID);
            Appointment appointment = await appointmentRepository.GetAppointmentByID(formFileTimeVM.AppointmentId);
            if (bus == null || appointment == null) return null;

            Time Newtime = automapper.Map<Time>(formFileTimeVM);
            //product.Img = await FileHelper.SaveFileAsync(addProductVM.Img);

            //var locationsWithPics = new Dictionary<string, string?>();

            foreach (var T in Newtime.LocationsWithPics)
            {
                foreach (var LC in formFileTimeVM.FormFileLocationsWithPics)
                {
                    T.ImagePath = await FileHelper.SaveFileAsync(LC.ImagePath);
                }
            }

            Time time = await timeRepository.CreateTime(Newtime);
            return automapper.Map<TimeVM>(time);
        }
        public async Task<TimeVM> EditTime(IFormFileTimeVM formFileTimeVM)
        {
            Bus bus = await busRepository.GetByIdAsync(formFileTimeVM.BusID);
            Appointment appointment = await appointmentRepository.GetAppointmentByID(formFileTimeVM.AppointmentId);
            Time timeFromDB = await timeRepository.GetTimeByID(formFileTimeVM.Id);

            if (bus == null || appointment == null || timeFromDB == null) return null;

            automapper.Map(formFileTimeVM, timeFromDB);

            #region Old Mapping
            // Ensure dictionary is initialized
            //if (timeFromDB.LocationsWithPics == null)
            //    timeFromDB.LocationsWithPics = new Dictionary<string, string?>();

            //Dictionary<string, string?> updatedLocations = new();

            //foreach (var LC in formFileTimeVM.LocationsWithPics)
            //{
            //    if (timeFromDB.LocationsWithPics.ContainsKey(LC.Key))
            //    {
            //        // Check if new file is uploaded
            //        if (LC.Value != null)
            //        {
            //            // Delete old file if exists
            //            if (!string.IsNullOrEmpty(timeFromDB.LocationsWithPics[LC.Key]))
            //            {
            //                FileHelper.Delete(timeFromDB.LocationsWithPics[LC.Key]);
            //            }

            //            updatedLocations[LC.Key] = await FileHelper.SaveFileAsync(LC.Value);
            //        }
            //        else
            //        {
            //            // Retain old file if no new file uploaded
            //            updatedLocations[LC.Key] = timeFromDB.LocationsWithPics[LC.Key];
            //        }
            //    }
            //    else
            //    {
            //        // Add new location with file
            //        updatedLocations[LC.Key] = await FileHelper.SaveFileAsync(LC.Value);
            //    }
            //}

            //// Update dictionary
            //timeFromDB.LocationsWithPics = updatedLocations; 
            #endregion

            foreach (var T in timeFromDB.LocationsWithPics)
            {
                foreach (var LC in formFileTimeVM.FormFileLocationsWithPics)
                {
                    
                    T.ImagePath = await FileHelper.SaveFileAsync(LC.ImagePath);
                }
            }

            Time updatedTime = await timeRepository.UpdateAsync(timeFromDB);
            return automapper.Map<TimeVM>(updatedTime);
        }


        public async Task<int> DeleteTime(int id)
        {
            int result = await timeRepository.DeleteTime(id);
            return result;
        }



    }
}
