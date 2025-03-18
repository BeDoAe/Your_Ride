using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Your_Ride.Helper;
using Your_Ride.Models;
using Your_Ride.Models.Your_Ride.Models;
using Your_Ride.Repository.AppointmentRepo;
using Your_Ride.Repository.BookRepo;
using Your_Ride.Repository.BusRepo;
using Your_Ride.Repository.TimeRepo;
using Your_Ride.Services.Generic;
using Your_Ride.ViewModels.AppointmentviewModel;
using Your_Ride.ViewModels.TimeViewModel;

namespace Your_Ride.Services.TimeServ
{
    public class TimeService :Service<Time> , ITimeService
    {
        private readonly IMapper automapper;
        private readonly ITimeRepository timeRepository;
        private readonly IBusRepository busRepository;
        private readonly IAppointmentRepository appointmentRepository;
        private readonly IBookRepository bookRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public TimeService(IMapper automapper , 
            ITimeRepository timeRepository ,
            IBusRepository busRepository , 
            IAppointmentRepository appointmentRepository ,
            IBookRepository bookRepository ,
            UserManager<ApplicationUser> userManager)
        {
            this.automapper = automapper;
            this.timeRepository = timeRepository;
            this.busRepository = busRepository;
            this.appointmentRepository = appointmentRepository;
            this.bookRepository = bookRepository;
            this.userManager = userManager;
        }

