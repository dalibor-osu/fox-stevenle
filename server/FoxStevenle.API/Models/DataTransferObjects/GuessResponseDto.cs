using FoxStevenle.API.Enums;

namespace FoxStevenle.API.Models.DataTransferObjects;

public class GuessResponseDto
{
    public GuessResult Result { get; set; }
    public SongDto? Song { get; set; }
}