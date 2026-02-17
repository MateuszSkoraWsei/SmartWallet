using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartWallet.Models;
using SmartWallet.Data;
using SmartWallet.Models.ViewModels;

namespace SmartWallet.Controllers
{
    public class TransactionController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext context;

        public TransactionController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if (user is null) return NotFound();
            var transactions = context.Transactions
                .Where(t => t.SenderId == user.Id || t.ReceiverId == user.Id)
                .OrderByDescending(t => t.Date)
                .ToList();
            return View(transactions);
        }
        //
        
        [HttpGet]
        public IActionResult Create()
        {

            return View(new CreateTransactionViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTransactionViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var sender = await userManager.GetUserAsync(User);

                var receiver = context.Users.FirstOrDefault(u => u.AccountNumber == vm.AccountNumber);
                if (receiver is null)
                {
                    ModelState.AddModelError(string.Empty, "Nie znaleziono odbiorcy o podanym numerze konta.");
                    return View(vm);
                }
                else if (receiver.AccountNumber == sender.AccountNumber)
                {
                    ModelState.AddModelError(string.Empty, "Nie Można robić przelewów na swój numer konta");
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
                return RedirectToAction("Index", "Dashboard");

            }
            return View(vm);



        }
    }
}
