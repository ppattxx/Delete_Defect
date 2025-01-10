using DeleteDefect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeleteDefect.Controllers
{
    public class DefectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DefectController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Mengambil Defect_Results beserta LocationName menggunakan Include
            var products = await _context.Defect_Results
                .Include(d => d.Location) // Join dengan tabel Locations
                .ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Selected(DateTime? selectedDate)
        {
            ViewData["SelectedDate"] = selectedDate?.ToString("yyyy-MM-dd");

            IQueryable<DefectModel> defects;

            if (selectedDate.HasValue)
            {
                defects = _context.Defect_Results
                    .Include(d => d.Location) // Join dengan tabel Locations
                    .Where(d => d.DateTime.Date == selectedDate.Value.Date);
            }
            else
            {
                defects = _context.Defect_Results
                    .Include(d => d.Location); // Join dengan tabel Locations
            }

            var defectList = await defects.ToListAsync();

            return View("Index", defectList);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Defect_Results.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Defect_Results.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
