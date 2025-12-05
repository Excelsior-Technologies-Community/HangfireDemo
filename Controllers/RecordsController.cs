using HangfireDemo.Data;
using HangfireDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HangfireDemo.Controllers
{
    public class RecordsController : Controller
    {
        private readonly AppDbContext _db;

        public RecordsController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var records = await _db.AppRecords
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return View(records);
        }

        public IActionResult Create()
        {
            var model = new AppRecord
            {
                ExpiryDate = DateTime.Now.AddDays(7) // default 7 days
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppRecord model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.CreatedAt = DateTime.Now;
            model.IsExpired = false;

            _db.AppRecords.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
