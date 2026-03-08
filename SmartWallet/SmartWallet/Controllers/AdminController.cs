using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Index()
        {
            return View();
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
        public async Task<IActionResult> Transactions()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return NotFound();

            var transactions = _context.Transactions
            .Include(t => t.Sender)
            .Include(t => t.Receiver)
            
            .ToList();
            return View(transactions);
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
            if (user is null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Users");
            }
            else
            {
                // Obsłuż błędy usuwania, np. wyświetl komunikat o błędzie
                return View("Error", "Nie można usunąć użytkownika.");
            }
        }

       
    }
}
