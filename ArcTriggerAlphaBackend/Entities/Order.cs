namespace ArcTriggerAlphaBackend.Entities
{
    public class Order
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Condition { get; set; }
        private Order() { }
        public string Size { get; set; }

        public string Stop { get; set; }

        public string Notes { get; set; }
         public DateTime CreateDateTime { get; set; }
        public DateTime OrderDateTime { get; set; }


        public Order(string name, string condition, string size, string stop, DateTime orderTime,string notes=null)
        {
            ID = Guid.NewGuid();
            Name = name;
            Condition = condition;
            Size = size;
            Stop = stop;
            Notes = notes;
            OrderDateTime = orderTime;
            CreateDateTime = DateTime.Now;
        }

        public void SetNote(string note)
        {
            Notes = note;
        }
    }
}
