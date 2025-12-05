using Hangfire;
using HangfireDemo.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace HangfireDemo.Controllers
{
    public class JobsController : Controller
    {
        // Main page
        public IActionResult Index()
        {
            return View();
        }

        // Trigger Daily Summary Email
        [HttpPost]
        public IActionResult RunDailySummary()
        {
            BackgroundJob.Enqueue<DailySummaryJob>(job => job.RunAsync());
            TempData["msg"] = "Daily Summary Email Job Triggered!";

            return RedirectToAction("Index");
        }

        // Trigger Auto Expire Records
        [HttpPost]
        public IActionResult RunRecordExpiry()
        {
            BackgroundJob.Enqueue<RecordExpiryJob>(job => job.RunAsync());
            TempData["msg"] = "Record Expiry Job Triggered!";
            return RedirectToAction("Index");
        }

        // Trigger DB Cleanup
        [HttpPost]
        public IActionResult RunDbCleanup()
        {
            BackgroundJob.Enqueue<DbCleanupJob>(job => job.RunAsync());
            TempData["msg"] = "DB Cleanup Job Triggered!";
            return RedirectToAction("Index");
        }
    }
}
