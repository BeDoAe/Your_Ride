using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        public async Task<IActionResult> GetAllTimes()
        {
            List<TimeVM> timeVMs = await timeService.GetAllTimes();

            List<BusVM> busVMs = await busService.GetAllBuses();
            List<AppointmentVM> appointmentVMs = await appointmentService.GetAllAppointments();
            ViewBag.Buses = busVMs;
            ViewBag.Appointments = appointmentVMs;
            return View("GetAllTimes", timeVMs);
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
    }
}
