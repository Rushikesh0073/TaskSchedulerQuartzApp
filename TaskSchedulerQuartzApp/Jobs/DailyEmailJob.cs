using Quartz;
using System.Text;
using TaskSchedulerQuartzApp.Data;
using TaskSchedulerQuartzApp.Models;

using Microsoft.EntityFrameworkCore;
using TaskSchedulerQuartzApp.Services;



namespace TaskSchedulerQuartzApp.Jobs
{
    public class DailyEmailJob : IJob
    {
        private readonly ApplicationDbContext db;
        private readonly IEmailSender emailSender;
        private readonly ILogger<DailyEmailJob> logger;
        public DailyEmailJob(ApplicationDbContext db, IEmailSender emailSender, ILogger<DailyEmailJob> logger)
        {
            this.db = db;
            this.emailSender = emailSender;
            this.logger = logger;
        }  
        
        
        public async Task Execute(IJobExecutionContext context)
            {
                logger.LogInformation("DailyEmailJob started at {Time}", DateTime.UtcNow);

                var nowIst = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata"));
                var today = nowIst.Date;
                var tomorrow = today.AddDays(1);

                var tasksDueTomorrow = await db.Tasks
                    .Where(t => t.DueDate >= tomorrow && t.DueDate < tomorrow.AddDays(1))
                    .ToListAsync();

                var sb = new StringBuilder();
                sb.AppendLine($"<h2>Daily Scheduler — {nowIst:yyyy-MM-dd} (IST)</h2>");
            sb.AppendLine($"<h2> Your task is scheduled </h2>");
            sb.AppendLine($"<h3>Tasks due tomorrow:</h3>");
            sb.AppendLine($"</br> </br> <h2> Thank you</h2>");
            if (!tasksDueTomorrow.Any())
            {
                /* sb.AppendLine("<p>No tasks due tomorrow.</p>");*/
            }
            else
            {
                sb.AppendLine("<ul>");
                foreach (var t in tasksDueTomorrow)
                    sb.AppendLine($"<li>{t.Title} — due {t.DueDate:yyyy-MM-dd}</li>");
                sb.AppendLine("</ul>");
            }

                var subject = $"Daily message @ 10 PM IST — {today:yyyy-MM-dd}";
                try
                {
                    await emailSender.SendAsync(subject, sb.ToString());
                    db.EmailLogs.Add(new EmailLog
                    {
                        SentAtUtc = DateTime.UtcNow,
                        To = "rushikesh0073@gmail.com",// "(configured recipients)"
                        Subject = subject,
                        Success = true
                    });
                    await db.SaveChangesAsync();
                    logger.LogInformation("Daily email sent successfully.");
                }
                catch (Exception ex)
                {
                    db.EmailLogs.Add(new EmailLog
                    {
                        SentAtUtc = DateTime.UtcNow,
                        To = "rushikesh0073@gmail.com",//"(configured recipients)"
                        Subject = subject,
                        Success = false,
                        ErrorMessage = ex.Message
                    });
                    await db.SaveChangesAsync();
                    logger.LogError(ex, "Failed to send daily email.");
                }
            }
        }


    }

