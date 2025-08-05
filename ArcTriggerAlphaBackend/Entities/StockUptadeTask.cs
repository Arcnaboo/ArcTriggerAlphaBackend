namespace ArcTriggerAlphaBackend.Entities
{
    public class StockUpdateTask
    {
        public Guid Id { get; private set; }           // PK
        public Guid StockId { get; private set; }      // FK → Stock
        public bool IsActive { get; private set; }     // If this task is enabled
        public int DeltaSeconds { get; private set; }  // Interval in seconds between updates
        public DateTime LastUpdateTime { get; private set; }
        public DateTime CreateTime { get; private set; }

        private StockUpdateTask() { }

        public StockUpdateTask(Guid stockId, int deltaSeconds, bool isActive)
        {
            Id = Guid.NewGuid();
            StockId = stockId;
            DeltaSeconds = deltaSeconds;
            IsActive = isActive;
            LastUpdateTime = DateTime.MinValue;
            CreateTime = DateTime.UtcNow;
        }

        public void MarkUpdated()
        {
            LastUpdateTime = DateTime.UtcNow;
        }

        public void SetActive(bool active)
        {
            IsActive = active;
        }

        public void SetDelta(int seconds)
        {
            DeltaSeconds = seconds;
        }
    }
}
