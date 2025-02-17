using Microsoft.AspNetCore.Mvc;
using Your_Ride.Repository.BusRepo;
using Your_Ride.Services.BusServ;
using Your_Ride.ViewModels.BusViewModel;

namespace Your_Ride.Controllers
{
    public class BusController : Controller
    {
        private readonly IBusService busService;

        public BusController(IBusService busService)
        {
            this.busService = busService;
        }
        //         /Bus/GetAllBuses
        [HttpGet]
      public async Task<ActionResult> GetAllBuses()
        {
            List<BusVM> busVMs= await busService.GetAllBuses();
            return View("GetAllBuses", busVMs);

        }
        [HttpGet]
        public async Task<ActionResult> GetBusByID(int BusID)
        {
            BusVM busVM = await busService.GetBusByID(BusID);
            return View("GetBusByID", busVM);

        }
        //         /Bus/CreateBus
        [HttpGet]
        public async Task<ActionResult> CreateBus()
        {
            return View("CreateBus");
        }
            [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateBus(BusVM busVM)
        {
            if (ModelState.IsValid)
            {
                BusVM busFromService = await busService.CreateBus(busVM);
                if (busFromService == null)
                    ModelState.AddModelError("", "Bus already exists");

                return RedirectToAction("GetAllBuses");

            }
            return View("CreateBus", busVM);
        }

        [HttpGet]
        public async Task<ActionResult> EditBus(int BusID)
        {
            BusVM busVM = await busService.GetBusByID(BusID);
            if (busVM == null)
            {
                return NotFound("No Bus with this ID Found");
            }
            return View("EditBus", busVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditBus(BusVM busVM)
        {
            if (ModelState.IsValid)
            {
                BusVM busFromService = await busService.EditBus(busVM);
                if (busFromService == null)
                {
                    return NotFound("No Bus with this ID Found");
                }
                return RedirectToAction("GetBusByID", new { BusID = busFromService.Id });
            }
            return View("EditBus", busVM);
        }
        [HttpGet]
        public async Task<ActionResult> DeleteBus(int BusID)
        {
            BusVM busVM = await busService.GetBusByID(BusID);
            if (busVM == null)
            {
                return NotFound("No Bus with this ID Found");
            }
            return View("DeleteBus", busVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>ConfrimDeleteBus([FromForm]  int Id)
        {
            int result = await busService.DeleteBus(Id);
            if (result == -1)
            {
                return NotFound("No Bus with this ID Found");
            }
            else if (result == 0)
            {
                return Content("Already been Deleted");
            }
            return RedirectToAction("GetAllBuses");
        }
    }
}
