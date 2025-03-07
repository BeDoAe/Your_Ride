using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Your_Ride.Models;
using Your_Ride.Services.AppointmentServ;
using Your_Ride.Services.TimeServ;
using Your_Ride.Services.UserTransactionLogServ;
using Your_Ride.ViewModels.AppointmentviewModel;
using Your_Ride.ViewModels.TimeViewModel;
using Your_Ride.ViewModels.UserTransactionLogViewModel;

namespace Your_Ride.Controllers
{
    public class UserTransactionLogController : Controller
    {
        private readonly IUserTransactionLogService userTransactionLogService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITimeService timeService;
        private readonly IAppointmentService appointmentService;

        public UserTransactionLogController(IUserTransactionLogService userTransactionLogService, UserManager<ApplicationUser> userManager, ITimeService timeService, IAppointmentService appointmentService)
        {
            this.userTransactionLogService = userTransactionLogService;
            this.userManager = userManager;
            this.timeService = timeService;
            this.appointmentService = appointmentService;
        }
        //     /UserTransactionLog/GetAllUserTransactionsLogs
        [HttpGet]
        public async Task<IActionResult> GetAllUserTransactionsLogs()
        {
            List<UserTransactionLogVM> userTransactionLogVMs = await userTransactionLogService.GetAllUserTransactioLogs();
            return View("GetAllUserTransactionsLogs", userTransactionLogVMs);
        }
        [HttpGet]
        public async Task<IActionResult> GetUserTransactionsLogById(int id)
        {
            UserTransactionLogVM userTransactionLogVM = await userTransactionLogService.GetUserTransactioLogById(id);
            return View("GetUserTransactionsLogById", userTransactionLogVM);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUserTransactioLogsByUserId(string id)
        {
            List<UserTransactionLogVM> userTransactionLogVMs = await userTransactionLogService.GetAllUserTransactioLogsByUserId(id);
            return View("GetAllUserTransactioLogsByUserId", userTransactionLogVMs);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUserTransactioLogsByTimeId(int id)
        {
            List<UserTransactionLogVM> userTransactionLogVMs = await userTransactionLogService.GetAllUserTransactioLogsByTimeId(id);
            return View("GetAllUserTransactioLogsByTimeId", userTransactionLogVMs);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUserTransactioLogsByAppointmentId(int id)
        {
            List<UserTransactionLogVM> userTransactionLogVMs = await userTransactionLogService.GetAllUserTransactioLogsByAppointmentId(id);
            return View("GetAllUserTransactioLogsByAppointmentId", userTransactionLogVMs);
        }
        // /UserTransactionLog/CreateUserTransactionLog
        [HttpGet]
        public async Task<IActionResult> CreateUserTransactionLog()
        {
            // Get the logged-in user
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(); // Ensure user is authenticated
            }

            // Create the view model and set the UserId
            UserTransactionLogVM userTransactionLogVM = new UserTransactionLogVM
            {
                UserId = user.Id // Set the logged-in user's ID
            };

            // Get all appointments and pass to the view
            List<AppointmentVM> appointmentsVM = await appointmentService.GetAllAppointments();
            ViewBag.Appointments = appointmentsVM;

            return View("CreateUserTransactionLog", userTransactionLogVM);
        }


        // POST method to handle transaction creation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUserTransactionLog(UserTransactionLogVM userTransactionLogVM)
        {
            if (!ModelState.IsValid) // Validate the model
            {
                List<AppointmentVM> appointmentsVM = await appointmentService.GetAllAppointments();
                ViewBag.Appointments = appointmentsVM;
                return View(userTransactionLogVM);
            }

            // Ensure the logged-in user is the same as the one submitted in the form
            var user = await userManager.GetUserAsync(User);
            if (user == null || user.Id != userTransactionLogVM.UserId)
            {
                return Unauthorized(); // Ensure user is authenticated and is the same user
            }

            var result = await userTransactionLogService.CreateUserTransactionLog(userTransactionLogVM);
            if (result != null)
            {
                return RedirectToAction("GetAllUserTransactionsLogs");
            }
            return RedirectToAction("GetAllUserTransactionsLogs");
        }


        // AJAX Call to Get Times related to Appointment
        [HttpGet]
        public async Task<IActionResult> GetTimesByAppointmentId(int appointmentId)
        {
            var times = await timeService.GetAllTimesByAppointmentID(appointmentId);
            return Json(times); // Return JSON data
        }


        [HttpGet]
        public async Task<IActionResult> DeleteUserTransactionLog(int id)
        {
            UserTransactionLogVM userTransactionLogVM = await userTransactionLogService.GetUserTransactioLogById(id);
            if (userTransactionLogVM == null)
            {
                return NotFound("No User Transaction Found");
            }
            return View("DeleteUserTransactionLog", userTransactionLogVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteUserTransactionLog(UserTransactionLogVM userTransactionLogVM )
        {
            int result = await userTransactionLogService.DeleteUserTransactionLog(userTransactionLogVM.Id);
            if (result == -1) return NotFound("User Transaction not found.");
            if (result == 0) return BadRequest("User Transaction is already deleted.");

            return RedirectToAction("GetAllUserTransactionsLogs");
        }
    }
}
