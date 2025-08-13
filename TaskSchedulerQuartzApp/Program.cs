using Microsoft.EntityFrameworkCore;
using Quartz;
using TaskSchedulerQuartzApp.Data;
using TaskSchedulerQuartzApp.Models;
using TaskSchedulerQuartzApp.Services;
using TaskSchedulerQuartzApp.Jobs;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();


// QUARTZ CONFIG
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    var tz = TimeZoneInfo.FindSystemTimeZoneById(builder.Configuration["Quartz:TimeZoneId"] ?? "Asia/Kolkata");

    var jobKey = new JobKey("DailyEmailJob");
    q.AddJob<DailyEmailJob>(opts => opts.WithIdentity(jobKey));

    /* q.AddTrigger(opts => opts
         .ForJob(jobKey)
         .WithIdentity("DailyEmailJob-trigger")
         .WithSchedule(CronScheduleBuilder
             .DailyAtHourAndMinute(22, 0) // 22:00 IST
             .InTimeZone(tz)
         )
     );*/

    q.AddTrigger(opts => opts
    .ForJob(jobKey)
    .WithIdentity("DailyEmailJob-trigger")
    .StartNow()
    .WithSimpleSchedule(x => x.WithIntervalInSeconds(15).RepeatForever())
);
});


/*for every 15 sec to check     instead     DailyAtHourAndMinute(22, 0)    */




builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=EmailLogs}/{action=Index}/{id?}");

app.Run();
