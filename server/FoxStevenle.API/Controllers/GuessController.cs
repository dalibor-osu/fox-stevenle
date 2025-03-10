using FoxStevenle.API.Enums;
using FoxStevenle.API.Models.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace FoxStevenle.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuessController(ILogger<GuessController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<GuessResponseDto>> Guess([FromBody] Guess guess)
    {
        if (string.IsNullOrWhiteSpace(guess.Text))
        {
            return BadRequest();
        }
        
        bool success = guess.Text.Equals("Test", StringComparison.CurrentCultureIgnoreCase);
        return Ok(new GuessResponseDto
        {
            Result = success ? GuessResult.Success : GuessResult.Fail,
            Song = success || guess.GuessIndex >= 2
                ? new SongDto
                {
                    Title = "That Choice",
                    Url = "https://open.spotify.com/track/0ksr2OxjHuuFXFS2pn7lTa?si=73985140255e4d27"
                }
                : null,
        });
    }
}