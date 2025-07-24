using System.Text.Json;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using StockMarker.Models;

namespace StockMarker.Services
{
    public class MarketDataService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "CPY21PY8M97Y6VKN";
        private const string baseUrl = "https://www.alphavantage.co/query";

        public MarketDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal?> GetLatestPriceAsync(string symbol)
        {
            var url = $"{baseUrl}?function=GLOBAL_QUOTE&symbol={symbol}&apikey={_apiKey}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);

            if (doc.RootElement.TryGetProperty("Global Quote", out JsonElement quote))
            {
                if (quote.TryGetProperty("05. price", out JsonElement priceElement))
                {
                    if (decimal.TryParse(priceElement.GetString(), out var price))
                    {
                        return price;
                    }
                }
            }


            return null;
        }

        public async Task<List<StockInfo>> GetLiveStockData()
        {
            // ⚠️ You can replace this mock data with real API later
            var stocks = new List<StockInfo>
        {
            new StockInfo { Symbol = "AAPL", OpenPrice = 190.0, CurrentPrice = 195.5M, ChangePercent = 2.4M },
            new StockInfo { Symbol = "GOOG", OpenPrice = 190.0, CurrentPrice = 2810.7M, ChangePercent = -1.1M },
            new StockInfo { Symbol = "TSLA", OpenPrice = 190.0, CurrentPrice = 255.2M, ChangePercent = 3.8M },
            new StockInfo { Symbol = "MSFT", OpenPrice = 190.0, CurrentPrice = 365.9M, ChangePercent = 1.9M },
            new StockInfo { Symbol = "AMZN", OpenPrice = 190.0, CurrentPrice = 128.6M, ChangePercent = -2.5M },
            new StockInfo { Symbol = "NFLX", CurrentPrice = 439.2M, ChangePercent = 0.6M },
          
        };

            return await Task.FromResult(stocks);
        }
        public async Task<StockInfo?> GetLiveStockData(string symbol)
        {
            var allStocks = await GetLiveStockData(); // Call the mock data method
            return allStocks.FirstOrDefault(s => s.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));
        }
    }
}
