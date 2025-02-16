using Microsoft.AspNetCore.Mvc;
using Your_Ride.Services.UniversityServ;
using Your_Ride.ViewModels.University;

namespace Your_Ride.Controllers
{
    public class UniversityController : Controller  
    {
        private readonly IUniversityService universityService;

        public UniversityController(IUniversityService universityService)
        {
            this.universityService = universityService;
        }

        // Display all universities
        //    University/GetAllUniversity
        public async Task<IActionResult> GetAllUniversity()
        {
            var universities = await universityService.GetAllUniversity();
            return View("GetAllUniversity",universities);
        }

        // Display University By ID
        public async Task<IActionResult> GetUniversityByID(int id)
        {
            var university = await universityService.GetUniversityById(id);
            return View("GetUniversity",university);
        }

        // Show create university form
        //        University/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View("CreateUniversity");
        }

        // Handle create university form submission
        [HttpPost]
        public async Task<IActionResult> Create(CreateUniversityVM model)
        {
            if (ModelState.IsValid)
            {
                await universityService.CreateUniversity(model);
                return RedirectToAction("Index","Home");
            }
            return View(model);
        }

        // Show edit form
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var university = await universityService.GetUniversityById(id);
            if (university == null)
            {
                return NotFound();
            }
            return View("EditUniversity", university);
        }

        // Handle update university
        [HttpPost]
        public async Task<IActionResult> Edit(int id, UniversityVM model)
        {
            if (ModelState.IsValid)
            {
                await universityService.UpdateUniversity(id, model);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        // Show delete confirmation
        // University/Delete?id=1
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var university = await universityService.GetUniversityById(id);

            return View("DeleteUniversity", university);
        }

        // Handle delete operation
        
        [HttpPost]

        public async Task<IActionResult> ConfirmDelete(int id)
        {
            int result = await universityService.DeleteUniversity(id);
            if (result == -1)
            {
                return NotFound();
            }
            else if (result == 0)
            {
                return Content("Already been Deleted ");
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}