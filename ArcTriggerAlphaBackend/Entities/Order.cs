using ArcTriggerAlphaBackend.Entities;

public class Order
{
    // Existing properties (unchanged)
    public Guid ID { get; set; }
    public string Name { get; set; }
    public string Condition { get; set; }
    public string Size { get; set; }
    public string Stop { get; set; }
    public string Notes { get; set; }
    public DateTime CreateDateTime { get; set; }
    public DateTime OrderDateTime { get; set; }

    // New fields (UI-aligned)
    public decimal TriggerPrice { get; private set; }
    public string OrderType { get; private set; }  // "Call Option" | "Put Option"
    public int StrikePrice { get; private set; }

    // Fixed navigation property (virtual + foreign key)
    public Guid StockId { get; private set; }
    public virtual Stock Stock { get; private set; }  // Critical: `virtual` for lazy-loading

    // Updated constructor
    public Order(
        string name, string condition, string size, string stop, DateTime orderTime,
        decimal triggerPrice, string orderType, int strikePrice, Guid stockId,
        string notes = null)
    {
        // Your existing logic...
        ID = Guid.NewGuid();
        Name = name;
        Condition = condition;
        Size = size;
        Stop = stop;
        Notes = notes;
        OrderDateTime = orderTime;
        CreateDateTime = DateTime.Now;

        // New fields
        TriggerPrice = triggerPrice;
        OrderType = orderType;
        StrikePrice = strikePrice;
        StockId = stockId;  // Set FK directly (avoids navigation issues)
    }

    public void SetNote(string note) => Notes = note;
}