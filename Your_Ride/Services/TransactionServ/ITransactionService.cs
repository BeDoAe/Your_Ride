using Your_Ride.Helper;
using Your_Ride.Models;
using Your_Ride.Services.Generic;
using Your_Ride.ViewModels.TransactionViewModel;

namespace Your_Ride.Services.TransactionServ
{
    public interface ITransactionService : IService<Transaction>
    {
        //public Task<List<TransactionvM>> GetAllTransactions();

    public Task<TransactionvM> GetTransactionByID(int TransactionID);
    public Task<int> DeleteTransaction(int TransactionID);
    public Task<TransactionvM> UpdateTransaction(TransactionvM transactionVM);
    public Task<TransactionvM> CreateTransaction(AllDataTransactionCreateVM allDataTransactionCreateVM);

        //public Task<PaginatedList<TransactionvM>> GetAllTransactions(string search, int pageNumber, int pageSize, string sortColumn, bool ascending);
        public Task<PaginatedList<TransactionvM>> GetAllTransactions(string search, int pageNumber, int pageSize, string sortColumn, bool ascending);



    }
}