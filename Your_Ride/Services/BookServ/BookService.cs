//using AutoMapper;
//using Microsoft.AspNetCore.Identity;
//using Your_Ride.Models;
//using Your_Ride.Repository.BookRepo;
//using Your_Ride.Services.Generic;
//using Your_Ride.ViewModels.BookViewModel;

//namespace Your_Ride.Services.BookServ
//{
//    public class BookService :Service<Book> , IBookService
//    {
//        private readonly IMapper automapper;
//        private readonly IBookRepository bookRepository;
//        private readonly UserManager<ApplicationUser> userManager;

//        public BookService(IMapper automapper , IBookRepository bookRepository , UserManager<ApplicationUser> userManager)
//        {
//            this.automapper = automapper;
//            this.bookRepository = bookRepository;
//            this.userManager = userManager;
//        }

//        public async Task<List<BookVM>> GetAllBooks()
//        {
//            List<Book> books = await bookRepository.GetAllBooks();

//            List<BookVM> bookVMs= automapper.Map<List<BookVM>>(books);
//            return bookVMs;
//        }
//        public async Task<BookVM> GetBookByID(int id)
//        {
//            Book book = await bookRepository.GetBookByID(id);
//            if (book == null) return null;

//            BookVM bookVM = automapper.Map<BookVM>(book);
//            return bookVM;
//        }
//        public async Task<List<BookVM>> GetAllBooksOfUser(string id)
//        {
//            //ApplicationUser applicationUser = await userManager.FindByIdAsync(id);
//            //if (applicationUser == null) return null;

//            List<Book> books = await bookRepository.GetAllBooksOfUser(id);

//            List<BookVM> bookVMs = automapper.Map<List<BookVM>>(books);

//            return bookVMs;
//        }


//        public async Task<BookVM> CreateBook(BookVM bookVM)
//        {
//           Book book= automapper.Map<Book>(bookVM);
//            await bookRepository.AddAsync(book);

//            return automapper.Map<BookVM>(book);
//        }

//        public async Task<BookVM> EditBook(BookVM bookVM)
//        {
//            Book book = await bookRepository.GetBookByID(bookVM.Id);
//            if (book == null) return null;

//            //automapper.Map(bookVM,book);
//            //await bookRepository.UpdateAsync(book);
//            book.MaxAmount = bookVM.MaxAmount;
//            await bookRepository.UpdateBook(book);

//            return automapper.Map<BookVM>(book);
//        }

//        public async Task<int> DeleteBook(int id)
//        {
//            Book book = await bookRepository.GetBookByID(id);

//           int result= await bookRepository.DeleteBook(id);

//            return result;
//        }
//    }
//}
