using Your_Ride.Models;
using Your_Ride.Services.Generic;
using Your_Ride.ViewModels.WalletViewModel;

namespace Your_Ride.Services.WalletServ
{
    public interface IWalletService : IService<Wallet>
    {
        public Task<List<WalletVM>> GetAllWallets();

        public Task<WalletVM> GetWalletByID(int walletID);
        public Task<WalletVM> UpdateWallet(int WalletID, double amount);

        public  Task<WalletVM> GetWalletByUserID(string UserID);

        public  Task<Wallet> getWalletByUserID(string UserID);



    }
}