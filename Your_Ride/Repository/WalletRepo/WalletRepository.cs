using Microsoft.EntityFrameworkCore;
using Your_Ride.Models;
using Your_Ride.Repository.Generic;
using Your_Ride.ViewModels.WalletViewModel;

namespace Your_Ride.Repository.WalletRepo
{
    public class WalletRepository : Repository<Wallet>, IWalletRepository
    {
        private readonly Context context;

        public WalletRepository( Context context ):base( context )
        {
            this.context = context;
        }

        public async Task<List<Wallet>> GetAllWallets()
        {
            List<Wallet> wallets = await context.Wallets.Include(x=>x.User).ToListAsync();
            return wallets;
        }

        public async Task<Wallet> GetlWalletByID(int walletID)
        {
            Wallet wallet = await context.Wallets.Include(x => x.User).FirstOrDefaultAsync(x=>x.Id==walletID);
            return wallet;
        }

        public async Task<Wallet> GetWalletByUserID (string UserID)
        {
            Wallet wallet = await context.Wallets.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == UserID);
            return wallet;
        }
    }
}
