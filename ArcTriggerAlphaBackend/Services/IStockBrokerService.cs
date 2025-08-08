using static ArcTriggerAlphaBackend.Models.SupportModels;

namespace ArcTriggerAlphaBackend.Services
{
    public interface IStockBrokerService
    {
        // 🔐 Auth & Session
        Task<bool> IsConnectedAsync();
        Task<bool> ConnectAsync();
        Task DisconnectAsync();

        // 📈 Market Data
        Task<decimal?> GetLastPriceAsync(string symbol);
        Task<decimal?> GetBidPriceAsync(string symbol);
        Task<decimal?> GetAskPriceAsync(string symbol);
        Task<MarketSnapshot> GetMarketSnapshotAsync(string symbolOrConid);
        Task<List<MarketSnapshot>> GetBulkMarketSnapshotAsync(IEnumerable<string> symbolsOrConids);

        // 🔎 Contract / Instrument Info
        Task<ContractInfo> ResolveSymbolAsync(string symbol);  // to conid
        Task<List<OptionContract>> GetOptionChainAsync(string underlyingSymbol);
        Task<OptionContract> GetOptionContractAsync(string symbol, decimal strike, string right, DateTime expiry);

        // 💼 Portfolio & Account
        Task<AccountSummary> GetAccountSummaryAsync();
        Task<List<Holding>> GetOpenPositionsAsync();
        Task<List<Trade>> GetRecentTradesAsync();
        Task<decimal> GetCashBalanceAsync(string currency = "USD");

        // 🛒 Order Management
        Task<OrderResult> PlaceOrderAsync(BrokerOrder order);
        Task<OrderResult> ModifyOrderAsync(string orderId, BrokerOrder newOrder);
        Task<bool> CancelOrderAsync(string orderId);
        Task<OrderStatus> GetOrderStatusAsync(string orderId);
        Task<List<OrderStatus>> GetOpenOrdersAsync();

        // 🧪 Trade Utilities
        Task<string> PreviewOrderAsync(BrokerOrder order);
        Task<BrokerCommissionEstimate> EstimateCommissionAsync(BrokerOrder order);

        // 🛠 Combo Orders (multi-leg)
        Task<OrderResult> PlaceComboOrderAsync(ComboOrder comboOrder);

        // ⚙️ Utility
        Task<DateTime> GetServerTimeAsync();
        Task<bool> KeepAliveAsync();
    }
}
