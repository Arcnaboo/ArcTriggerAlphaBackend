namespace ArcTriggerAlphaBackend.Entities
{
    public class TradeSetup
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }

        public string Ticker { get; private set; } = default!;
        public decimal TriggerPrice { get; private set; }
        public int Strike { get; private set; }
        // Rename for UI consistency (if you agree):  
        public string OrderType { get; private set; }  // Was `CallPut`  
        public DateTime ExpiryDate { get; private set; }
        public decimal LimitOffset { get; private set; }
        public decimal PositionSize { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        // EF Core private ctor
        private TradeSetup() { }

        // Public constructor for controlled instantiation
        public TradeSetup(
            Guid userId,
            string ticker,
            decimal triggerPrice,
            int strike,
            string otype,
            DateTime expiryDate,
            decimal limitOffset,
            decimal positionSize)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Ticker = ticker;
            TriggerPrice = triggerPrice;
            Strike = strike;
            OrderType = otype;
            ExpiryDate = expiryDate;
            LimitOffset = limitOffset;
            PositionSize = positionSize;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
