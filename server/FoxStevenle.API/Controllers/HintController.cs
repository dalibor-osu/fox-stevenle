using System.Globalization;
using FoxStevenle.API.Constants;
using FoxStevenle.API.DatabaseServices;
using FoxStevenle.API.Types.OptionalResult;
using FoxStevenle.API.Utils;
using Microsoft.AspNetCore.Mvc;

namespace FoxStevenle.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HintController(ILogger<HintController> logger, DailyQuizDatabaseService dailyQuizDatabaseService)
    : ControllerBase(logger)
{
    /// <summary>
    /// Gets a hint MP3 file
    /// </summary>
    /// <param name="date">Date to get the hint for</param>
    /// <param name="songNumber">Number of a song to get the hint for</param>
    /// <param name="index">Index of the hint</param>
    /// <returns><see cref="IActionResult"/> with the actual MP3 or error</returns>
    [HttpGet("{date}/{songNumber:int}/{index:int}")]
    public async Task<IActionResult> GetHint([FromRoute] string date, [FromRoute] int songNumber, [FromRoute] int index)
    {
        if (!DateOnly.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var dateOnly))
        {
            return CreateActionResultResponse(new Optional<string>(new OptionalError
                { Message = "Invalid date format", Type = OptionalErrorType.BadRequest }));
        }

        if (dateOnly > DateOnlyHelper.GetCurrentDateOnly())
        {
            return NotFound();
        }

        if (index is < 0 or > 2)
        {
            return CreateActionResultResponse(new Optional<string>(new OptionalError
                { Message = "Order can be in range from 0 to 2", Type = OptionalErrorType.BadRequest }));
        }

        if (songNumber is < 1 or > 5)
        {
            return CreateActionResultResponse(new Optional<string>(new OptionalError
                { Message = "Song number can be in range from 1 to 5", Type = OptionalErrorType.BadRequest }));
        }

        var quiz = await dailyQuizDatabaseService.GetByDateAsync(dateOnly);
        if (quiz == null)
        {
            return CreateActionResultResponse(new Optional<string>(new OptionalError
                { Message = "Quiz for this date was not found", Type = OptionalErrorType.NotFound }));
        }


        string relativeFilePath = $"{GeneralConstants.HintsDir}/{date}/{songNumber}/{index}.mp3";
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), relativeFilePath);
        if (!System.IO.File.Exists(filePath))
        {
            return CreateActionResultResponse(new Optional<string>(new OptionalError
                { Message = "File not found", Type = OptionalErrorType.InternalServerError }));
        }

        return PhysicalFile(filePath, "audio/mp3");
    }
}