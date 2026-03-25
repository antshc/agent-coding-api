using Api.SharedKernel.Payload;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using swebAngulas.ViewModels;

namespace swebAngulas.Controllers.v1;

[ApiController]
[Route("api/v1/weather-forecasts")]
public class WeatherForecastsController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    [SwaggerOperation(Summary = "Get weather  forecasts")]
    [HttpGet()]
    public async Task<ActionResult<ApiResponse<IEnumerator<WeatherForecast>>>> Get(CancellationToken cancellationToken)
    {
        return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index), TemperatureC = Random.Shared.Next(-20, 55), Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }));
    }
}
