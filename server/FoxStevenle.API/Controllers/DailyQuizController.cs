using FoxStevenle.API.DatabaseServices;
using FoxStevenle.API.Enums;
using FoxStevenle.API.Models;
using FoxStevenle.API.Models.DataTransferObjects;
using FoxStevenle.API.Utils;
using Microsoft.AspNetCore.Mvc;

namespace FoxStevenle.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DailyQuizController(ILogger<DailyQuizController> logger, DailyQuizDatabaseService dailyQuizDatabaseService)
    : ControllerBase(logger)
{
    /// <summary>
    /// Checks if <see cref="DailyQuiz"/> exists for the specified date
    /// </summary>
    /// <param name="date">Date to check</param>
    /// <returns>Empty body. Information is provided by the status code</returns>
    [HttpGet("exists/{date}")]
    public async Task<ActionResult> ExistsByDate([FromRoute] string date)
    {
        var dateOnly = DateOnlyHelper.GetFromStringKey(date);
        if (dateOnly is null)
        {
            return BadRequest();
        }

        if (dateOnly > DateOnlyHelper.GetCurrentDateOnly())
        {
            return NotFound();
        }

        bool exists = await dailyQuizDatabaseService.ExistsByDateAsync(dateOnly.Value);
        // TODO: Return true/false instead of different status codes
        return exists ? Ok() : NotFound();
    }
}