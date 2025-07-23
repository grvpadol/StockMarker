using Microsoft.OpenApi.Models;
using StockMarker.Models;     // <-- make sure these namespaces match your folders
using StockMarker.Services;   // <--  "StockMarker" is your project root namespace

var builder = WebApplication.CreateBuilder(args);

/* ------------------------------------------------------------------
 * 1️⃣  CONFIGURATION + DEPENDENCY INJECTION
 * ------------------------------------------------------------------*/


var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

var configuration = configBuilder.Build();


builder.Configuration.AddConfiguration(configuration);


// MongoDB configuration (reads the "MongoDB" section from appsettings.json)
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

// Your own service that talks to MongoDB
builder.Services.AddSingleton<StockPredictionService>();

// Web‑API controllers
builder.Services.AddHttpClient<MarketDataService>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Stock Prediction API",
        Version = "v1"
    });
});

// Razor Pages UI (leave it out if you don’t need a web UI yet)
builder.Services.AddRazorPages();

// Swagger / OpenAPI (handy for testing)
builder.Services.AddEndpointsApiExplorer();

/* ------------------------------------------------------------------
 * 2️⃣  BUILD THE APP
 * ------------------------------------------------------------------*/
var app = builder.Build();

/* ------------------------------------------------------------------
 * 3️⃣  HTTP REQUEST PIPELINE
 * ------------------------------------------------------------------*/
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();      // needed only if you serve static files / Razor assets

app.UseRouting();

app.UseAuthorization();

/* ------------------------------------------------------------------
 * 4️⃣  ENDPOINT MAPPING
 * ------------------------------------------------------------------*/
app.MapControllers();      // ← **DON’T forget this!**  Enables /api/... routes
app.MapRazorPages();       // ← Remove if you dropped AddRazorPages()

app.Run();