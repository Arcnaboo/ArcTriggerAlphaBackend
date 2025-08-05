using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ArcTriggerAlphaBackend.Entities;

namespace ArcTriggerAlphaBackend.Database.Interfaces
{
    public interface IMultilRepository
    {
        // User
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<bool> UserExistsAsync(string email);
        Task AddUserAsync(User user);
        Task DeleteUserAsync(Guid id);
        Task<List<User>> GetAllUsersAsync();

        // Orders
        Task<Order?> GetOrderByIdAsync(Guid id);
        Task<List<Order>> GetOrdersAsync();
        Task AddOrderAsync(Order order);
        Task DeleteOrderAsync(Guid id);

        // Trade Setups
        Task<TradeSetup?> GetTradeByIdAsync(Guid id);
        Task<List<TradeSetup>> GetAllTradesAsync();
        Task AddTradeAsync(TradeSetup trade);
        Task DeleteTradeAsync(Guid id);

        // Stocks
        Task<Stock?> GetStockByIdAsync(Guid id);
        Task<Stock?> GetStockByTickerAtAsync(string ticker, DateTime snapshotTime);
        Task AddStockAsync(Stock stock);
        Task DeleteStockAsync(Guid id);
        Task<List<Stock>> GetStockHistoryAsync(string ticker);

        // Stock Update Tasks
        Task<StockUpdateTask?> GetTaskByIdAsync(Guid id);
        Task<List<StockUpdateTask>> GetAllTasksAsync();
        Task<List<StockUpdateTask>> GetActiveTasksAsync();
        Task AddTaskAsync(StockUpdateTask task);
        Task DeleteTaskAsync(Guid id);
        Task UpdateTaskStateAsync(Guid id, bool isActive);
        Task UpdateLastRunAsync(Guid id);

        // Persistence
        Task SaveChangesAsync();
        void Reload();
    }
}
