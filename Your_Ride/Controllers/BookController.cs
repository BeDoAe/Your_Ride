using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Your_Ride.Helper;
using Your_Ride.Models;
using Your_Ride.Repository.BookRepo;
using Your_Ride.Services.BookServ;
using Your_Ride.Services.TimeServ;
using Your_Ride.Services.UserTransactionLogServ;
using Your_Ride.ViewModels.BookViewModel;
using Your_Ride.ViewModels.TimeViewModel;
using Your_Ride.ViewModels.UserTransactionLogViewModel;

namespace Your_Ride.Controllers
{
    public class BookController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IBookService bookService;
        private readonly ITimeService timeService;
        private readonly IUserTransactionLogService userTransactionLogService;

        public BookController(UserManager<ApplicationUser> userManager, IBookService bookService , ITimeService timeService ,IUserTransactionLogService userTransactionLogService)
        {
            this.userManager = userManager;
            this.bookService = bookService;
            this.timeService = timeService;
            this.userTransactionLogService = userTransactionLogService;
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
            var user = await userManager.GetUserAsync(User);
            if (user == null) return Unauthorized("No User Found , Please Login First");
            BookVM bookVM = await bookService.GetBookByID(id);
            if (bookVM == null)
            {
                return NotFound("Book not found");
            }
            UserTransactionLogVM userTransactionLogVM = await userTransactionLogService.GetUserTransactioLogByTimeIdUserId(bookVM.timeId, user.Id);
            if (userTransactionLogVM == null)
            {
                return NotFound("User Transaction not found");
            }
            ViewBag.UserTransactionLogID = userTransactionLogVM.Id;
            return View("GetBookByID", bookVM);
        }

        //         /Book/GetAllTimesToBook
        [HttpGet]
        public async Task<IActionResult> GetAllAvailableTimesToBook()
        {
            List<TimeVM> timeVMs = await timeService.GetAllAvailableTimes();


            // ✅ Convert DateTime to DateOnly safely
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            // ✅ Ensure we exclude already booked times
            timeVMs = timeVMs
                .Where(t =>
                    t.Appointment != null &&
                    t.Appointment.Date >= today ) 
                .ToList();
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
        public async Task<IActionResult> GetAllBooksOfUser(string id, DateOnly? searchDate, int pageNumber = 1, int pageSize = 5)
        {
            var applicationUser = await userManager.FindByIdAsync(id);
            if (applicationUser == null) return NotFound("No User Found");

            var allBookings = await bookService.GetAllBooksOfUser(id);

            // Filter by date if searchDate is provided
            if (searchDate.HasValue)
            {
                allBookings = allBookings.Where(b => b.Time?.Appointment?.Date == searchDate.Value).ToList();
            }

            // Get user transaction logs
            var userTransactionLogVMs = await userTransactionLogService.GetAllUserTransactioLogsByUserId(id);
            ViewBag.userTransactionLogVMs = userTransactionLogVMs;

            // Apply pagination
            var paginatedList = new PaginatedList<BookVM>(
                allBookings.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(),
                allBookings.Count,
                pageNumber,
                pageSize
            );

            return View("GetAllBooksOfUser", paginatedList);
        }
        //         /Book/CreateBook
        //[HttpGet]
        //public async Task<IActionResult> CreateBook()
        //{

        //    return View("CreateBook");

        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBook(BookVM bookVM)
        {
            var user = await userManager.GetUserAsync(User);

            bookVM.UserID = user.Id;

            BookUserTransactionVM  bookUserTransactionVM = await bookService.CreateBook(bookVM);
            if (bookUserTransactionVM == null)  
            {
                return RedirectToAction("GetAllBooksOfUser", new { id = user.Id });
            }
            ViewBag.UserTransactionLogID = bookUserTransactionVM.userTransactionLogID;
            return RedirectToAction("GetBookByID", new {id= bookUserTransactionVM.bookVM.Id});



        }

        //         /Book/EditBook?id=
        [HttpGet]
        public async Task<IActionResult> EditBook(int id, int UserTransactionLogID)
        {
            BookVM bookVM = await bookService.GetBookByID(id);
            if (bookVM == null) return NotFound("Book isn't Found");

            List<TimeVM> timeVMs = await timeService.GetAllAvailableTimes();

            var user = await userManager.GetUserAsync(User);
            if (user == null) return Unauthorized("Should Login");

            List<BookVM> bookVMs = await bookService.GetAllBooksOfUser(user.Id);

            // ✅ Convert DateTime to DateOnly safely
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            // ✅ Ensure we exclude already booked times
            timeVMs = timeVMs
                .Where(t =>
                    t.Appointment != null &&
                    t.Appointment.Date >= today &&
                    t.Id != bookVM.timeId &&
                    !bookVMs.Any(b => b.timeId == t.Id)) // Exclude booked times
                .ToList();

            ViewBag.UserTransactionLogID = UserTransactionLogID;
            ViewBag.AvaiableTimes = timeVMs;

            return View("EditBook", bookVM);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBook(BookVM bookVM, int NewTimeId, int UserTransactionLogID)
        {
            if (ModelState.IsValid)
            {
                TimeVM NewtimeVM = await timeService.GetTimeByID(NewTimeId);
                if (NewtimeVM == null) return RedirectToAction("EditBook", bookVM);

                BookVM bookVMFromDB = await bookService.EditBook(bookVM, NewtimeVM, UserTransactionLogID);
                if (bookVMFromDB == null) return RedirectToAction("EditBook", bookVM);

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
