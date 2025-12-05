using HangfireDemo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HangfireDemo.Jobs
{
    public class RecordExpiryJob
    {
        private readonly AppDbContext _db;
        private readonly ILogger<RecordExpiryJob> _logger;

        public RecordExpiryJob(AppDbContext db, ILogger<RecordExpiryJob> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task RunAsync()
        {
            _logger.LogInformation("RecordExpiryJob started.");

            var now = DateTime.Now;

            var toExpire = await _db.AppRecords
                .Where(r => !r.IsExpired && r.ExpiryDate <= now)
                .ToListAsync();

            if (toExpire.Any())
            {
                foreach (var rec in toExpire)
                {
                    rec.IsExpired = true;
                }

                await _db.SaveChangesAsync();

                _logger.LogInformation("RecordExpiryJob expired {Count} records.", toExpire.Count);
            }
            else
            {
                _logger.LogInformation("RecordExpiryJob: no records to expire.");
            }
        }
    }
}
