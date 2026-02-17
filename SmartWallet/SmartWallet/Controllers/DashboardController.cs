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

       

        
        

       
    }
}
