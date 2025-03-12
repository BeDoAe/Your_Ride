using Microsoft.EntityFrameworkCore;
using Your_Ride.Models;
using Your_Ride.Repository.Generic;

namespace Your_Ride.Repository.BusRepo
{
    public class BusRepository:Repository<Bus> , IBusRepository
    {
        private readonly Context context;
        public BusRepository(Context context) : base(context)
        { 
            this.context = context;
        }
        public async Task<List<Bus>> GetAllBuses()
        {
            List<Bus> buses = await context.Buses.Include(x=>x.Seats).ToListAsync();
            return buses;
        }
        public async Task<Bus> GetBusByID(int id)
        {
            Bus bus = await context.Buses.Include(x => x.Seats).FirstOrDefaultAsync(x=>x.Id==id);
            if (bus == null)
            {
                return null;
            }
            return bus;
        }
        public async Task<int> DeleteBus(int id)
        {
            Bus bus = await context.Buses.FirstOrDefaultAsync(x => x.Id == id);

            if (bus == null)
            {
                return -1;

            }
            else if (bus.IsDeleted == true)
            {
                return 0;
            }
            else
            {
                bus.IsDeleted = true;
                await SaveDB();
                return 1;
            }
        }
        public async Task<List<string>> GetAllBusIdentifier ()
        {
            List<string> busIdentifiers = await context.Buses.Include(x=>x.Seats).Select(x=>x.BusIdentifier).ToListAsync();

            return busIdentifiers;
        }

        public async Task<List<Bus>> GetAllAvailableBus()
        {
            List<int> usedBusIds = await context.Times.Where(x=>x.IsDeleted ==false).Select(t => t.BusID).Distinct().ToListAsync();

            List<Bus> availableBuses = await context.Buses
                .Include(b => b.Seats)
                .Where(b => !usedBusIds.Contains(b.Id))
                .ToListAsync();

            return availableBuses;
        }
        public async Task<List<Bus>> GetAllAvailableBus(int id)
        {
            List<int> usedBusIds = await context.Times
                .Where(x => x.IsDeleted == false)
                .Select(t => t.BusID)
                .Distinct().ToListAsync();

            List<Bus> availableBuses = await context.Buses
                .Include(b => b.Seats)
                .Where(b => !usedBusIds.Contains(b.Id) || b.Id == id)
                .ToListAsync();

            return availableBuses;
        }

        public async Task<Bus> GetBusByIdentifier(char c)
        {
           string busIdentifier = c.ToString();
            Bus bus = await context.Buses.Include(x => x.Seats).FirstOrDefaultAsync(x => x.BusIdentifier == busIdentifier);

            return bus;
        }
    }
}
