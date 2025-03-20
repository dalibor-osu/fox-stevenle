using FoxStevenle.API.Enums;

namespace FoxStevenle.API.Models.DataTransferObjects;

/// <summary>
/// Model representing a guess response to a player
/// </summary>
public class  GuessResponseDto
{
    /// <summary>
    /// Result type of the guess
    /// </summary>
    public GuessResult Result { get; set; }
    
    /// <summary>
    /// Info about the song if user guessed it correctly or failed the last attempt
    /// </summary>
    public SongDto? Song { get; set; }
}