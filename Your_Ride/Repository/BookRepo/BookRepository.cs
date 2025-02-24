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
            List<Book> books = await context.Books.Include(x=>x.User).Include(x=>x.Appointment).ToListAsync();
            return books;
        }
        public async Task<Book> GetBookByID(int id)
        {
            Book book = await context.Books.Include(x => x.User).Include(x => x.Appointment).FirstOrDefaultAsync(x=>x.Id==id);
            return book;
        }
        public async Task<List<Book>> GetAllBooksOfUser(string id)
        {
            List<Book> books = await context.Books.Include(x => x.User).Include(x => x.Appointment).Where(x => x.UserID == id).ToListAsync();
            return books;
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
