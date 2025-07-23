using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StockMarker.Models;



namespace StockMarker.Services
{
    public class StockPredictionService
    {
        
            private readonly IMongoCollection<StockPrediction> _predictionsCollection;
            private readonly IMongoCollection<StockPrediction> _stockCollection;
        public StockPredictionService(IOptions<MongoDBSettings> mongoSettings)
            {
                var mongoClient = new MongoClient(mongoSettings.Value.ConnectionString);
                var mongoDatabase = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
                _predictionsCollection = mongoDatabase.GetCollection<StockPrediction>(mongoSettings.Value.CollectionName);
            }

            public async Task<List<StockPrediction>> GetAsync() =>
                await _predictionsCollection.Find(_ => true).ToListAsync();

            public async Task<StockPrediction?> GetAsync(string id) =>
                await _predictionsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            public async Task CreateAsync(StockPrediction prediction) =>
                await _predictionsCollection.InsertOneAsync(prediction);

       
        public async Task<List<StockPrediction>> GetAllAsync()
            {
                 return await _stockCollection.Find(_ => true).ToListAsync();
            }
        public async Task InsertCurrentPriceAsync(string symbol, decimal price)
        {
            var prediction = new StockPrediction
            {
                StockSymbol = symbol,
                Date = DateTime.UtcNow,
                PredictedPrice = (double)price, // initially just store actual
                Confidence = 1.0
            };

            await _stockCollection.InsertOneAsync(prediction);
        }
        public async Task InsertStockPriceAsync(StockPrediction prediction)
        {
            await _stockCollection.InsertOneAsync(prediction);
        }

    }
 }