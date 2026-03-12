using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Identity.Client;
using SmartWallet.Data;
using SmartWallet.Models;

namespace SmartWallet.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        public AdminController(ILogger<AdminController> logger, UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user is null) return NotFound();

            var viewModel = new Models.ViewModels.AdminIndexViewModel
            {
                TotalUsers = _context.Users.Count(),
                TotalTransactions = _context.Transactions.Count(),
                TotalBalance = _context.Users.Sum(u => u.Balance)
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return NotFound();   

            var allUsers = _context.Users.ToList();
            return View(allUsers);
   
        }

        [HttpGet]
        public async Task<IActionResult> Transactions(TransactionStatus? status)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return NotFound();

            var query = _context.Transactions
            .Include(t => t.Sender)
            .Include(t => t.Receiver)
            .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value); 
            }
            var transactions = await query.OrderByDescending(t => t.Date).ToListAsync();

            ViewBag.CurrentFilter = status;
            
            return View(transactions);
        }
        [HttpGet]
        public async Task<IActionResult> TransactionDetails(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Sender)
                .Include(t => t.Receiver)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.TransactionID == id);
            if (transaction is null) return NotFound();
            return View(transaction);
        }
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> TransactionStatusSetToComplete(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Sender)
                .Include(t => t.Receiver)
                .FirstOrDefaultAsync(t => t.TransactionID == id);
            if (transaction is null) return NotFound();
            
            if(transaction.Status != TransactionStatus.Completed)
            {
                transaction.Status = TransactionStatus.Completed;
                transaction.Sender.Balance -= transaction.Amount;
                transaction.Receiver.Balance += transaction.Amount;

                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Transactions));
        }
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> TransactionStatusSetToCancel(int id)
        {  
            var transaction = await _context.Transactions
                .Include(t => t.Sender)
                .Include(t => t.Receiver)
                .FirstOrDefaultAsync(t => t.TransactionID == id);

            if (transaction is null) return NotFound();

            if(transaction.Status != TransactionStatus.Cancelled)
            {
                transaction.Status = TransactionStatus.Cancelled;
                transaction.Sender.Balance += transaction.Amount;
                transaction.Receiver.Balance -= transaction.Amount;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Transactions));
        }


        [HttpGet]
        public async Task<IActionResult> UserDetails(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();

            return View(user);
        }
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var currUserID = _userManager.GetUserId(User);
            if (user is null) return NotFound();

            if( id == currUserID)
            {
                TempData["Error"] =  "Nie można usunąć własnego konta.";
                return RedirectToAction("Users");
            }
            var userTransactions = _context.Transactions
                .Where(t => t.SenderId == id ||  t.ReceiverId == id);
            _context.Transactions.RemoveRange(userTransactions);

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Użytkownik został pomyślnie usunięty.";
                return RedirectToAction("Users");
            }
            else
            {
                
                return View("Error", "Nie można usunąć użytkownika.");
            }
        }

       
    }
}
