namespace FoxStevenle.API.Models.Database;

public class Song
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? CoverUrl { get; set; }
    public int Duration { get; set; }
}