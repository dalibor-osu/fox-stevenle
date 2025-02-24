using FoxStevenle.API.Enums;
using FoxStevenle.API.Models.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace FoxStevenle.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuessController(ILogger<GuessController> logger) : ControllerBase
{
    [HttpPost]
    public ActionResult<GuessResponseDto> Guess([FromBody] GuessDto guess)
    {
        if (string.IsNullOrWhiteSpace(guess.Text))
        {
            return BadRequest();
        }
        
        bool success = guess.Text.Equals("Test", StringComparison.CurrentCultureIgnoreCase);
        return Ok(new GuessResponseDto
        {
            Result = success ? GuessResult.Success : GuessResult.Fail,
            Song = success
                ? new SongDto
                {
                    Title = "That Choice",
                    Url = "https://open.spotify.com/track/0ksr2OxjHuuFXFS2pn7lTa?si=73985140255e4d27"
                }
                : null,
        });
    }
}