using Microsoft.AspNetCore.Mvc;
using Your_Ride.Helper;
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
        public async Task<IActionResult> GetAllWallets(string search, int pageNumber = 1, int pageSize = 10)
        {
            List<WalletVM> allWallets = await walletService.GetAllWallets();

            // Filtering based on search query
            if (!string.IsNullOrEmpty(search))
            {
                allWallets = allWallets
                    .Where(w => w.User.UserName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                w.Amount.ToString().Contains(search))
                    .ToList();
            }

            // Pagination logic
            int totalItems = allWallets.Count();
            List<WalletVM> paginatedWallets = allWallets
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var paginatedList = new PaginatedList<WalletVM>(paginatedWallets, totalItems, pageNumber, pageSize);

            return View("GetAllWallets", paginatedList);
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
