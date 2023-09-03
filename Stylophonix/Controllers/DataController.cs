using Microsoft.AspNetCore.Mvc;
using Stylophonix.Interfaces.Controllers;
using Stylophonix.Messages;

namespace Stylophonix.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DataController : ControllerBase
{
    private readonly IDataControllerService _controllerService;

    public DataController(IDataControllerService controllerService)
    {
        _controllerService = controllerService;
    }

    [HttpGet("refresh")]
    public IActionResult RefreshData()
    {
        return _controllerService.RefreshData()
            ? Ok(Confirmations.Data.Refreshed)
            : BadRequest(Errors.Data.RefreshCooldown);
    }
}