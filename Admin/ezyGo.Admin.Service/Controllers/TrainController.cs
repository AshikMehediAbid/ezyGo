using ezyGo.Admin.Domain.Interfaces;
using ezyGo.Admin.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ezyGo.Admin.Service.Controllers;

[Route("api/admin/train")]
[ApiController]
public class TrainController : ControllerBase
{
    private readonly ITrainService _rainService;

    public TrainController(ITrainService trainService)
    {
        _rainService = trainService;
    }

    [HttpPost]
    [Route("add-station")]
    public async Task<IActionResult> Create([FromForm]Station station)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _rainService.Create(station);
            return Ok(station);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
