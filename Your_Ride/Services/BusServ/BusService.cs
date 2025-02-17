using AutoMapper;
using Your_Ride.Models;
using Your_Ride.Repository.BusRepo;
using Your_Ride.Services.Generic;
using Your_Ride.ViewModels.BusViewModel;

namespace Your_Ride.Services.BusServ
{
    public class BusService : Service<Bus> , IBusService
    {
        private readonly IBusRepository busRepository;
        private readonly IMapper automapper;
        public BusService(IMapper automapper , IBusRepository busRepository)
        {
           this.automapper = automapper;
            this.busRepository = busRepository;
        }

        public async Task<List<BusVM>> GetAllBuses()
        {
            List<Bus> buses = await busRepository.GetAllAsync();

            List<BusVM> busVMs = automapper.Map<List<BusVM>>(buses);

            return busVMs;
        }
        public async Task<BusVM> GetBusByID(int  BusID )
        {
            Bus bus = await busRepository.GetByIdAsync(BusID);

            BusVM busVM = automapper.Map<BusVM>(bus);

            return busVM;
        }
        public async Task<BusVM> CreateBus(BusVM busVM)
        {
            Bus busFromDB = await busRepository.GetByIdAsync(busVM.Id);
            if (busFromDB == null)
            {
                Bus bus = automapper.Map<Bus>(busVM);
                await busRepository.AddAsync(bus);
                return busVM;
            }
            return null;
        }

        public async Task<BusVM> EditBus(BusVM busVM)
        {
            Bus busFromDB = await busRepository.GetByIdAsync(busVM.Id);
            if (busFromDB == null)
            {
                return null;
            }
            automapper.Map(busVM, busFromDB);

            await busRepository.UpdateAsync(busFromDB);
            return automapper.Map<BusVM>(busFromDB);
        }
        public async Task<int> DeleteBus(int id)
        {
            int result = await busRepository.DeleteBus(id);
            return result;
        }
    }
}
