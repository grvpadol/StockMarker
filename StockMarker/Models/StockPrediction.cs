using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StockMarker.Models
{
    public class StockPrediction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }


        [BsonElement("StockSymbol")]
        public string StockSymbol { get; set; }

        [BsonElement("symbol")]
        public string Symbol { get; set; } = string.Empty;

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("predictedPrice")]
        public double PredictedPrice { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }

        [BsonElement("confidence")]
        public double Confidence { get; set; }
    }
}
