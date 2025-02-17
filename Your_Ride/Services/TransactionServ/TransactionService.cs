using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Your_Ride.Helper;
using Your_Ride.Models;
using Your_Ride.Repository.TransactionRepo;
using Your_Ride.Repository.WalletRepo;
using Your_Ride.Services.Generic;
using Your_Ride.ViewModels.TransactionViewModel;
using Your_Ride.ViewModels.WalletViewModel;

namespace Your_Ride.Services.TransactionServ
{
    public class TransactionService : Service<Transaction>, ITransactionService
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IWalletRepository walletRepository;
        private readonly IMapper automapper;

        public TransactionService(ITransactionRepository transactionRepository , IWalletRepository walletRepository, IMapper automapper)
        {
            this.transactionRepository = transactionRepository;
            this.walletRepository = walletRepository;
            this.automapper = automapper;
        }
        public async Task<List<TransactionvM>> GetAllTransactions()
        {
            List<Transaction> transactions = await transactionRepository.GetAllAsync();
            
            List<TransactionvM > transactionvMs = automapper.Map<List<TransactionvM>>(transactions);
            return transactionvMs;
        }
        public async Task<TransactionvM> GetTransactionByID(int TransactionID)
        {
          
            Transaction transaction = await transactionRepository.GetTransactionByID(TransactionID);
            if (transaction == null) return null;
            TransactionvM transactionvM = automapper.Map<TransactionvM>(transaction);
            return transactionvM;
        }

        public async Task<int> DeleteTransaction(int TransactionID)
        {
           int Result = await transactionRepository.DeleteTransaction(TransactionID);
                        return Result;
        }

        public async Task<TransactionvM> UpdateTransaction (TransactionvM transactionVM)
        {

            Transaction transactionFromDB = await transactionRepository.GetByIdAsync(transactionVM.Id);
            if (transactionFromDB == null) return null;
            Wallet wallet = await walletRepository.GetByIdAsync(transactionVM.WalletID);
            if(wallet.Amount<0)
            {
                return transactionVM;
            }


            if (transactionVM.OptionAmount > 0 && transactionVM.OptionType.HasValue)
            {
                if (transactionVM.OptionType == TransactionType.Decrease && transactionVM.OptionAmount <= transactionVM.Amount)
                {
                    wallet.Amount -= transactionVM.OptionAmount.Value;
                    transactionFromDB.Amount -= transactionVM.OptionAmount.Value;

                    /////////////////
                    transactionFromDB.EditedTransactionDate = DateTime.Now;

                    await transactionRepository.UpdateAsync(transactionFromDB);

                    return automapper.Map<TransactionvM>(transactionFromDB);
                }
                else if (transactionVM.OptionType == TransactionType.Increase)
                {
                    //wallet.Amount -= transactionFromDB.Amount;
                    wallet.Amount += transactionVM.OptionAmount.Value;
                    transactionFromDB.Amount += transactionVM.OptionAmount.Value;

                    transactionFromDB.EditedTransactionDate = DateTime.Now;

                    await transactionRepository.UpdateAsync(transactionFromDB);

                    return automapper.Map<TransactionvM>(transactionFromDB);
                }
                else
                {
                    return null;
                }
            }

            wallet.Amount -= transactionFromDB.Amount;
            wallet.Amount += transactionVM.Amount;

             transactionFromDB.Amount = transactionVM.Amount;
            //transactionFromDB.TransactionDate=DateTime.Now;

            DateTime currentDateTime = DateTime.Now;
            transactionFromDB.EditedTransactionDate = currentDateTime;

            await transactionRepository.UpdateAsync(transactionFromDB);

          return  automapper.Map<TransactionvM>(transactionFromDB);
            
            
        }

        public async Task<TransactionvM> CreateTransaction(AllDataTransactionCreateVM allDataTransactionCreateVM)
        {

            //ApplicationUser userFromDB = await transactionRepository.GetUserByID(allDataTransactionCreateVM.UserId);

            //ApplicationUser AdminFromDB= await transactionRepository.GetUserByID(allDataTransactionCreateVM.AdminId);

            //Wallet walletFromDB = await walletRepository.GetWalletByUserID(allDataTransactionCreateVM.User.Id);
            //if (userFromDB == null || AdminFromDB == null || walletFromDB== null)
            //    return null;

            //Transaction transaction = automapper.Map<Transaction>(allDataTransactionCreateVM);

            //allDataTransactionCreateVM.TransactionDate = DateTime.Now;
            //allDataTransactionCreateVM.User.Id = transaction.UserId;
            //allDataTransactionCreateVM.Admin.Id = transaction.AdminId;
            //walletFromDB=transaction.Wallet;

            Transaction transaction = automapper.Map<Transaction>(allDataTransactionCreateVM);

            await transactionRepository.AddAsync(transaction);

            TransactionvM transactionvM = automapper.Map<TransactionvM>(transaction);
            return transactionvM;


        }
        public async Task<PaginatedList<TransactionvM>> GetAllTransactions(string search, int pageNumber, int pageSize, string sortColumn, bool ascending)
        {
            IQueryable<Transaction> query = transactionRepository.GetAllQuery();

            // 🔍 Search functionality
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t =>
                    t.User.FirstName.Contains(search) ||
                    t.User.LastName.Contains(search) ||
                    t.Admin.FirstName.Contains(search) ||
                    t.Admin.LastName.Contains(search) ||
                    t.Amount.ToString().Contains(search)
                );
            }

            // 🔄 Sorting
            query = sortColumn switch
            {
                "Id" => ascending ? query.OrderBy(t => t.Id) : query.OrderByDescending(t => t.Id),
                "Amount" => ascending ? query.OrderBy(t => t.Amount) : query.OrderByDescending(t => t.Amount),
                "Date" => ascending ? query.OrderBy(t => t.TransactionDate) : query.OrderByDescending(t => t.TransactionDate),
                "User" => ascending ? query.OrderBy(t => t.User.FirstName).ThenBy(t => t.User.LastName)
                                    : query.OrderByDescending(t => t.User.FirstName).ThenByDescending(t => t.User.LastName),
                "Admin" => ascending ? query.OrderBy(t => t.Admin.FirstName).ThenBy(t => t.Admin.LastName)
                                     : query.OrderByDescending(t => t.Admin.FirstName).ThenByDescending(t => t.Admin.LastName),
                _ => query.OrderByDescending(t => t.TransactionDate) // Default sorting by date descending
            };

            // 📄 Pagination
            var totalItems = await query.CountAsync();
            var transactions = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TransactionvM
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    TransactionDate = t.TransactionDate,
                    User = t.User,
                    Admin = t.Admin,
                    Wallet = t.Wallet
                })
                .ToListAsync();

            return new PaginatedList<TransactionvM>(transactions, totalItems, pageNumber, pageSize);
        }

    }
}
