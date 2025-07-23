using Microsoft.AspNetCore.Mvc;
using StockMarker.Models;
using StockMarker.Services;


namespace StockMarker.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly StockPredictionService _service;

        public StockController(StockPredictionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<StockPrediction>>> Get() =>
            await _service.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<StockPrediction>> Get(string id)
        {
            var prediction = await _service.GetAsync(id);

            if (prediction is null)
                return NotFound();

            return prediction;
        }

        [HttpPost]
        public async Task<IActionResult> Post(StockPrediction prediction)
        {
            await _service.CreateAsync(prediction);
            return CreatedAtAction(nameof(Get), new { id = prediction.Id }, prediction);
        }
    }
}
