using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Your_Ride.Models;
using Your_Ride.Models.Your_Ride.Models;
using Your_Ride.Services.AppointmentServ;
using Your_Ride.ViewModels.AppointmentviewModel;

namespace Your_Ride.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService appointmentService;
        private readonly UserManager<ApplicationUser> userManager;

        public AppointmentController(IAppointmentService appointmentService , UserManager<ApplicationUser> userManager)
        {
            this.appointmentService = appointmentService;
            this.userManager = userManager;
        }
        // GET: /Appointment/GetAllAppointments
        //public async Task<IActionResult> GetAllAppointments()
        //{
        //    List<AppointmentVM> appointmentVMs = await appointmentService.GetAllAppointments();
        //    return View("GetAllAppointments", appointmentVMs);
        //}
        public async Task<IActionResult> GetAllAppointments(string searchQuery, string sortOrder = "desc")
        {
            var appointments = await appointmentService.GetAllAppointments(searchQuery, sortOrder);
            ViewBag.SortOrder = sortOrder == "asc" ? "desc" : "asc"; // Toggle sorting
            ViewBag.SearchQuery = searchQuery;
            return View("GetAllAppointments", appointments);
        }


        // GET: /Appointment/GetAppointmentByID?id=
        public async Task<IActionResult> GetAppointmentByID(int id)
        {
            AppointmentVM appointmentVM = await appointmentService.GetAppointmentByID(id);
            if (appointmentVM == null) return NotFound("No Appointment Found");
            return View("GetAppointmentByID", appointmentVM);
        }
        //// GET: /Appointment/GetAllAppointmentsByBusGuideID?id=
        //public async Task<IActionResult> GetAllAppointmentsByBusGuideID(String id)
        //{
        //    List<AppointmentVM> appointmentVMs = await appointmentService.GetAppointmentsByBuisGuideID(id);
        //    return View("GetAllAppointments", appointmentVMs);
        //}
        // GET: /Appointment/CreateAppointment (Show Form)
        [HttpGet]
        public async Task<IActionResult> CreateAppointment()
        {
            //var users = await userManager.Users.ToListAsync();
            //ViewBag.Users = users; // Store users in ViewBag

            var appointmentVM = new AppointmentVM(); // Initialize an empty AppointmentVM
            return View("CreateAppointment", appointmentVM);
        }


        // POST: /Appointment/CreateAppointment (Submit Form)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAppointment(AppointmentVM appointmentVM)
        {
            if (ModelState.IsValid)
            {
                //ApplicationUser user = userManager.Users.FirstOrDefault(x => x.Id == appointmentVM.BusGuideId);
                //if (user == null)
                //{
                //    return RedirectToAction("CreateAppointment",appointmentVM);
                //}
                AppointmentVM createdAppointment = await appointmentService.CreateAppointment(appointmentVM);
                if (createdAppointment == null)
                {
                    ModelState.AddModelError("", "Failed to create appointment. Appointment of Same Date is Created .");
                    return View("CreateAppointment", appointmentVM);
                }
                return RedirectToAction("GetAllAppointments");
            }
            return View("CreateAppointment", appointmentVM);
        }

        // GET: /Appointment/EditAppointment?id=
        [HttpGet]
        public async Task<IActionResult> EditAppointment(int id)
        {
            AppointmentVM appointmentVM = await appointmentService.GetAppointmentByID(id);
            if (appointmentVM == null) return NotFound("Appointment not found");
            //List<ApplicationUser> users = await userManager.Users.ToListAsync();
            // ViewBag.users=users;
            return View("EditAppointment", appointmentVM);
        }

        // POST: /Appointment/EditAppointment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAppointment(AppointmentVM appointmentVM)
        {
            if (ModelState.IsValid)
            {
                AppointmentVM updatedAppointment = await appointmentService.EditAppointment(appointmentVM);
                if (updatedAppointment == null)
                {
                    ModelState.AddModelError("", "Update failed. Book or appointment not found.");
                    return View("EditAppointment", appointmentVM);
                }
                return RedirectToAction("GetAllAppointments");
            }
            //List<ApplicationUser> users = await userManager.Users.ToListAsync();
            //ViewBag.users = users;
            return View("EditAppointment", appointmentVM);
        }

        // GET: /Appointment/DeleteAppointment?id=
        [HttpGet]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            AppointmentVM appointmentVM = await appointmentService.GetAppointmentByID(id);
            if (appointmentVM == null) return NotFound("Appointment not found");

            return View("DeleteAppointment", appointmentVM);
        }

        // POST: /Appointment/DeleteAppointment
        [HttpPost, ActionName("DeleteAppointment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeleteAppointment(int id)
        {
            int result = await appointmentService.DeleteAppointment(id);
            if (result == -1) return NotFound("Appointment not found.");
            if (result == 0) return BadRequest("Appointment is already deleted.");

            return RedirectToAction("GetAllAppointments");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteAppointment(int id)
        {
            AppointmentVM appointmentVM = await appointmentService.CompleteAppointment(id);
            if (appointmentVM == null)
                return NotFound("No Appointment Found");
            return RedirectToAction("GetAppointmentByID", new { id = id });
        }
    }

}