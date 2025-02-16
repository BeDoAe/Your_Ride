using AutoMapper;
using Your_Ride.Models;
using Your_Ride.Repository.WalletRepo;
using Your_Ride.Services.Generic;
using Your_Ride.ViewModels.WalletViewModel;

namespace Your_Ride.Services.WalletServ
{
    public class WalletService : Service<Wallet> , IWalletService
    {
        private readonly IMapper autoMapper;
        private readonly IWalletRepository walletRepository;

        public WalletService(IMapper autoMapper , IWalletRepository walletRepository ) 
        {
            this.autoMapper = autoMapper;
            this.walletRepository = walletRepository;
        }

        public async Task<List<WalletVM>> GetAllWallets()
        {
            List<Wallet> wallets = await walletRepository.GetAllWallets();

            List<WalletVM> walletsVM = autoMapper.Map<List<WalletVM>>(wallets);

            return walletsVM;
        }


        public async Task<WalletVM> GetWalletByID(int walletID)
        {
            Wallet wallet = await walletRepository.GetlWalletByID(walletID);
            if (wallet == null) return null;

            WalletVM walletVM = autoMapper.Map<WalletVM>(wallet);

            return walletVM;
        }

        public async Task<WalletVM> UpdateWallet(int WalletID , double amount)
        {
            Wallet walletFromDB = await walletRepository.GetlWalletByID(WalletID);

            if (walletFromDB == null) return null;

            walletFromDB.Amount=amount;
            //autoMapper.Map(walletVM, walletFromDB);
          await  walletRepository.UpdateAsync(walletFromDB);

            return autoMapper.Map<WalletVM>(walletFromDB);
        }

        public async Task<WalletVM> GetWalletByUserID(string UserID)
        {
            Wallet wallet = await walletRepository.GetWalletByUserID(UserID);

            if (wallet == null) return null;

            return autoMapper.Map<WalletVM>(wallet);
        }

        public async Task<Wallet> getWalletByUserID(string UserID)
        {
            Wallet wallet = await walletRepository.GetWalletByUserID(UserID);

            if (wallet == null) return null;

            return wallet;
        }

    }
}
