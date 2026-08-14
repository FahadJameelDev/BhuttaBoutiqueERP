using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BhuttaBoutiqueERP.Data;
using BhuttaBoutiqueERP.Models.Auth.Entities;
using BhuttaBoutiqueERP.Models.Auth.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace BhuttaBoutiqueERP.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // 1️⃣ GET: Login Page
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // 2️⃣ POST: Login Logic
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == model.Email && u.IsActive);

            if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            // Session create karo
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name ?? "User");
            HttpContext.Session.SetString("UserEmail", user.Email ?? "");
            HttpContext.Session.SetInt32("UserRoleId", user.RoleId);
            HttpContext.Session.SetString("UserRole", user.Role?.Name ?? "Staff");

            return RedirectToAction("Index", "Home");
        }

        // 3️⃣ GET: Logout
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // 4️⃣ GET: Access Denied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // 🔐 Helper: Password Hash Verify (Null-safe)
        private bool VerifyPassword(string? plainPassword, string? hashedPassword)
        {
            if (string.IsNullOrEmpty(plainPassword) || string.IsNullOrEmpty(hashedPassword))
            {
                return false;
            }

            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainPassword));
            var computedHash = Convert.ToBase64String(hashedBytes);
            return computedHash == hashedPassword;
        }
    }
}