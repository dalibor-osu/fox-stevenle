namespace FoxStevenle.API.Models.DataTransferObjects;

/// <summary>
/// Info about the song
/// </summary>
public class SongDto
{
    /// <summary>
    /// Title of the song
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Artist of the song
    /// </summary>
    public string Artists { get; set; } = string.Empty;
    
    /// <summary>
    /// Link to the song (mostly Spotify or Soundcloud)
    /// </summary>
    public string Url { get; set; } = string.Empty;
    
    /// <summary>
    /// Link to the song album cover
    /// </summary>
    public string? CoverUrl { get; set; }
}