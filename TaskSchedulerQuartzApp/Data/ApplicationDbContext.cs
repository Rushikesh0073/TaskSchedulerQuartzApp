using Microsoft.EntityFrameworkCore;
using TaskSchedulerQuartzApp.Models;

namespace TaskSchedulerQuartzApp.Data
{
    /* public class ApplicationDbContext:DbContext
     {

     }*/

    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<EmailLog> EmailLogs => Set<EmailLog>();
    }
}
