namespace ArcTriggerAlphaBackend.Entities
{
    public class Stock
    {
        public Guid Id { get; private set; }

        public string Ticker { get; private set; } = default!;
        public decimal LastPrice { get; private set; }
        public decimal HighOfDay { get; private set; }
        public decimal PremarketHigh { get; private set; }
        public decimal CurrentVolume { get; private set; }
        public decimal AvgVolume30dAtTime { get; private set; }
        public decimal Rvol { get; private set; }

        public DateTime SnapshotTime { get; private set; }   // Market timestamp
        public DateTime LastUpdated { get; private set; }    // System update time

        private Stock() { }

        public Stock(string ticker,
                     decimal lastPrice,
                     decimal highOfDay,
                     decimal premarketHigh,
                     decimal currentVolume,
                     decimal avgVolume30dAtTime,
                     DateTime snapshotTime)
        {
            Id = Guid.NewGuid();
            Ticker = ticker;
            LastPrice = lastPrice;
            HighOfDay = highOfDay;
            PremarketHigh = premarketHigh;
            CurrentVolume = currentVolume;
            AvgVolume30dAtTime = avgVolume30dAtTime;
            Rvol = avgVolume30dAtTime == 0 ? 0 : currentVolume / avgVolume30dAtTime;
            SnapshotTime = snapshotTime;
            LastUpdated = DateTime.UtcNow;
        }

        public void UpdatePrice(decimal newPrice)
        {
            LastPrice = newPrice;
            LastUpdated = DateTime.UtcNow;
        }
    }
}
