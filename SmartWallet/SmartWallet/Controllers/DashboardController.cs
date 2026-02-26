using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartWallet.Data;
using SmartWallet.Models;
using SmartWallet.Models.ViewModels;
using System.Net.Mail;

namespace SmartWallet.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly UserManager<ApplicationUser> _userManager;
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
        public async Task<IActionResult> PersonalData()
        {
            var user = await userManager.GetUserAsync(User);
            if (user is null) return NotFound();

            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await userManager.GetUserAsync(User);
            if (user is null) return NotFound();

            var model = new EditProfileViewModel
            {
                Name = user.Name,
                Surname = user.Surname,
                Address = user.address,
                Gender = user.Gender,
                ExistingAvatarUrl = user.ProfilePictureUrl

            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.FullName = $"{model.Name} {model.Surname}";
            user.address = model.Address;
            user.Gender = model.Gender;
            

            if (model.AvatarFile != null)
            {
                var extension = Path.GetExtension(model.AvatarFile.FileName).ToLower();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ProfilePicture", "Nieobsługiwany format pliku. Dozwolone: .jpg, .jpeg, .png");
                    return View(model);
                }
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/avatars", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.AvatarFile.CopyToAsync(stream);
                }
                if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ProfilePictureUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                user.ProfilePictureUrl = $"/uploads/avatars/{fileName}";

            }
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Dashboard");
            }


            return View(model);

        }








    }
}
