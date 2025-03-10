using FoxStevenle.API.Enums;
using FoxStevenle.API.Models.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace FoxStevenle.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DailyQuizController(ILogger<DailyQuizController> logger) : ControllerBase
{
    [HttpGet]
    public ActionResult GetArchive()
    {
        throw new NotImplementedException();
    }
}