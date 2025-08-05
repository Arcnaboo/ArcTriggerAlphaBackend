using ArcTriggerAlphaBackend.Database.Interfaces;
using ArcTriggerAlphaBackend.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcTriggerAlphaBackend.Database.Repositories
{
    public class MultiRepo : IMultilRepository
    {
        private readonly MinimalContext _context = new();

        Task IMultilRepository.AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            return Task.CompletedTask;
        }

        Task IMultilRepository.AddStockAsync(Stock stock)
        {
            _context.Stocks.Add(stock);
            return Task.CompletedTask;
        }

        Task IMultilRepository.AddTaskAsync(StockUpdateTask task)
        {
            _context.Tasks.Add(task);
            return Task.CompletedTask;
        }

        Task IMultilRepository.AddTradeAsync(TradeSetup trade)
        {
            _context.Trades.Add(trade);
            return Task.CompletedTask;
        }

        Task IMultilRepository.AddUserAsync(User user)
        {
            _context.Users.Add(user);
            return Task.CompletedTask;
        }

        async Task IMultilRepository.DeleteOrderAsync(Guid id)
        {
            var entity = await _context.Orders.FindAsync(id);
            if (entity != null) _context.Orders.Remove(entity);
        }

        async Task IMultilRepository.DeleteStockAsync(Guid id)
        {
            var entity = await _context.Stocks.FindAsync(id);
            if (entity != null) _context.Stocks.Remove(entity);
        }

        async Task IMultilRepository.DeleteTaskAsync(Guid id)
        {
            var entity = await _context.Tasks.FindAsync(id);
            if (entity != null) _context.Tasks.Remove(entity);
        }

        async Task IMultilRepository.DeleteTradeAsync(Guid id)
        {
            var entity = await _context.Trades.FindAsync(id);
            if (entity != null) _context.Trades.Remove(entity);
        }

        async Task IMultilRepository.DeleteUserAsync(Guid id)
        {
            var entity = await _context.Users.FindAsync(id);
            if (entity != null) _context.Users.Remove(entity);
        }

        Task<List<StockUpdateTask>> IMultilRepository.GetActiveTasksAsync()
        {
            return _context.Tasks.Where(t => t.IsActive).ToListAsync();
        }

        Task<List<StockUpdateTask>> IMultilRepository.GetAllTasksAsync()
        {
            return _context.Tasks.ToListAsync();
        }

        Task<List<TradeSetup>> IMultilRepository.GetAllTradesAsync()
        {
            return _context.Trades.ToListAsync();
        }

        Task<List<User>> IMultilRepository.GetAllUsersAsync()
        {
            return _context.Users.ToListAsync();
        }

        Task<Order?> IMultilRepository.GetOrderByIdAsync(Guid id)
        {
            return _context.Orders.FindAsync(id).AsTask();
        }

        Task<List<Order>> IMultilRepository.GetOrdersAsync()
        {
            return _context.Orders.ToListAsync();
        }

        Task<Stock?> IMultilRepository.GetStockByIdAsync(Guid id)
        {
            return _context.Stocks.FindAsync(id).AsTask();
        }

        Task<Stock?> IMultilRepository.GetStockByTickerAtAsync(string ticker, DateTime snapshotTime)
        {
            return _context.Stocks
                .Where(s => s.Ticker == ticker && s.SnapshotTime == snapshotTime)
                .FirstOrDefaultAsync();
        }

        Task<List<Stock>> IMultilRepository.GetStockHistoryAsync(string ticker)
        {
            return _context.Stocks
                .Where(s => s.Ticker == ticker)
                .OrderByDescending(s => s.SnapshotTime)
                .ToListAsync();
        }

        Task<StockUpdateTask?> IMultilRepository.GetTaskByIdAsync(Guid id)
        {
            return _context.Tasks.FindAsync(id).AsTask();
        }

        Task<TradeSetup?> IMultilRepository.GetTradeByIdAsync(Guid id)
        {
            return _context.Trades.FindAsync(id).AsTask();
        }

        Task<User?> IMultilRepository.GetUserByEmailAsync(string email)
        {
            return _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        Task<User?> IMultilRepository.GetUserByIdAsync(Guid id)
        {
            return _context.Users.FindAsync(id).AsTask();
        }

        Task<bool> IMultilRepository.UserExistsAsync(string email)
        {
            return _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        async Task IMultilRepository.UpdateLastRunAsync(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                typeof(StockUpdateTask).GetProperty("LastUpdateTime")!
                    .SetValue(task, DateTime.UtcNow);
            }
        }

        async Task IMultilRepository.UpdateTaskStateAsync(Guid id, bool isActive)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                typeof(StockUpdateTask).GetProperty("IsActive")!
                    .SetValue(task, isActive);
            }
        }

        void IMultilRepository.Reload()
        {
            // No-op (optional hook for manual refresh)
        }

        async Task IMultilRepository.SaveChangesAsync()
        {
            int retry = 3;
            bool failed;
            do
            {
                failed = false;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    failed = true;
                    if (--retry < 0) throw;

                    foreach (var entry in ex.Entries)
                    {
                        await entry.ReloadAsync();
                        if (entry.State == EntityState.Modified)
                        {
                            var db = entry.GetDatabaseValues();
                            entry.OriginalValues.SetValues(db);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Save Error] {ex.Message}");
                    throw;
                }
            } while (failed && retry > 0);
        }
    }
}
