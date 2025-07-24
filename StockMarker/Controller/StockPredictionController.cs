using Microsoft.AspNetCore.Mvc;
using StockMarker.Models;
using StockMarker.Services;

namespace StockMarker.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockPredictionController : ControllerBase
    {
        private readonly StockPredictionService _service;
        private readonly MarketDataService _marketService;
        public StockPredictionController(StockPredictionService service, MarketDataService marketService)
        {
            _service = service;
            _marketService = marketService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Test success");
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<StockPrediction>>> GetAllPredictions()
        {
            return await _service.GetAllAsync();
        }


        [HttpGet("raw")]
        public async Task<IActionResult> GetAll()
        {
            var predictions = await _service.GetAllAsync();
            return Ok(predictions);
        }


        [HttpPost("insert/{symbol}")]
        public async Task<IActionResult> FetchAndStore(string symbol)
        {
            var latestPrice = await _marketService.GetLatestPriceAsync(symbol);

            if (latestPrice == null)
                return BadRequest("Could not fetch price from API.");

            var prediction = new StockPrediction
            {
                Symbol = symbol, // use the symbol from the route parameter
                Price = (double)latestPrice.Value,
                PredictedPrice = (double)latestPrice.Value * 1.05, // dummy prediction
                Timestamp = DateTime.UtcNow
            };

            await _service.InsertStockPriceAsync(prediction);
            return Ok(prediction);
        }

        [HttpGet("price/{symbol}")]
        public async Task<IActionResult> GetCurrentPrice(string symbol)
        {
            var price = await _marketService.GetLatestPriceAsync(symbol);

            if (price == null)
                return NotFound("Price not found for the given symbol.");

            return Ok(new { Symbol = symbol.ToUpper(), Price = price });
        }

        [HttpGet("top-movers")]
        public async Task<IActionResult> GetTopMovers()
        {
            var allStocks = await _marketService.GetLiveStockData(); // Implement this method next

            var topGainers = allStocks
                .OrderByDescending(s => s.ChangePercent)
                .Take(5)
                .ToList();

            var topLosers = allStocks
                .OrderBy(s => s.ChangePercent)
                .Take(5)
                .ToList();

            return Ok(new
            {
                Gainers = topGainers,
                Losers = topLosers
            });
        }

        [HttpGet("live")]
        public async Task<IActionResult> GetLiveScanning()
        {
            var allStocks = await _marketService.GetLiveStockData(); // Same helper method
            return Ok(allStocks);
        }

    }
}