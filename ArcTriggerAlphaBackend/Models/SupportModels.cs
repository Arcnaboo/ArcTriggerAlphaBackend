using System;
using System.Collections.Generic;

namespace ArcTriggerAlphaBackend.Models
{
    public class SupportModels
    {
        public SupportModels()
        {
            throw new NotImplementedException();
        }

        // ✅ Represents a general broker order (stocks or options)
        public class BrokerOrder
        {
            public string ConId { get; set; }
            public string Action { get; set; } // "BUY" or "SELL"
            public string OrderType { get; set; } // "MKT", "LMT", etc.
            public int Quantity { get; set; }
            public decimal? LimitPrice { get; set; }
            public string Tif { get; set; } = "DAY"; // "DAY", "GTC"
            public bool OutsideRth { get; set; } = false;
            public string AccountId { get; set; }
        }

        // ✅ Represents an options contract (call/put)
        public class OptionContract
        {
            public string Symbol { get; set; }
            public string ConId { get; set; }
            public decimal Strike { get; set; }
            public string Right { get; set; } // "C" or "P"
            public DateTime Expiry { get; set; }
            public string Exchange { get; set; }
        }

        // ✅ Basic snapshot of market data
        public class MarketSnapshot
        {
            public string SymbolOrConId { get; set; }
            public decimal? Last { get; set; }
            public decimal? Bid { get; set; }
            public decimal? Ask { get; set; }
            public decimal? ChangePercent { get; set; }
            public long? Volume { get; set; }
        }

        // ✅ Account summary (cash, margin, etc.)
        public class AccountSummary
        {
            public string AccountId { get; set; }
            public decimal NetLiquidation { get; set; }
            public decimal CashBalance { get; set; }
            public decimal BuyingPower { get; set; }
            public string Currency { get; set; }
        }

        // ✅ Portfolio position
        public class Holding
        {
            public string ConId { get; set; }
            public string Symbol { get; set; }
            public string AssetType { get; set; } // "STK", "OPT", etc.
            public int Quantity { get; set; }
            public decimal AverageCost { get; set; }
            public decimal MarketValue { get; set; }
            public decimal UnrealizedPnl { get; set; }
            public string Currency { get; set; }
        }

        // ✅ Trade history entry
        public class Trade
        {
            public string TradeId { get; set; }
            public string ConId { get; set; }
            public string Symbol { get; set; }
            public DateTime Time { get; set; }
            public string Action { get; set; } // "BUY"/"SELL"
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal Commission { get; set; }
            public string Exchange { get; set; }
        }

        // ✅ Standard order result wrapper
        public class OrderResult
        {
            public string OrderId { get; set; }
            public string Status { get; set; }
            public string Message { get; set; }
        }

        // ✅ Current status of any order
        public class OrderStatus
        {
            public string OrderId { get; set; }
            public string ConId { get; set; }
            public string Symbol { get; set; }
            public string Status { get; set; } // "Submitted", "Filled", etc.
            public int FilledQuantity { get; set; }
            public int RemainingQuantity { get; set; }
            public decimal AverageFillPrice { get; set; }
        }

        // ✅ Commission estimate for previewing orders
        public class BrokerCommissionEstimate
        {
            public decimal EstimatedCommission { get; set; }
            public decimal EstimatedTotalCost { get; set; }
            public decimal EstimatedMarginImpact { get; set; }
        }

        // ✅ Combo / multi-leg order
        public class ComboOrder
        {
            public List<BrokerOrder> Legs { get; set; } = new();
            public string AccountId { get; set; }
        }

        // 🔧 Option chain filter input (optional utility)
        public class OptionChainFilter
        {
            public string Symbol { get; set; }
            public DateTime? Expiry { get; set; }
            public decimal? StrikeMin { get; set; }
            public decimal? StrikeMax { get; set; }
            public string Right { get; set; } // "C", "P", or null for both
        }
        public class ContractInfo
        {
            public string Symbol { get; set; }
            public string ConId { get; set; }
            public string SecurityType { get; set; } // e.g., "STK", "OPT"
            public string Exchange { get; set; }
            public string Currency { get; set; }
            public int Multiplier { get; set; } // e.g., 100 for options
            public string PrimaryExchange { get; set; }
            public string CompanyName { get; set; }
            public DateTime? Expiry { get; set; } // optional for options
            public decimal? Strike { get; set; }  // optional for options
            public string Right { get; set; }     // "C" or "P"
        }
    }
}
