using HangfireDemo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HangfireDemo.Jobs
{
    public class DbCleanupJob
    {
        private readonly AppDbContext _db;
        private readonly ILogger<DbCleanupJob> _logger;

        public DbCleanupJob(AppDbContext db, ILogger<DbCleanupJob> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task RunAsync()
        {
            _logger.LogInformation("DbCleanupJob started.");

            // Example: delete expired records older than 30 days
            var threshold = DateTime.Now.AddDays(-30);

            var oldExpired = await _db.AppRecords
                .Where(r => r.IsExpired && r.ExpiryDate <= threshold)
                .ToListAsync();

            if (oldExpired.Any())
            {
                _db.AppRecords.RemoveRange(oldExpired);
                await _db.SaveChangesAsync();

                _logger.LogInformation("DbCleanupJob deleted {Count} old expired records.", oldExpired.Count);
            }
            else
            {
                _logger.LogInformation("DbCleanupJob: no records to clean.");
            }
        }
    }
}
