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
            List<Bus> buses = await busRepository.GetAllBuses();

            List<BusVM> busVMs = automapper.Map<List<BusVM>>(buses);

            return busVMs;
        }
        public async Task<BusVM> GetBusByID(int  BusID )
        {
            Bus bus = await busRepository.GetBusByID(BusID);

            BusVM busVM = automapper.Map<BusVM>(bus);

            return busVM;
        }
        public async Task<BusVM> CreateBus(BusVM busVM)
        {
            Bus busFromDB = await busRepository.GetBusByID(busVM.Id);
            if (busFromDB == null)
            {
                //Generate Bus Tag
                string busIdentifier = await GenerateNextBusIdentifier();

                Bus bus = automapper.Map<Bus>(busVM);
                bus.BusIdentifier = busIdentifier;

                // Generate seats
                for (int i = 1; i <= bus.NumberOfSeats; i++)
                {
                    bus.Seats.Add(new Seat { SeatLabel = $"{busIdentifier}{i}" });
                }

                await busRepository.AddAsync(bus);

                return busVM;
            }
            return null;
        }

        public async Task<BusVM> EditBus(BusVM busVM)
        {
            Bus busFromDB = await busRepository.GetBusByID(busVM.Id);
            if (busFromDB == null)
            {
                return null;
            }
            automapper.Map(busVM, busFromDB);
            // Generate seats
            for (int i = 1; i <= busFromDB.NumberOfSeats; i++)
            {
                busFromDB.Seats.Add(new Seat { SeatLabel = $"{busFromDB.BusIdentifier}{i}" });
            }
            await busRepository.UpdateAsync(busFromDB);
            return automapper.Map<BusVM>(busFromDB);
        }
        public async Task<int> DeleteBus(int id)
        {
            int result = await busRepository.DeleteBus(id);
            return result;
        }


        public async Task<string> GenerateNextBusIdentifier()
        {
            // Fetch all existing bus identifiers from the database
            var existingIdentifiers = await busRepository.GetAllBusIdentifier();

            if (!existingIdentifiers.Any())
                return "A"; // First bus starts with 'A'

            // Get the last assigned identifier in alphabetical order
            string lastIdentifier = existingIdentifiers.OrderBy(id => id).Last();

            // Generate the next identifier
            return GetNextIdentifier(lastIdentifier);
        }

        private string GetNextIdentifier(string lastIdentifier)
        {
            char[] chars = lastIdentifier.ToCharArray();

            // Increment from the last character
            for (int i = chars.Length - 1; i >= 0; i--)
            {
                if (chars[i] < 'Z')
                {
                    chars[i]++;
                    return new string(chars);
                }
                chars[i] = 'A';
            }

            // If all characters were 'Z', add another character in front
            return 'A'+ new string(chars) ;
        }
        public async Task<List<BusVM>> GetAllAvailableBus()
        {
            List<Bus> buses = await busRepository.GetAllAvailableBus();

            List<BusVM> busVMs = automapper.Map<List<BusVM>>(buses);

            return busVMs;
        }
        public async Task<List<BusVM>> GetAllAvailableBus(int id)
        {
            List<Bus> buses = await busRepository.GetAllAvailableBus(id);

            List<BusVM> busVMs = automapper.Map<List<BusVM>>(buses);

            return busVMs;
        }
    }
}
