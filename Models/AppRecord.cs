using System;

namespace HangfireDemo.Models
{
    public class AppRecord
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // When record should be considered “expired”
        public DateTime ExpiryDate { get; set; }

        public bool IsExpired { get; set; } = false;
    }
}
