using Microsoft.EntityFrameworkCore;
using Your_Ride.Models;
using Your_Ride.Repository.Generic;

namespace Your_Ride.Repository.BookRepo
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly Context context;

        public BookRepository(Context context) : base(context) 
        {
            this.context = context;
        }
        
        public async Task<List<Book>> GetAllBooks()
        {
            List<Book> books = await context.Books
                   .Include(x => x.User)
                   .Include(x => x.Seat)
                   .Include(x => x.Time)
                       .ThenInclude(t => t.Appointment)
                   .Include(x => x.Time)
                       .ThenInclude(t => t.BusGuide)
                   .Include(x => x.Time)
                       .ThenInclude(t => t.LocationsWithPics)
                   .ToListAsync();
            return books;
        }
        public async Task<Book> GetBookByID(int id)
        {
            Book? book = await context.Books
                   .Include(x => x.User)
                   .Include(x => x.Seat)
                   .Include(x => x.Time)
                       .ThenInclude(t => t.Appointment)   
                   .Include(x => x.Time)
                       .ThenInclude(t => t.BusGuide)     
                   .Include(x => x.Time)
                       .ThenInclude(t => t.LocationsWithPics) 
                   .FirstOrDefaultAsync(x => x.Id == id);
            return book;
        }
        public async Task<List<Book>> GetAllBooksOfUser(string id)
        {
            List<Book> books = await context.Books.Include(x => x.User).Include(x => x.Seat).Include(x => x.Time).ThenInclude(x => x.Appointment).Where(x => x.UserID == id).ToListAsync();
            return books;
        }
        public async Task<Book> CreateBook(Book book)
        {
            bool AlreadyBooked = await CheckAlreadyBooked(book);
            if (AlreadyBooked == true) return null;

            await context.AddAsync(book);
            await SaveDB();
            return book;

        }
        public async Task<bool>CheckAlreadyBooked(Book book)
        {
            Book bookFromDB = await context.Books.FirstOrDefaultAsync(x => x.timeId == book.timeId && x.UserID == book.UserID && x.SeatId == book.SeatId);
            if (bookFromDB == null) return false;
            else
            {
                return true;
            }
        }
        public async Task<List<Seat>> GetAllAvailbleSeats(int id)
        {
            List<Seat> seats = (await context.Seats
                      .Where(x => x.IsAvailable == true && x.BusId == id)
                      .ToListAsync())
                      .OrderBy(x => new string(x.SeatLabel.TakeWhile(char.IsLetter).ToArray()))
                      .ThenBy(x => int.Parse(new string(x.SeatLabel.SkipWhile(char.IsLetter).ToArray())))
                      .ToList();

            return seats;

        }
        public async Task<Seat> GetSeatByID(int id)
        {
            Seat seat = await context.Seats.FirstOrDefaultAsync(x => x.Id == id);
            return seat;

        }
        public async Task<int> DeleteBook(int id)
        {
            Book book = await context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (book == null)
            {
                return -1;

            }
            else if (book.IsDeleted == true)
            {
                return 0;
            }
            else
            {
                book.IsDeleted = true;
                await SaveDB();
                return 1;
            }
        }
        public async Task<Book> UpdateBook(Book book)
        {
            context.Books.Update(book);
          await context.SaveChangesAsync();
            return book;

        }

    }
}
