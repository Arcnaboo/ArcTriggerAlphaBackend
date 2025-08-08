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

        private readonly HttpClient _http;
        private const string BaseUrl = "http://localhost:5000/v1/api";
        private bool _connected = false;

        private IBKRService()
        {
            _http = new HttpClient();
        }

        public async Task<bool> ConnectAsync()
        {
            try
            {
                var response = await _http.GetAsync($"{BaseUrl}/iserver/auth/status");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                _connected = json.Contains("\"authenticated\":true");
                return _connected;
            }
            catch
            {
                _connected = false;
                return false;
            }
        }

        public async Task DisconnectAsync()
        {
            try
            {
                await _http.PostAsync($"{BaseUrl}/logout", null);
            }
            catch { }
            finally
            {
                _connected = false;
            }
        }

        public Task<bool> IsConnectedAsync()
        {
            return Task.FromResult(_connected);
        }

        public async Task<bool> KeepAliveAsync()
        {
            try
            {
                var response = await _http.PostAsync($"{BaseUrl}/iserver/reauthenticate", null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<DateTime> GetServerTimeAsync()
        {
            var response = await _http.GetAsync($"{BaseUrl}/iserver/time");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var unixMillis = long.Parse(json);
            return DateTimeOffset.FromUnixTimeMilliseconds(unixMillis).DateTime;
        }

        public async Task<SupportModels.ContractInfo> ResolveSymbolAsync(string symbol)
        {
            var res = await _http.GetAsync($"{BaseUrl}/trsrv/stocks?symbols={symbol}");
            res.EnsureSuccessStatusCode();
            var json = await res.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            if (!root.TryGetProperty(symbol, out var details)) return null;
            var item = details[0];

            return new SupportModels.ContractInfo
            {
                Symbol = item.GetProperty("symbol").GetString(),
                ConId = item.GetProperty("conid").GetRawText(),
                SecurityType = item.GetProperty("sectype").GetString(),
                Exchange = item.GetProperty("exchange").GetString(),
                Currency = item.GetProperty("currency").GetString(),
                CompanyName = item.TryGetProperty("companyName", out var nameProp) ? nameProp.GetString() : null
            };
        }

        public Task<bool> CancelOrderAsync(string orderId)
        {
            throw new NotImplementedException();
        }

        public Task<SupportModels.BrokerCommissionEstimate> EstimateCommissionAsync(SupportModels.BrokerOrder order)
        {
            throw new NotImplementedException();
        }

        public Task<SupportModels.AccountSummary> GetAccountSummaryAsync()
        {
            throw new NotImplementedException();
        }

        public Task<decimal?> GetAskPriceAsync(string symbol)
        {
            throw new NotImplementedException();
        }

        public Task<decimal?> GetBidPriceAsync(string symbol)
        {
            throw new NotImplementedException();
        }

        public Task<List<SupportModels.MarketSnapshot>> GetBulkMarketSnapshotAsync(IEnumerable<string> symbolsOrConids)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetCashBalanceAsync(string currency)
        {
            throw new NotImplementedException();
        }

        public Task<decimal?> GetLastPriceAsync(string symbol)
        {
            throw new NotImplementedException();
        }

        public Task<SupportModels.MarketSnapshot> GetMarketSnapshotAsync(string symbolOrConid)
        {
            throw new NotImplementedException();
        }

        public Task<List<SupportModels.OrderStatus>> GetOpenOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<SupportModels.Holding>> GetOpenPositionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<SupportModels.OptionContract>> GetOptionChainAsync(string underlyingSymbol)
        {
            throw new NotImplementedException();
        }

        public Task<SupportModels.OptionContract> GetOptionContractAsync(string symbol, decimal strike, string right, DateTime expiry)
        {
            throw new NotImplementedException();
        }

        public Task<SupportModels.OrderStatus> GetOrderStatusAsync(string orderId)
        {
            throw new NotImplementedException();
        }

        public Task<List<SupportModels.Trade>> GetRecentTradesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SupportModels.OrderResult> ModifyOrderAsync(string orderId, SupportModels.BrokerOrder newOrder)
        {
            throw new NotImplementedException();
        }

        public Task<SupportModels.OrderResult> PlaceComboOrderAsync(SupportModels.ComboOrder comboOrder)
        {
            throw new NotImplementedException();
        }

        public Task<SupportModels.OrderResult> PlaceOrderAsync(SupportModels.BrokerOrder order)
        {
            throw new NotImplementedException();
        }

        public Task<string> PreviewOrderAsync(SupportModels.BrokerOrder order)
        {
            throw new NotImplementedException();
        }
    }
}
