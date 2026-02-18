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
    }
}
