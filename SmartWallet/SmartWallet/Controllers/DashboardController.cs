using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartWallet.Data;
using SmartWallet.Models;
using System.Net.Mail;

namespace SmartWallet.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext context;

        public DashboardController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }
        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if (user is null) return NotFound();

            var recentTransaztion = context.Transactions
                .Where(t => t.SenderId == user.Id || t.ReceiverId == user.Id)
                .OrderByDescending(t => t.Date)
                .Take(5)
                .ToList();

            var viewModel = new DashboardViewModel
            {
                FullName = user.FullName,
                Balance = user.Balance,
                AccountNumber = user.AccountNumber,
                RecentTransactions = recentTransaztion

            };

            return View(viewModel);
        }

        // get : Dashboard/Transactions

        public async Task<IActionResult> Transactions()
        {
            var user = await userManager.GetUserAsync(User);
            if (user is null) return NotFound();
            var transactions = context.Transactions
                .Where(t => t.SenderId == user.Id || t.ReceiverId == user.Id)
                .OrderByDescending(t => t.Date)
                .ToList();
            return View(transactions);
        }
        [HttpGet]
        public IActionResult CreateTransaction()
        {
            
            return View(new CreateTransactionViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTransaction(CreateTransactionViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var sender = await userManager.GetUserAsync(User);

                var receiver = context.Users.FirstOrDefault(u => u.AccountNumber == vm.AccountNumber);
                if(receiver is null)
                {
                    ModelState.AddModelError(string.Empty, "Nie znaleziono odbiorcy o podanym numerze konta.");
                    return View(vm);
                }
                sender.Balance -= vm.Amount;
                receiver.Balance += vm.Amount;

                var transaction = new Transactions
                {
                    AccountNumber = vm.AccountNumber,
                    SenderId = sender.Id,
                    ReceiverId = receiver.Id,
                    Amount = vm.Amount,
                    TransactionName = vm.TransactionName,
                    Description = vm.Description,
                    Date = DateTime.Now,
                    CategoryID = 2  
                };
                
                context.Transactions.Add(transaction);
                await context.SaveChangesAsync();
                return RedirectToAction("Index","Dashboard");

            }
            return View(vm);    



        }
    }
}