        public async Task<List<TimeVM>> GetAllTimes()
        {
            List<Time> times = await timeRepository.GetAllTimes();

            List<TimeVM> timeVMs = automapper.Map<List<TimeVM>>(times);
            return timeVMs;
        }
        public async Task<List<TimeVM>> GetAllAvailableTimes()
        {
            List<Time> times = await timeRepository.GetAllAvailableTimes();

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
            List<Time> times = await timeRepository.GetAllTimesByAppointmentID(id);
            List<TimeVM> timeVMs = automapper.Map<List<TimeVM>>(times);
            foreach (var timeVM in timeVMs)
            {
                // Ensure Category is properly mapped to the enum
                if (timeVM.Category == 0)
                {
                    timeVM.Category = TimeVM.TripCategory.Arrival;
                }
                else 
                {
                    timeVM.Category = TimeVM.TripCategory.Departure;
                }
                timeVM.FormattedTime = timeVM.TimeOnly.ToString("hh:mm tt");
            }


            return timeVMs;
        }
        public async Task<List<TimeVM>> GetAppointmentsByBuisGuideID(string id)
        {
            List<Time> times = await timeRepository.GetAppointmentsByBusGuideID(id);

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
              ApplicationUser AdminBusGuide = await userManager.Users.FirstOrDefaultAsync(x=>x.Id==formFileTimeVM.BusGuideId);
            bool isAdmin = await userManager.IsInRoleAsync(AdminBusGuide, "Admin");
            //if (isAdmin != true) return null;

            if (bus == null || appointment == null || isAdmin != true) return null;

            Time Newtime = automapper.Map<Time>(formFileTimeVM);
            Newtime.Appointment = appointment;
            Newtime.Bus = bus;
            Newtime.BusGuide = AdminBusGuide;
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
                    locationImage.ImagePath = await FileHelper.SaveFileAsync(LC.ImageFile);

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

        //public async Task<IFormFileTimeVM> EditTime(IFormFileTimeVM formFileTimeVM)
        //{
        //    // Retrieve existing Time object from DB
        //    Time timeFromDB = await timeRepository.GetTimeByID(formFileTimeVM.Id);

        //    if (timeFromDB == null)
        //        return null;

        //    ApplicationUser AdminBusGuide = await userManager.Users.FirstOrDefaultAsync(x => x.Id == formFileTimeVM.BusGuideId);
        //    bool isAdmin = await userManager.IsInRoleAsync(AdminBusGuide, "Admin");
        //    if (!isAdmin) return null;

        //    // Update each existing location or mark as deleted
        //    foreach (var existingLocation in timeFromDB.LocationsWithPics.ToList())
        //    {
        //        var updatedLocation = formFileTimeVM.FormFileLocationsWithPics
        //            .FirstOrDefault(l => l.Id == existingLocation.Id);

        //        if (updatedLocation != null)
        //        {
        //            existingLocation.TimeId = updatedLocation.TimeId;
        //            existingLocation.Location = updatedLocation.Location;
        //            existingLocation.LocationOrder = updatedLocation.LocationOrder;
        //            existingLocation.PathURL = updatedLocation.PathURL;

        //            // Handle new image upload
        //            if (updatedLocation.ImagePath != null)
        //            {
        //                // Delete old file
        //                if (!string.IsNullOrEmpty(existingLocation.ImagePath))
        //                {
        //                    FileHelper.Delete(existingLocation.ImagePath);
        //                }

        //                // Save new file
        //                existingLocation.ImagePath = await FileHelper.SaveFileAsync(updatedLocation.ImageFile);
        //            }
        //        }
        //        else
        //        {
        //            existingLocation.IsDeleted = true;
        //        }
        //    }

        //    // Add new locations if they have Id = 0
        //    foreach (var newLocation in formFileTimeVM.FormFileLocationsWithPics.Where(l => l.Id == 0))
        //    {
        //        LocationImage locationImage = new LocationImage
        //        {
        //            PathURL = newLocation.PathURL,
        //            LocationOrder = newLocation.LocationOrder,
        //            Location = newLocation.Location,
        //            TimeId = formFileTimeVM.Id,
        //            ImagePath = newLocation.ImagePath != null ? await FileHelper.SaveFileAsync(newLocation.ImageFile) : null
        //        };

        //        timeFromDB.LocationsWithPics.Add(locationImage);
        //    }

        //    // Save updates
        //    Time updatedTime = await timeRepository.UpdateAsync(timeFromDB);

        //    // Map the updated Time entity to ViewModel
        //    return automapper.Map<IFormFileTimeVM>(updatedTime);
        //}
        #endregion


        public async Task<IFormFileTimeVM> EditTime(IFormFileTimeVM formFileTimeVM)
        {
            Time timeFromDB = await timeRepository.GetTimeByID(formFileTimeVM.Id);
            if (timeFromDB == null) return null;

            // Validate Bus Guide Role
            ApplicationUser AdminBusGuide = await userManager.Users.FirstOrDefaultAsync(x => x.Id == formFileTimeVM.BusGuideId);
            if (AdminBusGuide == null || !(await userManager.IsInRoleAsync(AdminBusGuide, "Admin"))) return null;
            if(formFileTimeVM.Category == IFormFileTimeVM.TripCategory.Arrival)
            {
                timeFromDB.DueDateArrivalSubmission = null;

            }else if (formFileTimeVM.Category == IFormFileTimeVM.TripCategory.Departure)
            {
                timeFromDB.DueDateArrivalSubmission = null;
            }
            // Update Fields
            timeFromDB.Fee = formFileTimeVM.Fee;
            timeFromDB.Category = (Time.TripCategory)formFileTimeVM.Category;
            timeFromDB.TimeOnly = formFileTimeVM.TimeOnly;
            timeFromDB.BusGuideId = formFileTimeVM.BusGuideId;
            timeFromDB.AppointmentId = formFileTimeVM.AppointmentId;
            timeFromDB.BusID = formFileTimeVM.BusID;



            // Update Locations with Images
            foreach (var existingLocation in timeFromDB.LocationsWithPics.ToList())
            {
                var updatedLocation = formFileTimeVM.FormFileLocationsWithPics.FirstOrDefault(l => l.Id == existingLocation.Id);
                if (updatedLocation != null)
                {
                    existingLocation.Location = updatedLocation.Location;
                    existingLocation.LocationOrder = updatedLocation.LocationOrder;
                    existingLocation.PathURL = updatedLocation.PathURL;

                    if (updatedLocation.ImageFile != null)
                    {
                        if (!string.IsNullOrEmpty(existingLocation.ImagePath))
                        {
                            FileHelper.Delete(existingLocation.ImagePath);
                        }
                        existingLocation.ImagePath = await FileHelper.SaveFileAsync(updatedLocation.ImageFile);
                    }
                }
                else
                {
                    existingLocation.IsDeleted = true;
                }
            }

            // Save to Database
            Time timeAfterSaving = await timeRepository.UpdateTime(timeFromDB);
            return automapper.Map<IFormFileTimeVM>(timeAfterSaving);
        }


        public IFormFileTimeVM MappingToFormFile(TimeVM timeVM)
        {
            IFormFileTimeVM fileTimeVM = automapper.Map<IFormFileTimeVM>(timeVM);
            return fileTimeVM;
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
        public async Task<bool> AddLocationImages(int id, List<FormFileLocationPics> formFileLocationImageVMs)
        {
            List<LocationImage> locationImages = automapper.Map<List<LocationImage>>(formFileLocationImageVMs);

            for (int i = 0; i < formFileLocationImageVMs.Count; i++)
            {
                locationImages[i].ImagePath = await FileHelper.SaveFileAsync(formFileLocationImageVMs[i].ImageFile);
            }

            return await timeRepository.AddLocationImages(id, locationImages);
        }
        #region Old Add Location
        //public async Task<bool> AddLocationImages(int timeId, List<FormFileLocationPics> formFileLocationImageVMs)
        //{
        //    // Fetch existing orders for this TimeId
        //    List<int> existingOrders = await timeRepository.GetTimeLocationOrder(timeId);

        //    // Find the next expected order number
        //    int expectedOrder = (existingOrders.Count > 0) ? existingOrders.Max() + 1 : 1;

        //    foreach (var newLocation in formFileLocationImageVMs)
        //    {
        //        if (newLocation.LocationOrder != expectedOrder)
        //        {
        //            // Order is incorrect
        //            return false;  // Or throw a custom exception
        //        }

        //        expectedOrder++;  // Move to the next expected number
        //    }

        //    // Proceed with saving
        //    List<LocationImage> locationImages = automapper.Map<List<LocationImage>>(formFileLocationImageVMs);

        //    for (int i = 0; i < formFileLocationImageVMs.Count; i++)
        //    {
        //        locationImages[i].ImagePath = await FileHelper.SaveFileAsync(formFileLocationImageVMs[i].ImageFile);
        //    }

        //    return await timeRepository.AddLocationImages(timeId, locationImages);
        //} 
        #endregion


        public List<FormFileLocationPics> MapToFormFileLocationImages(List<LocationImage> locationImages)
        {
            return automapper.Map<List<FormFileLocationPics>>(locationImages);
        }
        public async Task<LocationImage> GetLocationImage(int id)
        {
            LocationImage locationImage = await timeRepository.GetLocationImage(id);
            return locationImage;

        }
        public async Task<List<LocationImage>> GetLocationImagessByTimeID(int id)
        {
            List<LocationImage> locationImages = await timeRepository.GetLocationImagessByTimeID(id);   
            return locationImages;
        }

        public async Task<List<int>> GetTimeLocationOrder(int timeId)
        {
            List<int> orders = await timeRepository.GetTimeLocationOrder(timeId);
            return orders;
        }

        public async Task<TimeVM> CompleteTime(int id)
        {
            Time timeFromDB = await timeRepository.GetTimeByID(id);
            List<Seat> seats = await busRepository.GetAllSeatsByBus(timeFromDB.BusID);
            if (timeFromDB != null)
            {
                timeFromDB.HasCompleted = true;
                foreach(Seat s in seats)
                {
                    s.IsAvailable = true;

                }
                await timeRepository.SaveDB();
                return automapper.Map<TimeVM>(timeFromDB);
            }
            return null;
        }

    }
}
