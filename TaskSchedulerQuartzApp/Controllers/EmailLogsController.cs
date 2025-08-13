using Microsoft.AspNetCore.Mvc;
using TaskSchedulerQuartzApp.Data;
using Microsoft.EntityFrameworkCore;

namespace TaskSchedulerQuartzApp.Controllers
{
    public class EmailLogsController : Controller
    {

        ApplicationDbContext _db;
      
        public EmailLogsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var logs = await _db.EmailLogs
                .AsNoTracking()
                .OrderByDescending(x => x.SentAtUtc)      
                .ToListAsync();
            return View(logs);
        }
    }
}
