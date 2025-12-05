using HangfireDemo.Data;
using HangfireDemo.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HangfireDemo.Jobs
{
    public class DailySummaryJob
    {
        private readonly AppDbContext _db;
        private readonly IEmailService _emailService;
        private readonly ILogger<DailySummaryJob> _logger;

        public DailySummaryJob(
            AppDbContext db,
            IEmailService emailService,
            ILogger<DailySummaryJob> logger)
        {
            _db = db;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task RunAsync()
        {
            _logger.LogInformation("DailySummaryJob started.");

            var since = DateTime.Now.AddDays(-1);

            var newRecordsCount = await _db.AppRecords
                .Where(r => r.CreatedAt >= since)
                .CountAsync();

            var expiredCount = await _db.AppRecords
                .Where(r => r.IsExpired && r.CreatedAt >= since)
                .CountAsync();

            var subject = "Daily records summary";
            var body = $@"
            Daily Summary ({DateTime.Now:yyyy-MM-dd}):

            New records in last 24 hours: {newRecordsCount}
            Newly expired records in last 24 hours: {expiredCount}
            ";

            // Send to a demo email
            await _emailService.SendEmailAsync("aryanprajapati7990@gmail.com", subject, body);

            _logger.LogInformation("DailySummaryJob finished. New={NewRecords}, Expired={Expired}",
                newRecordsCount, expiredCount);
        }
    }
}
