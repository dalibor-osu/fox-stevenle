namespace FoxStevenle.API.Models.DataTransferObjects;

public class Guess
{
    public string Text { get; set; } = string.Empty;
    public string? Date { get; set; }
    public int SongIndex { get; set; }
    public int GuessIndex { get; set; }
}