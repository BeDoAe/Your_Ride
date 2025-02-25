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
            Newtime.Appointment = appointment;
            Newtime.Bus = bus;
            //product.Img = await FileHelper.SaveFileAsync(addProductVM.Img);

            //var locationsWithPics = new Dictionary<string, string?>();

            foreach (var LC in formFileTimeVM.FormFileLocationsWithPics) 
            {
                //foreach (var T in Newtime.LocationsWithPics)
                //{
                    LocationImage locationImage = new LocationImage();
                    locationImage.TimeId = LC.TimeId;
                    locationImage.PathURL= LC.PathURL;
                    locationImage.LocationOrder = LC.LocationOrder;
                    locationImage.Location=LC.Location;
                    locationImage.TimeId = LC.TimeId;
                    locationImage.ImagePath = await FileHelper.SaveFileAsync(LC.ImagePath);

                Newtime.LocationsWithPics.Add(locationImage);

                //}
            }

            Time time = await timeRepository.CreateTime(Newtime);
            return automapper.Map<TimeVM>(time);
        }
        #region Old Edit

        //public async Task<TimeVM> EditTime(IFormFileTimeVM formFileTimeVM)
        //{
        //    Bus bus = await busRepository.GetByIdAsync(formFileTimeVM.BusID);
        //    Appointment appointment = await appointmentRepository.GetAppointmentByID(formFileTimeVM.AppointmentId);
        //    Time timeFromDB = await timeRepository.GetTimeByID(formFileTimeVM.Id);

        //    if (bus == null || appointment == null || timeFromDB == null) return null;

        //    automapper.Map(formFileTimeVM, timeFromDB);

        //    #region Old Mapping
        //    // Ensure dictionary is initialized
        //    //if (timeFromDB.LocationsWithPics == null)
        //    //    timeFromDB.LocationsWithPics = new Dictionary<string, string?>();

        //    //Dictionary<string, string?> updatedLocations = new();

        //    //foreach (var LC in formFileTimeVM.LocationsWithPics)
        //    //{
        //    //    if (timeFromDB.LocationsWithPics.ContainsKey(LC.Key))
        //    //    {
        //    //        // Check if new file is uploaded
        //    //        if (LC.Value != null)
        //    //        {
        //    //            // Delete old file if exists
        //    //            if (!string.IsNullOrEmpty(timeFromDB.LocationsWithPics[LC.Key]))
        //    //            {
        //    //                FileHelper.Delete(timeFromDB.LocationsWithPics[LC.Key]);
        //    //            }

        //    //            updatedLocations[LC.Key] = await FileHelper.SaveFileAsync(LC.Value);
        //    //        }
        //    //        else
        //    //        {
        //    //            // Retain old file if no new file uploaded
        //    //            updatedLocations[LC.Key] = timeFromDB.LocationsWithPics[LC.Key];
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        // Add new location with file
        //    //        updatedLocations[LC.Key] = await FileHelper.SaveFileAsync(LC.Value);
        //    //    }
        //    //}

        //    //// Update dictionary
        //    //timeFromDB.LocationsWithPics = updatedLocations; 
        //    #endregion

        //    foreach (var T in timeFromDB.LocationsWithPics)
        //    {
        //        foreach (var LC in formFileTimeVM.FormFileLocationsWithPics)
        //        {

        //            T.ImagePath = await FileHelper.SaveFileAsync(LC.ImagePath);
        //        }
        //    }

        //    Time updatedTime = await timeRepository.UpdateAsync(timeFromDB);
        //    return automapper.Map<TimeVM>(updatedTime);
        //}
        #endregion

        public async Task<TimeVM> EditTime(IFormFileTimeVM formFileTimeVM)
        {
            // Retrieve the existing Time object from the database using its ID.
            Time timeFromDB = await timeRepository.GetTimeByID(formFileTimeVM.Id);

            // Loop through the existing locations linked to this Time entry
            foreach (var existingLocation in timeFromDB.LocationsWithPics.ToList())
            {
                // Check if the current location exists in the updated request data
                var updatedLocation = formFileTimeVM.FormFileLocationsWithPics
                    .FirstOrDefault(l => l.Id == existingLocation.Id);

                if (updatedLocation != null) // If the location exists in the update request
                {
                    // Update location name, order, and path URL from the request data
                    existingLocation.TimeId = updatedLocation.TimeId;
                    existingLocation.Location = updatedLocation.Location;
                    existingLocation.LocationOrder = updatedLocation.LocationOrder;
                    existingLocation.PathURL = updatedLocation.PathURL;

                    // If a new image is uploaded for this location, update it
                    if (updatedLocation.ImagePath != null)
                    {
                        // Delete the existing image file if it exists
                        if (!string.IsNullOrEmpty(existingLocation.ImagePath))
                        {
                            FileHelper.Delete(existingLocation.ImagePath);
                        }

                        // Save the new image and update the ImagePath property
                        existingLocation.ImagePath = await FileHelper.SaveFileAsync(updatedLocation.ImagePath);
                    }
                }
                else // If the location does NOT exist in the update request, mark it as deleted
                {
                    existingLocation.IsDeleted = true; // Soft delete (instead of permanently removing it)
                }
            }

            // Loop through new locations that are not in the database yet (ID = 0)
            foreach (var newLocation in formFileTimeVM.FormFileLocationsWithPics.Where(l => l.Id == 0))
            {
                // Create a new LocationImage object and populate it with values from the request
                LocationImage locationImage = new LocationImage
                {
                    PathURL = newLocation.PathURL,           // Assign the new path URL
                    LocationOrder = newLocation.LocationOrder, // Set the display order
                    Location = newLocation.Location,         // Set the location name
                    TimeId = formFileTimeVM.Id,              // Associate it with the Time entity

                    // Save the new image file (if provided) and store its path
                    ImagePath = newLocation.ImagePath != null ?
                        await FileHelper.SaveFileAsync(newLocation.ImagePath) : null
                };

                // Add the newly created location to the database entity
                timeFromDB.LocationsWithPics.Add(locationImage);
            }

            // Save all changes to the database
            Time updatedTime = await timeRepository.UpdateAsync(timeFromDB);

            // Map the updated Time entity to a ViewModel and return it
            return automapper.Map<TimeVM>(updatedTime);
        }

        public async Task<int> DeleteTime(int id)
        {
            int result = await timeRepository.DeleteTime(id);
            return result;
        }

        public async Task<int> DeleteLocationImage(int id)
        {
            int result = await timeRepository.DeleteLocationImage(id);
            return result;
        }
        public async Task<LocationImage> GetLocationImage(int id)
        {
            LocationImage locationImage = await timeRepository.GetLocationImage(id);
            return locationImage;

        }


    }
}
