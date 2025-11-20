using ezyGo.Admin.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ezyGo.Admin.Service.Controllers;

[Route("api/admin")]
[ApiController]
public class BusController : ControllerBase
{

    [HttpPost]
    [Route("create-vehicle")]
    public IActionResult Create([FromForm] Vehicle vehicle)
    {
        return Ok(vehicle);
    }
}
