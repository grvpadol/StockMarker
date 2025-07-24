using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockMarker.Services;
using StockMarker.Models;

public class IndexModel : PageModel
{
    private readonly StockPredictionService _stockService;
    private readonly MarketDataService _marketService;

    public IndexModel(StockPredictionService stockService, MarketDataService marketService)
    {
        _stockService = stockService;
        _marketService = marketService;
    }

    [BindProperty]
    public string Symbol { get; set; }

    public decimal? LatestPrice { get; set; }
    public double? PredictedPrice { get; set; }
    public string? ResponseMessage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrEmpty(Symbol))
        {
            ResponseMessage = "Please enter a valid stock symbol.";
            return Page();
        }

        var latestPrice = await _marketService.GetLatestPriceAsync(Symbol);
        if (latestPrice == null)
        {
            ResponseMessage = "Could not fetch latest price.";
            return Page();
        }

        LatestPrice = latestPrice.Value;
        PredictedPrice = Math.Round((double)(latestPrice.Value * 1.05m), 2);

        await _stockService.InsertCurrentPriceAsync(Symbol, latestPrice.Value);

        ResponseMessage = "Prediction saved successfully!";
        return Page();
    }


    public List<StockInfo> TopGainers { get; set; } = new();
    public List<StockInfo> TopLosers { get; set; } = new();
    public List<StockInfo> AllStocks { get; set; } = new();

    public async Task OnGetAsync()
    {
        var allStocks = await _marketService.GetLiveStockData();

        AllStocks = allStocks.OrderByDescending(x => Math.Abs(x.ChangePercent)).ToList();
        TopGainers = allStocks.Where(x => x.ChangePercent > 0)
                              .OrderByDescending(x => x.ChangePercent).Take(5).ToList();

        TopLosers = allStocks.Where(x => x.ChangePercent < 0)
                             .OrderBy(x => x.ChangePercent).Take(5).ToList();
    }

}