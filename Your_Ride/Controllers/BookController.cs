using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Your_Ride.Models;
using Your_Ride.Repository.BookRepo;
using Your_Ride.Services.BookServ;
using Your_Ride.Services.TimeServ;
using Your_Ride.ViewModels.BookViewModel;
using Your_Ride.ViewModels.TimeViewModel;

namespace Your_Ride.Controllers
{
    public class BookController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IBookService bookService;
        private readonly ITimeService timeService;

        public BookController(UserManager<ApplicationUser> userManager, IBookService bookService , ITimeService timeService)
        {
            this.userManager = userManager;
            this.bookService = bookService;
            this.timeService = timeService;
        }
        //         /Book/GetAllBooks
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            List<BookVM> bookVMs = await bookService.GetAllBooks();
            return View("GetAllBooks", bookVMs);
        }

        //         /Book/GetBookByID?id=
        [HttpGet]
        public async Task<IActionResult> GetBookByID(int id)
        {
            BookVM bookVM = await bookService.GetBookByID(id);
            return View("GetBookByID", bookVM);
        }
        //         /Book/GetAllTimesToBook
        [HttpGet]
        public async Task<IActionResult> GetAllTimesToBook()
        {
            List<TimeVM> timeVMs = await timeService.GetAllAvailableTimes();
            return View("GetAllTimesToBook",timeVMs);
        }
        [HttpGet]
        public async Task<IActionResult> GetBookTimeByID(int id)
        {
            TimeVM timeVM = await timeService.GetTimeByID(id);
            if (timeVM == null) return NotFound("No Time Found !!");
                return View("GetBookTimeByID", timeVM);
        }
        //         /Book/GetAllBooksOfUser?id=
        [HttpGet]
        public async Task<IActionResult> GetAllBooksOfUser(string id)
        {
            ApplicationUser applicationUser = await userManager.FindByIdAsync(id);
            if (applicationUser == null) return NotFound("No User Found");

            List<BookVM> bookVMs = await bookService.GetAllBooksOfUser(id);
            if (bookVMs == null) return NotFound("This User Didn't Book anything Yet !");
            return View("GetAllBooksOfUser", bookVMs);
        }
        //         /Book/CreateBook
        [HttpGet]
        public async Task<IActionResult> CreateBook()
        {

            return View("CreateBook");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBook(BookVM bookVM)
        {
            var user = await userManager.GetUserAsync(User);

            bookVM.UserID = user.Id;

            BookUserTransactionVM  bookUserTransactionVM = await bookService.CreateBook(bookVM);
            if (bookUserTransactionVM == null)  
            {
                return RedirectToAction("GetAllBooks");
            }else
            {
                return View(bookVM);
            }

        }

        //         /Book/EditBook?id=
        [HttpGet]
        public async Task<IActionResult> EditBook(int id , int UserTransactionLogID)
        {
            List<TimeVM> timeVMs = await timeService.GetAllAvailableTimes();

            BookVM bookVM = await bookService.GetBookByID(id);
            if (bookVM == null) return NotFound("Book isn't Found");
            ViewBag.UserTransactionLogID = UserTransactionLogID;
            ViewBag.AvaiableTimes = timeVMs;
            return View("EditBook", bookVM );

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBook(BookVM bookVM , TimeVM NewtimeVM , int UserTransactionLogID)
        {
            if (ModelState.IsValid)
            {
                BookVM bookVMFromDB = await bookService.EditBook(bookVM , NewtimeVM , UserTransactionLogID);
                if (bookVMFromDB == null) return RedirectToAction("EditBook", bookVM);

                //return RedirectToAction("GetBookByID", bookVM);
                return RedirectToAction("GetAllBooks");

            }
            else
            {
                return RedirectToAction("EditBook", bookVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteBook(int id)
        {
            BookVM bookVM = await bookService.GetBookByID(id);
            if (bookVM == null) return NotFound("Book isn't Found");

            return View("DeleteBook", bookVM);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            if (ModelState.IsValid)
            {
                int result = await bookService.DeleteBook(id);
                if (result == -1)
                {
                    return NotFound("Book isn't Found");
                }
                else if (result == 0)
                {
                    return Content("Already been Deleted");
                }
                else
                {
                    return RedirectToAction("GetAllBooks");
                }
            }
            return RedirectToAction("EditBook", new { id = id });
        }
    }
}
