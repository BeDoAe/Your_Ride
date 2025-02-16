using Microsoft.AspNetCore.Mvc;
using Your_Ride.Services.WalletServ;
using Your_Ride.ViewModels.WalletViewModel;

namespace Your_Ride.Controllers
{
    public class WalletController : Controller
    {
        private readonly IWalletService walletService;

        public WalletController(IWalletService walletService)
        {
            this.walletService = walletService;
        }
        //  /Wallet/GetAllWallets
        [HttpGet]
        public async Task<IActionResult> GetAllWallets()
        {
            List<WalletVM> walletVMs= await walletService.GetAllWallets();
            return View("GetAllWallets", walletVMs);
        }

        [HttpGet]
        public async Task<IActionResult> GetWalletByID(int WalletID)
        {
           WalletVM walletVM = await walletService.GetWalletByID( WalletID);
            return View("GetWalletByID", walletVM);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateWallet(int WalletID)
        {
            WalletVM walletVM = await walletService.GetWalletByID(WalletID);
            if (walletVM == null) return NotFound();
            return View("UpdateWallet",walletVM);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateWallet(int WalletID , double amount)
        {
            if (!ModelState.IsValid)
            {
                // Reload the wallet so the view has user info
                WalletVM walletVM = await walletService.GetWalletByID(WalletID);
                return View(walletVM);
            }

            WalletVM Model = await walletService.UpdateWallet(WalletID, amount);
                if (Model == null) return NotFound();

                return RedirectToAction("GetWalletByID", new { WalletID = WalletID });
        }
    }
}
