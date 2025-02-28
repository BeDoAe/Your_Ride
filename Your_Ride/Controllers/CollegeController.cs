using Microsoft.AspNetCore.Mvc;
using Your_Ride.Services.CollegeServ;
using Your_Ride.Services.UniversityServ;
using Your_Ride.ViewModels.College;
using Your_Ride.ViewModels.University;

namespace Your_Ride.Controllers
{
    public class CollegeController : Controller
    {
        private readonly ICollegeService collegeService;

        private readonly IUniversityService UniversityService;

        public CollegeController(ICollegeService collegeService , IUniversityService universityService)
        {
            this.collegeService = collegeService;
            this.UniversityService = universityService;
        }


        // Display all Colleges
        //    College/GetAllCollege
        public async Task<IActionResult> GetAllCollege()
        
        {
            List<CollegeVM> collegesVM = await collegeService.GetAllCollege();
            return View("GetAllCollege", collegesVM);
        }

        // Display College By ID
        public async Task<IActionResult> GetCollegeByID(int id)
        {
            CollegeVM collegeVM = await collegeService.GetCollegeById(id);
            return View("GetCollege", collegeVM);
        }

        // Show create College form
        //        College/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<UniversityVM> universityVMs = await UniversityService.GetAllUniversity();
            UniversityCollegesVM universityCollegesVM = new UniversityCollegesVM();
            universityCollegesVM.Universities = universityVMs;
            return View("CreateCollege",universityCollegesVM);
        }

        // Handle create College form submission
        [HttpPost]
        public async Task<IActionResult> Create(CreateCollege createCollege)
        {
            if (ModelState.IsValid)
            {
                await collegeService.CreateUniversity(createCollege);
                return RedirectToAction("Index", "Home");
            }
            return View(createCollege);
        }

        // Show edit form
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var college = await collegeService.GetCollegeById(id);
            if (college == null)
            {
                return NotFound();
            }

            List<UniversityVM> universityVMs = await UniversityService.GetAllUniversity();

            var EditCollegeVM = new EditCollegeVM
            {
                Universities = universityVMs,
                CollegeVM = college
            };

            return View("EditCollege", EditCollegeVM);
        }

        // Handle update College
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditCollegeVM editCollegeVM)
        {
            if (ModelState.IsValid)
            {
                var collegeVM = editCollegeVM.CollegeVM;

                if (collegeVM != null)
                {
                    await collegeService.UpdateCollege(id, collegeVM);
                    return RedirectToAction("GetAllCollege");
                }
            }

            // Reload universities if the form is invalid
            editCollegeVM.Universities = await UniversityService.GetAllUniversity();
            return View("EditCollege", editCollegeVM);
        }


        // Show delete confirmation
        // College/Delete?id=1
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var college = await collegeService.GetCollegeById(id);

            return View("DeleteCollege", college);
        }

        // Handle delete operation

        [HttpPost]

        public async Task<IActionResult> ConfirmDelete(int id)
        {
            int result = await collegeService.DeleteCollege(id);
            if (result == -1)
            {
                return NotFound();
            }
            else if (result == 0)
            {
                return Content("Already been Deleted ");
            }
            else return RedirectToAction("GetAllCollege");

        }
    }
}