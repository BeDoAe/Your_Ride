using Your_Ride.Models;
using Your_Ride.Repository.Generic;

namespace Your_Ride.Repository.TransactionRepo
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        public Task<List<Transaction>> GetAllTransaction();
        public Task<Transaction> GetTransactionByID(int id);
        public Task<int> DeleteTransaction(int id);

        public Task<ApplicationUser> GetUserByID(string id);

        public IQueryable<Transaction> GetAllQuery();



    }
}