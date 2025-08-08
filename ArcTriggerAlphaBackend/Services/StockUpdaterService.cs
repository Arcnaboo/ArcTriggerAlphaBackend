using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ArcTriggerAlphaBackend.Services
{
    /// <summary>
    /// Purpose is to read StockUpdates and update stock prices based on time intervals
    /// </summary>
    public class StockUpdaterService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // TODO: Fetch active StockUpdateTasks
            // TODO: Compare LastUpdateTime + DeltaSeconds vs DateTime.UtcNow
            // TODO: If ready, call IBKR API to get price and update Stock
            // TODO: Update LastUpdateTime in task
            // TODO: Save changes via repository.SaveChangesAsync()

            throw new NotImplementedException();
        }
    }
}
