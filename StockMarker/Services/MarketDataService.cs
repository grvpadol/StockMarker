using System.Text.Json;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

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
    }
}
