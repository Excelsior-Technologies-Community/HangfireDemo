using HangfireDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace HangfireDemo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppRecord> AppRecords { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppRecord>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Title)
                    .HasMaxLength(200)
                    .IsRequired();
            });
        }
    }
}
