using Your_Ride.Models;
using Your_Ride.Services.Generic;
using Your_Ride.ViewModels.BookViewModel;
using Your_Ride.ViewModels.TimeViewModel;

namespace Your_Ride.Services.BookServ
{
    public interface IBookService : IService<Book>
    {
        public Task<List<BookVM>> GetAllBooks();
        public Task<BookVM> GetBookByID(int id);
        public Task<List<BookVM>> GetAllBooksOfUser(string id);
        public Task<BookUserTransactionVM> CreateBook(BookVM bookVM);
        public Task<BookVM> EditBook(BookVM bookVM, TimeVM newTimeVM, int UserTransactionLogID);
        public Task<int> DeleteBook(int id);
            
    }
}