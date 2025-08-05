using ArcTriggerAlphaBackend.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArcTriggerAlphaBackend.Database
{
    public class MinimalContext : DbContext
    {
        public DbSet<User> Users { get; private set; } = default!;
        public DbSet<Order> Orders { get; private set; } = default!;
        public DbSet<TradeSetup> Trades { get; private set; } = default!;
        public DbSet<Stock> Stocks { get; private set; } = default!;
        public DbSet<StockUpdateTask> Tasks { get; private set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=arc_trigger_alpha.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<StockUpdateTask>()
                .HasOne<Stock>()
                .WithMany()
                .HasForeignKey("StockId")
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
