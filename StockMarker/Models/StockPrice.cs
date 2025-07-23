namespace StockMarker.Models
{
    public class StockPrice
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public float Open { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Close { get; set; }
        public long Volume { get; set; }
    }
}
