using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartWallet.Data;
using SmartWallet.Models;
using SmartWallet.Models.ViewModels;

namespace SmartWallet.Controllers
{
    public class FinanceController : Controller
    {
        
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FinanceController(UserManager<ApplicationUser> userManager , AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var model = new BalanceViewModel
            {
                TotalBalance = user.Balance,
                AccountNumber = user.AccountNumber,
                MonthlyIncomes = _context.Transactions
                    .Where(t => t.ReceiverId == user.Id && t.Date >= DateTime.Now.AddMonths(-1))
                    .Sum(t => t.Amount),
                MonthlyExpenses = _context.Transactions
                    .Where(t => t.SenderId == user.Id && t.Date >= DateTime.Now.AddMonths(-1))
                    .Sum(t => t.Amount)
            };
            return View(model);
        }
    }
}
