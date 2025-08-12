using ArcTriggerAlphaBackend.Database.Interfaces;
using ArcTriggerAlphaBackend.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcTriggerAlphaBackend.Database.Repositories
{
    public sealed class MultiRepo : IMultilRepository
    {
        private readonly MinimalContext _minimalContext;

        public MultiRepo()
        {
            _minimalContext = new MinimalContext();
            _minimalContext.Database.EnsureCreated();
        }

        async Task IMultilRepository.AddOrderAsync(Order order)
        {
            await _minimalContext.Orders.AddAsync(order);
            await _minimalContext.SaveChangesAsync();
        }

        async Task IMultilRepository.AddStockAsync(Stock stock)
        {
            await _minimalContext.Stocks.AddAsync(stock);
            await _minimalContext.SaveChangesAsync();
        }

        async Task IMultilRepository.AddTaskAsync(StockUpdateTask task)
        {
            await _minimalContext.Tasks.AddAsync(task);
            await _minimalContext.SaveChangesAsync();
        }

        async Task IMultilRepository.AddTradeAsync(TradeSetup trade)
        {
            await _minimalContext.Trades.AddAsync(trade);
            await _minimalContext.SaveChangesAsync();
        }

        async Task IMultilRepository.AddUserAsync(User user)
        {
            await _minimalContext.Users.AddAsync(user);
            await _minimalContext.SaveChangesAsync();
        }

        async Task IMultilRepository.BulkInsertOrdersAsync(IEnumerable<Order> orders)
        {
            await _minimalContext.Orders.AddRangeAsync(orders);
            await _minimalContext.SaveChangesAsync();
        }

        async Task IMultilRepository.BulkUpdateStocksAsync(IEnumerable<Stock> stocks)
        {
            _minimalContext.Stocks.UpdateRange(stocks);
            await _minimalContext.SaveChangesAsync();
        }

        async Task IMultilRepository.DeleteOrderAsync(Guid id)
        {
            var order = await _minimalContext.Orders.FindAsync(id);
            if (order != null)
            {
                _minimalContext.Orders.Remove(order);
                await _minimalContext.SaveChangesAsync();
            }
        }

        async Task IMultilRepository.DeleteStockAsync(Guid id)
        {
            var stock = await _minimalContext.Stocks.FindAsync(id);
            if (stock != null)
            {
                _minimalContext.Stocks.Remove(stock);
                await _minimalContext.SaveChangesAsync();
            }
        }

        async Task IMultilRepository.DeleteTaskAsync(Guid id)
        {
            var task = await _minimalContext.Tasks.FindAsync(id);
            if (task != null)
            {
                _minimalContext.Tasks.Remove(task);
                await _minimalContext.SaveChangesAsync();
            }
        }

        async Task IMultilRepository.DeleteTradeAsync(Guid id)
        {
            var trade = await _minimalContext.Trades.FindAsync(id);
            if (trade != null)
            {
                _minimalContext.Trades.Remove(trade);
                await _minimalContext.SaveChangesAsync();
            }
        }

        async Task IMultilRepository.DeleteUserAsync(Guid id)
        {
            var user = await _minimalContext.Users.FindAsync(id);
            if (user != null)
            {
                _minimalContext.Users.Remove(user);
                await _minimalContext.SaveChangesAsync();
            }
        }

        async Task<List<StockUpdateTask>> IMultilRepository.GetActiveTasksAsync()
        {
            return await _minimalContext.Tasks
                .Where(t => t.IsActive)
                .AsNoTracking()
                .ToListAsync();
        }

        async Task<List<StockUpdateTask>> IMultilRepository.GetAllTasksAsync()
        {
            return await _minimalContext.Tasks
                .AsNoTracking()
                .ToListAsync();
        }

        async Task<List<TradeSetup>> IMultilRepository.GetAllTradesAsync()
        {
            return await _minimalContext.Trades
                .AsNoTracking()
                .ToListAsync();
        }

        async Task<List<User>> IMultilRepository.GetAllUsersAsync()
        {
            return await _minimalContext.Users
                .AsNoTracking()
                .ToListAsync();
        }

        async Task<Order?> IMultilRepository.GetOrderByIdAsync(Guid id)
        {
            return await _minimalContext.Orders
                .Include(o => o.Stock)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.ID == id);
        }

        async Task<List<Order>> IMultilRepository.GetOrdersAsync()
        {
            return await _minimalContext.Orders
                .AsNoTracking()
                .ToListAsync();
        }

        async Task<List<Order>> IMultilRepository.GetOrdersByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _minimalContext.Orders
                .Where(o => o.OrderDateTime >= start && o.OrderDateTime <= end)
                .AsNoTracking()
                .ToListAsync();
        }

        async Task<List<Order>> IMultilRepository.GetOrdersByStockIdAsync(Guid stockId)
        {
            return await _minimalContext.Orders
                .Where(o => o.StockId == stockId)
                .AsNoTracking()
                .ToListAsync();
        }

        async Task<Stock?> IMultilRepository.GetStockByIdAsync(Guid id)
        {
            return await _minimalContext.Stocks
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        async Task<Stock?> IMultilRepository.GetStockByTickerAtAsync(string ticker, DateTime snapshotTime)
        {
            return await _minimalContext.Stocks
                .Where(s => s.Ticker == ticker && s.SnapshotTime == snapshotTime)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        async Task<List<Stock>> IMultilRepository.GetStockHistoryAsync(string ticker)
        {
            return await _minimalContext.Stocks
                .Where(s => s.Ticker == ticker)
                .OrderByDescending(s => s.SnapshotTime)
                .AsNoTracking()
                .ToListAsync();
        }

        async Task<StockUpdateTask?> IMultilRepository.GetTaskByIdAsync(Guid id)
        {
            return await _minimalContext.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        async Task<TradeSetup?> IMultilRepository.GetTradeByIdAsync(Guid id)
        {
            return await _minimalContext.Trades
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        async Task<User?> IMultilRepository.GetUserByEmailAsync(string email)
        {
            return await _minimalContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        async Task<User?> IMultilRepository.GetUserByIdAsync(Guid id)
        {
            return await _minimalContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        async Task<List<Stock>> IMultilRepository.GetWatchlistStocksAsync()
        {
            return await _minimalContext.Stocks
                .Where(s => s.LastPrice > 0)
                .OrderBy(s => s.Ticker)
                .AsNoTracking()
                .ToListAsync();
        }

        async Task IMultilRepository.SaveChangesAsync()
        {
            await _minimalContext.SaveChangesAsync();
        }

        async Task IMultilRepository.UpdateLastRunAsync(Guid id)
        {
            var task = await _minimalContext.Tasks.FindAsync(id);
            if (task != null)
            {
                task.MarkUpdated();
                await _minimalContext.SaveChangesAsync();
            }
        }

        async Task IMultilRepository.UpdateTaskStateAsync(Guid id, bool isActive)
        {
            var task = await _minimalContext.Tasks.FindAsync(id);
            if (task != null)
            {
                task.SetActive(isActive);
                await _minimalContext.SaveChangesAsync();
            }
        }

        async Task<bool> IMultilRepository.UserExistsAsync(string email)
        {
            return await _minimalContext.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        void IMultilRepository.Reload()
        {
            // Implementation not needed for current requirements
        }
    }
}