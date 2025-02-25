using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Your_Ride.Helper;
using Your_Ride.Models;
using Your_Ride.Repository.BusRepo;
using Your_Ride.Services.AppointmentServ;
using Your_Ride.Services.BusServ;
using Your_Ride.Services.TimeServ;
using Your_Ride.ViewModels.AppointmentviewModel;
using Your_Ride.ViewModels.BusViewModel;
using Your_Ride.ViewModels.TimeViewModel;

namespace Your_Ride.Controllers
{
    public class TimeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITimeService timeService;
        private readonly IBusService busService;
        private readonly IAppointmentService appointmentService;

        public TimeController(UserManager<ApplicationUser> userManager, ITimeService timeService, IBusService busService, IAppointmentService appointmentService)
        {
            this.userManager = userManager;
            this.timeService = timeService;
            this.busService = busService;
            this.appointmentService = appointmentService;
        }
        //[HttpGet]
        //public async Task<IActionResult> GetAllTimes()
        //{
        //    List<TimeVM> timeVMs = await timeService.GetAllTimes();

        //    List<BusVM> busVMs = await busService.GetAllBuses();
        //    List<AppointmentVM> appointmentVMs = await appointmentService.GetAllAppointments();
        //    ViewBag.Buses = busVMs;
        //    ViewBag.Appointments = appointmentVMs;
        //    return View("GetAllTimes", timeVMs);
        //}

        // /Time/GetAllTimes
        [HttpGet]
        public async Task<IActionResult> GetAllTimes(string search, string sortOrder, int page = 1, int pageSize = 10)
        {
            List<TimeVM> timeVMs = await timeService.GetAllTimes();

            // **Filtering (Search)**
            if (!string.IsNullOrEmpty(search))
            {
                timeVMs = timeVMs.Where(t =>
                    t.Bus.DriverName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    t.Bus.PlateNumber.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    t.Bus.NumberOfSeats.ToString().Contains(search) ||
                    t.TimeOnly.ToString("hh:mm tt").Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    t.Appointment.Date.ToString("MM/dd/yyyy").Contains(search)
                ).ToList();
            }

            // **Sorting**
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TimeSortParam = sortOrder == "time_asc" ? "time_desc" : "time_asc";
            ViewBag.DriverSortParam = sortOrder == "driver_asc" ? "driver_desc" : "driver_asc";

            timeVMs = sortOrder switch
            {
                "time_asc" => timeVMs.OrderBy(t => t.TimeOnly).ToList(),
                "time_desc" => timeVMs.OrderByDescending(t => t.TimeOnly).ToList(),
                "driver_asc" => timeVMs.OrderBy(t => t.Bus.DriverName).ToList(),
                "driver_desc" => timeVMs.OrderByDescending(t => t.Bus.DriverName).ToList(),
                _ => timeVMs
            };

            // **Pagination**
            int totalItems = timeVMs.Count;
            List<TimeVM> paginatedTimes = timeVMs.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PaginatedList<TimeVM> paginatedList = new PaginatedList<TimeVM>(paginatedTimes, totalItems, page, pageSize);

            return View("GetAllTimes", paginatedList);
        }

        [HttpGet]
        public async Task<IActionResult> GetTimeByID(int id)
        {
            TimeVM timeVM = await timeService.GetTimeByID(id);

            if (timeVM == null) return NotFound("No Time Appointment");

            return View("GetTimeByID", timeVM);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTimesByBusID(int id)
        {
            List<TimeVM> timeVMs = await timeService.GetAllTimesByBusID(id);

            return View("GetAllTimesByBusID", timeVMs);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTimesByAppointmentID(int id)
        {
            List<TimeVM> timeVMs = await timeService.GetAllTimesByAppointmentID(id);

            return View("GetAllTimesByAppointmentID", timeVMs);
        }
        // /Time/CreateTime
        [HttpGet]
        public async Task<IActionResult> CreateTime()
        {
            List<BusVM> busVMs = await busService.GetAllBuses();
            List<AppointmentVM> appointmentVMs = await appointmentService.GetAllAppointments();
            ViewBag.Buses = busVMs;
            ViewBag.Appointments = appointmentVMs;

            //TimeVM timeVM = new TimeVM();
            IFormFileTimeVM formFileTimeVM = new IFormFileTimeVM();

            return View("CreateTime", formFileTimeVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateTime(IFormFileTimeVM  formFileTimeVM)
        {
            if (!ModelState.IsValid)
            {
                List<BusVM> busVMs = await busService.GetAllBuses();
                List<AppointmentVM> appointmentVMs = await appointmentService.GetAllAppointments();
                ViewBag.Buses = busVMs;
                ViewBag.Appointments = appointmentVMs;
                return RedirectToAction("CreateTime", formFileTimeVM);
            }
            TimeVM TVM = await timeService.CreateTime(formFileTimeVM);
            if (TVM == null)
                return NotFound("Already have same time in that Appointment");
            return RedirectToAction("GetAllTimes");
        }

        [HttpGet]
        public async Task<IActionResult> EditTime(int id)
        {
            List<BusVM> busVMs = await busService.GetAllBuses();
            List<AppointmentVM> appointmentVMs = await appointmentService.GetAllAppointments();
            ViewBag.Buses = busVMs;
            ViewBag.Appointments = appointmentVMs;

            TimeVM timeVM = await timeService.GetTimeByID(id);
            if (timeVM == null)
            {
                return NotFound("Time");
            }

            return View("EditTime", timeVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditTime(IFormFileTimeVM formFileTimeVM)
        {
            if (!ModelState.IsValid)
            {

                List<BusVM> busVMs = await busService.GetAllBuses();
                List<AppointmentVM> appointmentVMs = await appointmentService.GetAllAppointments();
                ViewBag.Buses = busVMs;
                ViewBag.Appointments = appointmentVMs;
                return View("EditTime", formFileTimeVM);
            }
            TimeVM TVM = await timeService.EditTime(formFileTimeVM);
            if (TVM == null) return NotFound("Error Editing");
            return RedirectToAction("GetTimeByID", new { id = formFileTimeVM.Id });

        }

        [HttpGet]
        public async Task<IActionResult> DeleteTime(int id)
        {
            TimeVM timeVM = await timeService.GetTimeByID(id);
            if (timeVM == null)
            {
                return NotFound("Time");
            }
            return View("DeleteTime", timeVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteTime(TimeVM timeVM)
        {
            int result = await timeService.DeleteTime(timeVM.Id);
            if (result == -1) return NotFound("Time not found.");
            if (result == 0) return BadRequest("Time is already deleted.");

            return RedirectToAction("GetAllTimes");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteLocationImage(int id)
        {
            LocationImage locationImage = await timeService.GetLocationImage(id);
            if (locationImage == null)
            {
                return NotFound("Time");
            }
            return View("DeleteLocationImage", locationImage);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteLocationImage(LocationImage locationImage)
        {
            int result = await timeService.DeleteLocationImage(locationImage.Id);
            if (result == -1) return NotFound("Location or Image not found.");
            if (result == 0) return BadRequest("Location or Image is already deleted.");

            return RedirectToAction("GetAllTimes");
        }
    }
}
