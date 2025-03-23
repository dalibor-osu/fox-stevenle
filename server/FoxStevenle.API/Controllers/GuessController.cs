using FoxStevenle.API.DatabaseServices;
using FoxStevenle.API.Enums;
using FoxStevenle.API.Models;
using FoxStevenle.API.Models.DataTransferObjects;
using FoxStevenle.API.Types.OptionalResult;
using FoxStevenle.API.Utils;
using Microsoft.AspNetCore.Mvc;

namespace FoxStevenle.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuessController(ILogger<GuessController> logger, QuizEntryDatabaseService quizEntryDatabaseService)
    : ControllerBase(logger)
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="guess"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Guess([FromBody] Guess guess)
    {
        if (string.IsNullOrWhiteSpace(guess.Text))
        {
            return BadRequest("Invalid guess text");
        }

        var guessDate = DateOnlyHelper.GetFromStringKey(guess.Date ?? string.Empty);
        if (guessDate is null)
        {
            return BadRequest("Invalid date");
        }

        if (guessDate > DateOnlyHelper.GetCurrentDateOnly())
        {
            return NotFound();
        }

        var quizEntry =
            await quizEntryDatabaseService.GetFullForDateAndSongNumber(guessDate.Value, guess.SongIndex + 1);
        if (quizEntry is null)
        {
            return NotFound();
        }

        var song = quizEntry.Song;
        if (song is null)
        {
            return CreateActionResultResponse(new Optional<GuessResponseDto>(new OptionalError
                { Message = "Failed to retrieve song", Type = OptionalErrorType.InternalServerError }));
        }

        bool success = guess.Text.Equals(quizEntry.Song!.Title, StringComparison.CurrentCultureIgnoreCase);
        return Ok(new GuessResponseDto
        {
            Result = success ? GuessResult.Success : GuessResult.Fail,
            Song = success || guess.GuessIndex >= 2
                ? new SongDto
                {
                    Title = song.Title,
                    Url = song.Url,
                    CoverUrl = song.CoverUrl,
                    Authors = song.Authors
                }
                : null,
        });
    }
}