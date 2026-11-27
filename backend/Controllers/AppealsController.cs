using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppealsController : ControllerBase
{
    private readonly IAppealService _appealService;

    public AppealsController(IAppealService appealService)
    {
        _appealService = appealService;
    }

    /// <summary>
    /// Тестовая конечная точка каркаса контроллера (ЛР7).
    /// </summary>
    [HttpGet]
    public IActionResult GetStub()
    {
        return Ok(_appealService.GetStubStatus());
    }
}
