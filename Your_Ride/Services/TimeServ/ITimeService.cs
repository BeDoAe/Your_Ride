using Your_Ride.Models;
using Your_Ride.Services.Generic;
using Your_Ride.ViewModels.TimeViewModel;

namespace Your_Ride.Services.TimeServ
{
    public interface ITimeService : IService<Time>
    {
        public Task<List<TimeVM>> GetAllTimes();
        public Task<List<TimeVM>> GetAllTimesByBusID(int id);
        public Task<List<TimeVM>> GetAllTimesByAppointmentID(int id);
        public Task<TimeVM> GetTimeByID(int id);
        public Task<TimeVM> CreateTime(IFormFileTimeVM formFileTimeVM);
        public Task<TimeVM> EditTime(IFormFileTimeVM formFileTimeVM);
        public Task<int> DeleteTime(int id);

    }
}