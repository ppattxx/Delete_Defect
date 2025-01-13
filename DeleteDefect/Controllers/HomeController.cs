using System.Diagnostics;
using DeleteDefect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using DeleteDefect.Data;

namespace DeleteDefect.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: Halaman Login
        [HttpGet]
        public IActionResult Index()
        {
            // Cek apakah user sudah login
            if (HttpContext.Session.GetString("UserNIK") != null)
            {
                return RedirectToAction("Index", "Defect"); // Redirect ke halaman utama jika sudah login
            }
            return View();
        }

        // POST: Login Logic
        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {
            var user = GetUserByUsername(Username);

            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash))
                {
                    // Tentukan role berdasarkan NIK
                    string[] adminPrefixes = { "7098", "6950", "7217" };
                    string role = adminPrefixes.Any(prefix => user.NIK.StartsWith(prefix)) ? "Admin" : "User";


                    // Simpan informasi user dan role ke session
                    HttpContext.Session.SetString("UserNIK", user.NIK);
                    HttpContext.Session.SetString("UserName", user.Name);
                    HttpContext.Session.SetString("UserRole", role);

                    return RedirectToAction("Index", "Defect"); // Redirect ke halaman utama sesuai role
                }

                ViewData["ErrorMessage"] = "Invalid NIK or password.";
            }
            else
            {
                ViewData["ErrorMessage"] = "User not found.";
            }

            return View("Index");
        }

        // Fungsi untuk mendapatkan user berdasarkan Username (NIK)
        public InspectorModel GetUserByUsername(string username)
        {
            return _context.AspNetUsers
                .Where(u => u.NIK == username)
                .Select(u => new InspectorModel
                {
                    NIK = u.NIK,
                    Name = u.Name,
                    PasswordHash = u.PasswordHash
                })
                .FirstOrDefault();
        }

        // Logout Logic
        [HttpPost]
        public IActionResult Logout()
        {
            // Hapus semua data session
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }

        // Error Handling
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}