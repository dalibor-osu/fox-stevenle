namespace FoxStevenle.API.Models.DataTransferObjects;

/// <summary>
/// DTO that represents a play guess data
/// </summary>
public class Guess
{
    /// <summary>
    /// Text of the guess
    /// </summary>
    public string Text { get; set; } = string.Empty;
    
    /// <summary>
    /// Date of quiz
    /// </summary>
    public string? Date { get; set; }
    
    /// <summary>
    /// Index of the song
    /// </summary>
    public int SongIndex { get; set; }
    
    /// <summary>
    /// Index of the guess
    /// </summary>
    public int GuessIndex { get; set; }
}