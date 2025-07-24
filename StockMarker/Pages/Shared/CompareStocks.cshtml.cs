using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockMarker.Models;
using StockMarker.Services;

namespace StockMarker.Pages
{
    public class CompareStocksModel : PageModel
    {
        private readonly MarketDataService _marketService;

        public CompareStocksModel(MarketDataService marketService)
        {
            _marketService = marketService;
        }

        [BindProperty]
        public string Symbols { get; set; }

        public List<StockInfo> Stocks { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            Stocks = new List<StockInfo>();
            if (!string.IsNullOrWhiteSpace(Symbols))
            {
                var symbolList = Symbols.Split(",", StringSplitOptions.RemoveEmptyEntries)
                                        .Select(s => s.Trim().ToUpper())
                                        .ToList();

                foreach (var symbol in symbolList)
                {
                    var info = await _marketService.GetLiveStockData(symbol);
                    
                    if (info != null)
                        Stocks.Add(info);
                }
            }
            return Page();
        }
    }
}
