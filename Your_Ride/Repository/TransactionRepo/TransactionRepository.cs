using Microsoft.EntityFrameworkCore;
using Your_Ride.Models;
using Your_Ride.Repository.Generic;

namespace Your_Ride.Repository.TransactionRepo
{
    public class TransactionRepository: Repository<Transaction>, ITransactionRepository
    {
        private readonly Context context;

        public TransactionRepository ( Context  context):base(context)
        {
            this.context = context;
        }

        public async Task<List<Transaction>> GetAllTransaction()
        {
            List<Transaction> transactions = await context.Transactions.Include(x => x.Wallet).Include(x=>x.Admin).Include(x=>x.User).ToListAsync();
            return transactions;
        }

        public async Task<Transaction> GetTransactionByID(int  id)
        {
            Transaction transaction= await context.Transactions.Include(x=>x.Wallet).Include(x => x.Admin).Include(x => x.User).FirstOrDefaultAsync(x=>x.Id == id);
            return transaction;
        }

        public async Task<int> DeleteTransaction(int id)
        {
            Transaction transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == id);

            if (transaction != null)
            {
                return -1;

            }
            else if (transaction.IsDeleted==true)
            {
                return 0;
            }
            else
            {
                transaction.IsDeleted = false;
               await SaveDB();
                return 1;
            }
        }

        public async Task<ApplicationUser> GetUserByID(string id)
        {
            ApplicationUser applicationUser = await context.Users.FindAsync(id);
            if (applicationUser == null) return null;
            return applicationUser;
        }

        public IQueryable<Transaction> GetAllQuery()
        {
            return context.Transactions
                .Include(x => x.Wallet)
                .Include(x => x.Admin)
                .Include(x => x.User)
                .AsQueryable(); // Ensure it remains IQueryable
        }


    }
}
