using System.Globalization;
using FoxStevenle.API.DatabaseServices;
using FoxStevenle.API.Types.OptionalResult;
using Microsoft.AspNetCore.Mvc;

namespace FoxStevenle.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HintController(ILogger<HintController> logger, DailyQuizDatabaseService dailyQuizDatabaseService) : ControllerBase
{
    #if DEBUG
        private const string HintsFolder = "../../hints";
    #else
        private const string HintsFolder = "/hints";
    #endif

    [HttpGet("{date}/{order:int}")]
    public async Task<IActionResult> GetHint([FromRoute] string date, [FromRoute] int order)
    {
        if (!DateOnly.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateOnly))
        {
            return CreateActionResultResponse(new Optional<string>(new OptionalError
                { Message = "Invalid date format", Type = OptionalErrorType.BadRequest }));
        }

        if (order is < 0 or > 2)
        {
            return CreateActionResultResponse(new Optional<string>(new OptionalError
                { Message = "Order can be in range from 0 to 2", Type = OptionalErrorType.BadRequest }));
        }
        
        var quiz = await dailyQuizDatabaseService.GetByDateAsync(dateOnly);
        if (quiz == null)
        {
            return CreateActionResultResponse(new Optional<string>(new OptionalError
                { Message = "Quiz for this date was not found", Type = OptionalErrorType.NotFound }));
        }

        
        string relativeFilePath = $"{HintsFolder}/{date}/{order}.mp3";
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), relativeFilePath);
        if (!System.IO.File.Exists(filePath))
        {
            return CreateActionResultResponse(new Optional<string>(new OptionalError
                { Message = "File not found", Type = OptionalErrorType.InternalServerError }));
        }

        return PhysicalFile(filePath, "audio/mp3");
    }
}