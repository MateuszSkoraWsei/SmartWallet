using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Identity.Client;
using SmartWallet.Data;
using SmartWallet.Models;
using SmartWallet.Models.ViewModels;
using SmartWallet.Services;
using System.Threading.Tasks;

namespace SmartWallet.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private int _minAge;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            int userCount = _context.Users.Count();
            var user = new ApplicationUser
            {
                UserName = model.Email,
                AccountNumber = GenerateAccountNumber(userCount + 1),
                Balance = 1000,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname,
                FullName = $"{model.Name} {model.Surname}",
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                NormalizedUserName = GenerateUserName(model.Name, model.Surname, userCount + 1)

            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Account",
                    new { userId = user.Id, token = token }, Request.Scheme);

                await _emailService.SendEmailAsync(user.Email, "Potwierdź swój adres e-mail",
                    $"Aby potwierdzić konto w SmartWallet, <a href='{confirmationLink}'>kliknij tutaj</a>.");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }
        public string GenerateUserName(string name, string surname, int UserNumber)
        {
            return $"{name.Substring(0, 3).ToUpper()}{surname.Substring(0, 3).ToUpper()}{UserNumber.ToString("d4")}";
        }
        private string GenerateAccountNumber(int UserNumber)
        {
            string BankCode = "45";
            string data = DateTime.Now.ToString("yyyyMMdd");
            string IdPart = UserNumber.ToString("D4");

            return $"{BankCode}{data}{IdPart}";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if(User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                return RedirectToAction("Index", "Home");
            }

            // DIAGNOSTYKA:
            if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Konto nie jest zatwierdzone (prawdopodobnie brak potwierdzenia e-mail).");
            }
            else if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Konto jest zablokowane.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Błędny login lub hasło.");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous] 
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Nie znaleziono użytkownika o ID '{userId}'.");
            }

            
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }
            else
            {
                return BadRequest("Błąd podczas weryfikacji e-maila. Token mógł wygasnąć.");
            }
        }
    }

}
