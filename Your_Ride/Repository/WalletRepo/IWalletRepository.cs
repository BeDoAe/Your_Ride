using Your_Ride.Models;
using Your_Ride.Repository.Generic;

namespace Your_Ride.Repository.WalletRepo
{
    public interface IWalletRepository : IRepository<Wallet>
    {
        public  Task<List<Wallet>> GetAllWallets();

        public  Task<Wallet> GetlWalletByID(int walletID);

        public Task<Wallet> GetWalletByUserID(string UserID);


    }
}