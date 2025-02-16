using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Your_Ride.Models;
using Your_Ride.Repository.WalletRepo;
using Your_Ride.Services.TransactionServ;
using Your_Ride.Services.WalletServ;
using Your_Ride.ViewModels.Account;
using Your_Ride.ViewModels.TransactionViewModel;
using Your_Ride.ViewModels.WalletViewModel;


namespace Your_Ride.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TransactionController : Controller
    {
        private readonly ITransactionService transactionService;
        private readonly IWalletService walletService;
        private readonly UserManager<ApplicationUser> userManager;

        public TransactionController(ITransactionService transactionService ,IWalletService walletService , UserManager<ApplicationUser> userManager)
        {
            this.transactionService = transactionService;
            this.walletService = walletService;
            this.userManager = userManager;
        }
        // GET:  / Transaction/GetAllTransactions
        //public async Task<IActionResult> GetAllTransactions()
        //{
        //    var transactions = await transactionService.GetAllTransactions();
        //    return View("GetAllTransactions", transactions);
        //}

        // GET:  /Transaction/GetAllTransactions
        public async Task<IActionResult> GetAllTransactions(string search = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "Date", bool ascending = false)
        {
            var transactions = await transactionService.GetAllTransactions(search, pageNumber, pageSize, sortColumn, ascending);
            return View("GetAllTransactions", transactions);
        }


        // GET:      /Transaction/GetTransactionDetails/{id}
        public async Task<IActionResult> GetTransactionDetails(int id)
        {
            var transaction = await transactionService.GetTransactionByID(id);
            if (transaction == null) return NotFound();
            return View("GetTransactionDetails", transaction);
        }

        // GET:            /Transaction/CreateTransaction
        public IActionResult CreateTransaction()
        {
            //var users = userManager.Users.ToList();
            //return View("CreateTransaction",users);

            var users = userManager.Users.ToList();
            ViewBag.Users = users;
            return View("CreateTransaction");
        }

        // POST:        /Transaction/CreateTransaction
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTransaction(CreateTransactionvM createtransactionVM)
        {
            if (!ModelState.IsValid)
            {
                var users = userManager.Users.ToList();
                ViewBag.Users = users;
                return View(createtransactionVM);
            }
            AllDataTransactionCreateVM allDataTransactionCreateVM = new AllDataTransactionCreateVM();

                var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var adminUser = await userManager.FindByIdAsync(adminId);
            if (adminUser == null) return NotFound("Admin not found");

            var user = await userManager.FindByIdAsync(createtransactionVM.UserId);
            if (user == null) return NotFound("User not found");

            var wallet = await walletService.getWalletByUserID(user.Id);
            if (wallet == null) return NotFound("Wallet not found");

            allDataTransactionCreateVM.AdminId = adminId;
            allDataTransactionCreateVM.Admin = adminUser;
            allDataTransactionCreateVM.User = user;
            allDataTransactionCreateVM.Wallet = wallet;
            wallet.Amount += createtransactionVM.Amount;
            allDataTransactionCreateVM.WalletId = wallet.Id;
            allDataTransactionCreateVM.Amount = createtransactionVM.Amount;
            allDataTransactionCreateVM.TransactionDate = createtransactionVM.TransactionDate;

            var createdTransaction = await transactionService.CreateTransaction(allDataTransactionCreateVM);
            if (createdTransaction == null)
            {
                ModelState.AddModelError("", "Error creating transaction.");
                return View(createtransactionVM);
            }

            return RedirectToAction(nameof(GetAllTransactions));
        }

        // GET:        /Transaction/EditTransaction/{id}
        public async Task<IActionResult> EditTransaction(int id)
        {
            var transaction = await transactionService.GetTransactionByID(id);
            if (transaction == null) return NotFound();

            transaction.UserId = transaction.User.Id;
            transaction.AdminId = transaction.Admin.Id;
            transaction.WalletID = transaction.Wallet.Id;
            return View("EditTransaction", transaction);
        }

        // POST:        /Transaction/EditTransaction/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTransaction(TransactionvM transactionVM)
        {
            if (!ModelState.IsValid) return View(transactionVM);

            var updatedTransaction = await transactionService.UpdateTransaction(transactionVM);
            if (updatedTransaction == null)
            {
                ModelState.AddModelError("", "Transaction update failed.");
                return View("EditTransaction", transactionVM);
            }

            return RedirectToAction(nameof(GetAllTransactions));
        }

        // GET:                      /Transaction/DeleteTransaction/{id}
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await transactionService.GetTransactionByID(id);
            if (transaction == null) return NotFound();

            return View("DeleteTransaction", transaction);
        }

        // POST: Transaction/DeleteConfirmed/{id}
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await transactionService.DeleteTransaction(id);
            if (result ==-1)
            {
                ModelState.AddModelError("", "Error Deleting cause No Transaction Found.");
                return View("DeleteTransaction", await transactionService.GetTransactionByID(id));
            }else  if (result == 0)
            {
                return Content("Already been deleted");
            }else
            {
                return RedirectToAction(nameof(GetAllTransactions));
            }

           
        }
    }
}