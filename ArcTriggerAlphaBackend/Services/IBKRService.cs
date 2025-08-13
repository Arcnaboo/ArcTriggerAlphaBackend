using ArcTriggerAlphaBackend.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ArcTriggerAlphaBackend.Services
{
    public class IBKRService : IStockBrokerService
    {
        private static readonly Lazy<IBKRService> _instance = new Lazy<IBKRService>(() => new IBKRService());
        public static IBKRService Instance => _instance.Value;

        private readonly HttpClient _http = new HttpClient();
        private bool _isAuthenticated;

        private IBKRService()
        {
            _http.BaseAddress = new Uri("http://localhost:5000/v1/api");
        }

        async Task<bool> IStockBrokerService.IsConnectedAsync()
        {
            try
            {
                var response = await _http.GetAsync("iserver/auth/status").ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                _isAuthenticated = content.Contains("\"authenticated\":true");
                return _isAuthenticated;
            }
            catch
            {
                return false;
            }
        }

        async Task<bool> IStockBrokerService.ConnectAsync()
        {
            try
            {
                var response = await _http.PostAsync("iserver/auth/status", null).ConfigureAwait(false);
                _isAuthenticated = response.IsSuccessStatusCode;
                return _isAuthenticated;
            }
            catch
            {
                _isAuthenticated = false;
                return false;
            }
        }

        async Task IStockBrokerService.DisconnectAsync()
        {
            try
            {
                await _http.PostAsync("logout", null).ConfigureAwait(false);
            }
            finally
            {
                _isAuthenticated = false;
            }
        }

        async Task<decimal?> IStockBrokerService.GetLastPriceAsync(string symbol)
        {
            try
            {
                var response = await _http.GetAsync($"marketdata/snapshot?symbols={symbol}").ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var data = await JsonSerializer.DeserializeAsync<Dictionary<string, decimal>>(stream).ConfigureAwait(false);
                return data?["lastPrice"];
            }
            catch
            {
                return null;
            }
        }

        async Task<decimal?> IStockBrokerService.GetBidPriceAsync(string symbol)
        {
            try
            {
                var response = await _http.GetAsync($"marketdata/snapshot?symbols={symbol}").ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var data = await JsonSerializer.DeserializeAsync<Dictionary<string, decimal>>(stream).ConfigureAwait(false);
                return data?["bidPrice"];
            }
            catch
            {
                return null;
            }
        }

        async Task<decimal?> IStockBrokerService.GetAskPriceAsync(string symbol)
        {
            try
            {
                var response = await _http.GetAsync($"marketdata/snapshot?symbols={symbol}").ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var data = await JsonSerializer.DeserializeAsync<Dictionary<string, decimal>>(stream).ConfigureAwait(false);
                return data?["askPrice"];
            }
            catch
            {
                return null;
            }
        }

        async Task<SupportModels.MarketSnapshot> IStockBrokerService.GetMarketSnapshotAsync(string symbolOrConid)
        {
            try
            {
                var response = await _http.GetAsync($"marketdata/snapshot?conid={symbolOrConid}").ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                return await JsonSerializer.DeserializeAsync<SupportModels.MarketSnapshot>(stream).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        async Task<List<SupportModels.MarketSnapshot>> IStockBrokerService.GetBulkMarketSnapshotAsync(IEnumerable<string> symbolsOrConids)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("marketdata/snapshots", symbolsOrConids).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                return await JsonSerializer.DeserializeAsync<List<SupportModels.MarketSnapshot>>(stream).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        async Task<SupportModels.ContractInfo> IStockBrokerService.ResolveSymbolAsync(string symbol)
        {
            try
            {
                var response = await _http.GetAsync($"trsrv/stocks?symbols={symbol}").ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                return await JsonSerializer.DeserializeAsync<SupportModels.ContractInfo>(stream).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        // All other methods remain exactly as in your original file
        Task<List<SupportModels.OptionContract>> IStockBrokerService.GetOptionChainAsync(string underlyingSymbol)
            => throw new NotImplementedException();

        Task<SupportModels.OptionContract> IStockBrokerService.GetOptionContractAsync(string symbol, decimal strike, string right, DateTime expiry)
            => throw new NotImplementedException();

        Task<SupportModels.AccountSummary> IStockBrokerService.GetAccountSummaryAsync()
            => throw new NotImplementedException();

        Task<List<SupportModels.Holding>> IStockBrokerService.GetOpenPositionsAsync()
            => throw new NotImplementedException();

        Task<List<SupportModels.Trade>> IStockBrokerService.GetRecentTradesAsync()
            => throw new NotImplementedException();

        Task<decimal> IStockBrokerService.GetCashBalanceAsync(string currency)
            => throw new NotImplementedException();

        Task<SupportModels.OrderResult> IStockBrokerService.PlaceOrderAsync(SupportModels.BrokerOrder order)
            => throw new NotImplementedException();

        Task<SupportModels.OrderResult> IStockBrokerService.ModifyOrderAsync(string orderId, SupportModels.BrokerOrder newOrder)
            => throw new NotImplementedException();

        Task<bool> IStockBrokerService.CancelOrderAsync(string orderId)
            => throw new NotImplementedException();

        Task<SupportModels.OrderStatus> IStockBrokerService.GetOrderStatusAsync(string orderId)
            => throw new NotImplementedException();

        Task<List<SupportModels.OrderStatus>> IStockBrokerService.GetOpenOrdersAsync()
            => throw new NotImplementedException();

        Task<string> IStockBrokerService.PreviewOrderAsync(SupportModels.BrokerOrder order)
            => throw new NotImplementedException();

        Task<SupportModels.BrokerCommissionEstimate> IStockBrokerService.EstimateCommissionAsync(SupportModels.BrokerOrder order)
            => throw new NotImplementedException();

        Task<SupportModels.OrderResult> IStockBrokerService.PlaceComboOrderAsync(SupportModels.ComboOrder comboOrder)
            => throw new NotImplementedException();

        Task<DateTime> IStockBrokerService.GetServerTimeAsync()
            => throw new NotImplementedException();

        Task<bool> IStockBrokerService.KeepAliveAsync()
            => throw new NotImplementedException();
    }
}