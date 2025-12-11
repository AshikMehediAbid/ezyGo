using ezyGo.Platform.Domain.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ezyGo.Platform.Service.Controllers;

[Route("api/search")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;
    private readonly ILogger<SearchController> _logger;

    public SearchController(ISearchService searchService, ILogger<SearchController> logger)
    {
        _searchService = searchService;
        _logger = logger;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Search([FromQuery] string fromCity, [FromQuery] string toCity, [FromQuery] DateOnly date)
    {
        try
        {

            var trips = await _searchService.SearchTripsAsync(fromCity, toCity, date);
            _logger.LogInformation("Searched for trips from {FromCity} to {ToCity} on {Date}", fromCity, toCity, date);
            return Ok(trips);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,$"\n{ex.InnerException.Message}\n", "Error occurred while searching for trips from {FromCity} to {ToCity} on {Date}", fromCity, toCity, date);
            return StatusCode(500, "An error occurred while processing your request.");
        }

    }
}
