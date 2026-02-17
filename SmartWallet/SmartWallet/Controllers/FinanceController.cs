using Microsoft.AspNetCore.Mvc;
using SmartWallet.Data;

namespace SmartWallet.Controllers
{
    public class FinanceController : Controller
    {
        private readonly ILogger<FinanceController> _logger;   
        private readonly AppDbContext _context;
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
