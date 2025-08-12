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
            optionsBuilder
                .UseSqlite("Data Source=arc_trigger_alpha.db")
                .UseLazyLoadingProxies();
                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Users
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Stocks
            modelBuilder.Entity<Stock>()
                .HasIndex(s => s.Ticker);

            // Orders
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Stock)
                .WithMany()
                .HasForeignKey(o => o.StockId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderDateTime);

            // StockUpdateTasks
            modelBuilder.Entity<StockUpdateTask>()
                .HasOne<Stock>()
                .WithMany()
                .HasForeignKey("StockId")
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}