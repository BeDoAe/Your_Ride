using Your_Ride.Models;
using Your_Ride.Repository.Generic;

namespace Your_Ride.Repository.TimeRepo
{
    public interface ITimeRepository :IRepository<Time>
    {
        public Task<List<Time>> GetAllTimes();
        public Task<List<Time>> GetAllAvailableTimes();

        public Task<Time> GetTimeByID(int id);
        public Task<List<Time>> GetAllTimesByBusID(int id);
        public Task<List<Time>> GetAllTimesByAppointmentID(int id);
        public Task<Time> CreateTime(Time time);
        public Task<int> DeleteTime(int id);
        public Task<int> DeleteLocationImage(int id);
        public Task<LocationImage> GetLocationImage(int id);

        public Task<List<Time>> GetAppointmentsByBusGuideID(string id);

        public Task<bool> AddLocationImages(int id, List<LocationImage> locationImages);
        public Task<Time> UpdateTime(Time time);


        public Task<List<LocationImage>> GetLocationImagessByTimeID(int id);

        public Task<List<int>> GetTimeLocationOrder(int timeId);

    }
}