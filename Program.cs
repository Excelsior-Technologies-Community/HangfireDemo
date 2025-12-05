using Hangfire;
using Hangfire.SqlServer;
using HangfireDemo.Data;
using HangfireDemo.Jobs;
using HangfireDemo.Models;
using HangfireDemo.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? "Server=.;Database=HangfireDemoDb;Trusted_Connection=True;TrustServerCertificate=True;";

// EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Hangfire configuration
builder.Services.AddHangfire(config =>
{
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
          {
              CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
              SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
              QueuePollInterval = TimeSpan.FromSeconds(15),
              UseRecommendedIsolationLevel = true,
              DisableGlobalLocks = true
          });
});

// Hangfire server
builder.Services.AddHangfireServer();

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// MVC
builder.Services.AddControllersWithViews();

// DI for jobs & services
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<DailySummaryJob>();
builder.Services.AddScoped<RecordExpiryJob>();
builder.Services.AddScoped<DbCleanupJob>();

var app = builder.Build();

// Auto migrate DB
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Hangfire Dashboard (monitoring job status)
app.UseHangfireDashboard("/hangfire");

// Schedule recurring jobs
using (var scope = app.Services.CreateScope())
{
    RecurringJob.AddOrUpdate<DailySummaryJob>(
        "daily-summary-email",
        job => job.RunAsync(),
        Cron.Daily(9)          // Every day at 09:00
    );

    RecurringJob.AddOrUpdate<RecordExpiryJob>(
        "auto-expire-records",
        job => job.RunAsync(),
        Cron.Hourly()          // Every hour
    );

    RecurringJob.AddOrUpdate<DbCleanupJob>(
        "db-cleanup",
        job => job.RunAsync(),
        Cron.Daily(2)          // Every day at 02:00
    );
}

// Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Records}/{action=Index}/{id?}");

app.Run();
