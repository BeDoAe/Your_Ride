using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Your_Ride.Models;
using Your_Ride.Repository.BookRepo;
using Your_Ride.Services.Generic;
using Your_Ride.Services.TimeServ;
using Your_Ride.Services.UserTransactionLogServ;
using Your_Ride.ViewModels.BookViewModel;
using Your_Ride.ViewModels.TimeViewModel;
using Your_Ride.ViewModels.UserTransactionLogViewModel;

namespace Your_Ride.Services.BookServ
{
    public class BookService : Service<Book>, IBookService
    {
        private readonly IMapper automapper;
        private readonly IBookRepository bookRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITimeService timeService;
        private readonly IUserTransactionLogService userTransactionLogService;

        public BookService(IMapper automapper, IBookRepository bookRepository, UserManager<ApplicationUser> userManager , ITimeService timeService , IUserTransactionLogService userTransactionLogService)
        {
            this.automapper = automapper;
            this.bookRepository = bookRepository;
            this.userManager = userManager;
            this.timeService = timeService;
            this.userTransactionLogService = userTransactionLogService;
        }


        public async Task<List<BookVM>> GetAllBooks()
        {
            List<Book> books = await bookRepository.GetAllBooks();

            List<BookVM> bookVMs = automapper.Map<List<BookVM>>(books);
            return bookVMs;
        }
        public async Task<BookVM> GetBookByID(int id)
        {
            Book book = await bookRepository.GetBookByID(id);
            if (book == null) return null;

            BookVM bookVM = automapper.Map<BookVM>(book);
            return bookVM;
        }
        public async Task<List<BookVM>> GetAllBooksOfUser(string id)
        {
            ApplicationUser applicationUser = await userManager.FindByIdAsync(id);
            if (applicationUser == null) return new List<BookVM>(); // Return an empty list if no user is found

            List<Book> books = await bookRepository.GetAllBooksOfUser(id);
            if (books == null || books.Count == 0)
                return new List<BookVM>(); // Return an empty list if no books are found

            List<BookVM> bookVMs = automapper.Map<List<BookVM>>(books);
            return bookVMs;
        }



        public async Task<BookUserTransactionVM> CreateBook(BookVM bookVM)
        {
            ApplicationUser applicationUser = await userManager.FindByIdAsync(bookVM.UserID);
            if (applicationUser == null) return null;

            TimeVM timeVM = await timeService.GetTimeByID(bookVM.timeId);
            if (timeVM == null) return null;

            List<Seat> seats = await bookRepository.GetAllAvailbleSeats(timeVM.BusID);
            if (seats == null) return null;

            bookVM.SeatId = seats[0].Id;
            seats[0].IsAvailable = false;
            Book book = automapper.Map<Book>(bookVM);


            UserTransactionLogVM userTransactionLogVM = new UserTransactionLogVM
            {
                UserId = bookVM.UserID,
                AppointmentId = timeVM.AppointmentId,
                TimeId = timeVM.Id,
                WithdrawalAmount = timeVM.Fee
            };

            UserTransactionLogVM newUserTransactionLogVM = await userTransactionLogService.CreateUserTransactionLog(userTransactionLogVM);
            if (newUserTransactionLogVM == null) return null;


            Book newbook = await bookRepository.CreateBook(book);
            if (newbook == null) return null;


          BookVM returnBookVM =   automapper.Map<BookVM>(newbook);

            BookUserTransactionVM bookUserTransactionVM = new BookUserTransactionVM();
            bookUserTransactionVM.bookVM = returnBookVM;
            bookUserTransactionVM.userTransactionLogID = newUserTransactionLogVM.Id;

            return bookUserTransactionVM;

        }

