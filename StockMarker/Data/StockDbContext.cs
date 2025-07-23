


using Microsoft.EntityFrameworkCore;
using StockPricePredictionApp.Models;

public class StockDbContext : DbContext
{
    public StockDbContext(DbContextOptions<StockDbContext> options) : base(options) { }

    public DbSet<StockPrice> StockPrices { get; set; }
}