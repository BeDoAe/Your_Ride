using Your_Ride.Models;
using Your_Ride.Services.Generic;
using Your_Ride.ViewModels.BusViewModel;

namespace Your_Ride.Services.BusServ
{
    public interface IBusService : IService<Bus>
    {
        public Task<List<BusVM>> GetAllBuses();
        public Task<BusVM> GetBusByID(int BusID);
        public Task<BusVM> CreateBus(BusVM busVM);
        public Task<BusVM> EditBus(BusVM busVM);
        public Task<int> DeleteBus(int id);

        public  Task<string> GenerateNextBusIdentifier();
        public Task<List<BusVM>> GetAllAvailableBus();


        public Task<List<BusVM>> GetAllAvailableBus(int id);

    }
}