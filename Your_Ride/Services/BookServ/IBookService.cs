using Your_Ride.Models;
using Your_Ride.Services.Generic;
using Your_Ride.ViewModels.BookViewModel;

namespace Your_Ride.Services.BookServ
{
    public interface IBookService : IService<Book>
    {
        public Task<List<BookVM>> GetAllBooks();
        public Task<BookVM> GetBookByID(int id);
        public Task<List<BookVM>> GetAllBooksOfUser(string id);
        public Task<BookVM> CreateBook(BookVM bookVM);
        public Task<BookVM> EditBook(BookVM bookVM);
        public Task<int> DeleteBook(int id);
            
    }
}