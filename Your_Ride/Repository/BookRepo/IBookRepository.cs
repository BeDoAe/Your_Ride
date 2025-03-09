using Your_Ride.Models;
using Your_Ride.Repository.Generic;

namespace Your_Ride.Repository.BookRepo
{
    public interface IBookRepository :IRepository<Book>
    {
        public Task<List<Book>> GetAllBooks();
        public Task<Book> GetBookByID(int id);
        public Task<List<Book>> GetAllBooksOfUser(string id);
        public Task<int> DeleteBook(int id);
        public  Task<Book> UpdateBook(Book book);
        public Task<Book> CreateBook(Book book);

        public Task<bool> CheckAlreadyBooked(Book book);
        public Task<List<Seat>> GetAllAvailbleSeats(int id);
        public Task<Seat> GetSeatByID(int id);

    }
}