namespace StockMarker.Models
{
    public class StockInfo
    {
        public string Symbol { get; set; } = string.Empty;
        public decimal CurrentPrice { get; set; }
        public double OpenPrice { get; set; }
        public decimal ChangePercent { get; set; } // e.g. +2.3% or -1.5%
    }
}