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

    }
}