        public async Task<BookVM> EditBook(BookVM bookVM , TimeVM newTimeVM , int UserTransactionLogID)
        {
            Book bookFromDB = await bookRepository.GetBookByID(bookVM.Id);
            if (bookFromDB == null) return null;

            ApplicationUser applicationUser = await userManager.FindByIdAsync(bookVM.UserID);
            if (applicationUser == null) return null;

            TimeVM timeVM = await timeService.GetTimeByID(bookVM.timeId);
            if (timeVM == null) return null;

            TimeVM newTimeVMFromDB = await timeService.GetTimeByID(newTimeVM.Id);
            if (newTimeVMFromDB == null) return null;

            List<Seat> seats = await bookRepository.GetAllAvailbleSeats(newTimeVM.BusID);
            if (seats == null) return null;

            Seat Currentseat = await bookRepository.GetSeatByID(bookVM.SeatId);
            if (Currentseat == null) return null;

            if (timeVM.Category == 0)
            {
                if (timeVM.DueDateArrivalSubmission == null || timeVM.DueDateArrivalSubmission <=DateTime.Now)
                {
                    bookFromDB.timeId = newTimeVMFromDB.Id;
                    bookFromDB.SeatId = seats[0].Id;
                    seats[0].IsAvailable = false;
                    Currentseat.IsAvailable = true;

                    UserTransactionLogVM userTransactionLogVMFromDB = await userTransactionLogService.GetUserTransactioLogById(UserTransactionLogID);
                    if (userTransactionLogVMFromDB == null) return null;

                    //userTransactionLogVMFromDB.AppointmentId = newTimeVM.AppointmentId;
                    //userTransactionLogVMFromDB.TimeId = newTimeVM.Id;
                    //userTransactionLogVMFromDB.UserId = bookVM.UserID;
                    //userTransactionLogVMFromDB.WithdrawalAmount = newTimeVM.Fee;
                  
                    UserTransactionLogVM userTransactionVM = automapper.Map<UserTransactionLogVM>(userTransactionLogVMFromDB);

                    userTransactionVM.AppointmentId = newTimeVM.AppointmentId;
                    userTransactionVM.TimeId = newTimeVM.Id;
                    userTransactionVM.UserId = bookVM.UserID;
                    userTransactionVM.WithdrawalAmount = newTimeVM.Fee;

                    UserTransactionLogVM userTransactionLogVMAfterUpdate = await userTransactionLogService.EditUserTransactionLog(userTransactionVM);
                    if (userTransactionLogVMAfterUpdate == null) return null;

                    await bookRepository.UpdateBook(bookFromDB);
                    await bookRepository.SaveDB();
                    return automapper.Map<BookVM>(bookFromDB);
                }else
                {
                    return null;
                }
            }else
            {
                if (timeVM.DueDateDepartureSubmission == null && timeVM.DueDateDepartureSubmission <= DateTime.Now)
                {

                    bookFromDB.timeId = newTimeVMFromDB.Id;
                    bookFromDB.SeatId = seats[0].Id;
                    seats[0].IsAvailable = false;
                    Currentseat.IsAvailable = true;

                    UserTransactionLogVM userTransactionLogVMFromDB = await userTransactionLogService.GetUserTransactioLogById(UserTransactionLogID);
                    if (userTransactionLogVMFromDB == null) return null;

                    userTransactionLogVMFromDB.AppointmentId = newTimeVM.AppointmentId;
                    userTransactionLogVMFromDB.TimeId = newTimeVM.Id;
                    userTransactionLogVMFromDB.UserId = bookVM.UserID;
                    userTransactionLogVMFromDB.WithdrawalAmount = newTimeVM.Fee;

                    UserTransactionLogVM userTransactionLogVMAfterUpdate = await userTransactionLogService.EditUserTransactionLog(userTransactionLogVMFromDB);
                    if (userTransactionLogVMAfterUpdate == null) return null;

                    await bookRepository.UpdateBook(bookFromDB);
                    await bookRepository.SaveDB();
                    return automapper.Map<BookVM>(bookFromDB);
                }
                else
                {
                    return null;
                }
            }

        }

        public async Task<int> DeleteBook(int id)
        {
            Book book = await bookRepository.GetBookByID(id);

            int result = await bookRepository.DeleteBook(id);

            return result;
        }
    }
}
