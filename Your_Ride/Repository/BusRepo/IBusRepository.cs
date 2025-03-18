using Your_Ride.Models;
using Your_Ride.Repository.Generic;

namespace Your_Ride.Repository.BusRepo
{
    public interface IBusRepository :IRepository<Bus>
    {
        public Task<int> DeleteBus(int id);

        public  Task<List<Bus>> GetAllBuses();

        public Task<Bus> GetBusByID(int id);

        public Task<List<string>> GetAllBusIdentifier();
        public Task<List<Bus>> GetAllAvailableBus();
        public Task<List<Bus>> GetAllAvailableBus(int id);

        public Task<Bus> GetBusByIdentifier(char c);

        public Task<List<Seat>> GetAllSeatsByBus(int id);


    }

}