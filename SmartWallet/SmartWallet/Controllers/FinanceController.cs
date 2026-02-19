using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public async Task<IActionResult> GetCategoryCharts()
        {
            var user = await _userManager.GetUserAsync(User);
            var categoryData = _context.Transactions
                .Where(t => t.SenderId == user.Id && t.Date >= DateTime.Now.AddMonths(-1))
                .Include(t => t.Category)
                .GroupBy(t => t.Category.CategoryName)
                .Select(g => new 
                {
                    CategoryName = g.Key,
                    TotalAmount = g.Sum(t => t.Amount),
                    color = g.FirstOrDefault().Category.CategoryColor,
                    icon = g.FirstOrDefault().Category.CategoryIcon
                })
                .ToList();
            return Json(categoryData);
        }
        [HttpGet]
        public async Task<IActionResult> GetBalanceHistory()
        {
            var user = await _userManager.GetUserAsync(User);
            var transactions = _context.Transactions
                .Where(t => t.SenderId == user.Id || t.ReceiverId == user.Id)
                .OrderBy(t => t.Date)
                .ToList();

            var history = new List<object>();
            decimal currentBalance = 0; // Możesz zacząć od 0 lub od salda początkowego

            foreach (var t in transactions)
            {
                if (t.ReceiverId == user.Id) currentBalance += t.Amount;
                else currentBalance -= t.Amount;

                history.Add(new { date = t.Date.ToString("dd.MM"), balance = currentBalance });
            }

            return Json(history);

        }
    }
}
