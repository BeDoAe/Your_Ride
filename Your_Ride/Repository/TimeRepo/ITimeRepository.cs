using Your_Ride.Models;
using Your_Ride.Repository.Generic;

namespace Your_Ride.Repository.TimeRepo
{
    public interface ITimeRepository :IRepository<Time>
    {
        public Task<List<Time>> GetAllTimes();
        public Task<Time> GetTimeByID(int id);
        public Task<List<Time>> GetAllTimesByBusID(int id);
        public Task<List<Time>> GetAllTimesByAppointmentID(int id);
        public Task<Time> CreateTime(Time time);
        public Task<int> DeleteTime(int id);
        public Task<int> DeleteLocationImage(int id);
        public Task<LocationImage> GetLocationImage(int id);

        public Task<List<Time>> GetAppointmentsByBusGuideID(string id);

        public Task<LocationImage> AddLocationImage(int id, LocationImage locationImage);



    }
}