using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskSchedulerQuartzApp.Data;
using TaskSchedulerQuartzApp.Models;

namespace TaskSchedulerQuartzApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TasksController(ApplicationDbContext db) 
        {  
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _db.Tasks.ToListAsync();
            return View(items);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.Tasks.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.Tasks.FindAsync(id);
            if (item == null)
            {
                return NotFound();

            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskItem model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _db.Tasks.Update(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _db.Tasks.FindAsync(id);
            if (item == null) { 
                return NotFound();
            }
            return View(item);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Tasks.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.Tasks.FindAsync(id);
            if (item != null)
            {
                _db.Tasks.Remove(item);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
