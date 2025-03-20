using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FoxStevenle.API.Types.Interfaces;
using static FoxStevenle.API.Constants.DatabaseConstants;

namespace FoxStevenle.API.Models;

[Table(SongTable.TableName)]
public class Song : IIdentifiable
{
    /// <inheritdoc cref="IIdentifiable.Id"/>
    [Column(IIdentifiable.ColumnName)]
    [Key]
    public int Id { get; set; }
    
    /// <summary>
    /// Title of the song
    /// </summary>
    [Column(SongTable.Title)]
    [MaxLength(64)]
    [Required]
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Authors of the song (always Fox Stevenson, sometimes someone else too)
    /// </summary>
    [Column(SongTable.Authors)]
    [MaxLength(128)]
    [Required]
    public string Authors { get; set; } = string.Empty;
    
    /// <summary>
    /// Suffix of the song ("Fox Stevenson Remix", "DnB Mix", ...)
    /// </summary>
    [Column(SongTable.Suffix)]
    [MaxLength(128)]
    public string Suffix { get; set; } = string.Empty;
    
    /// <summary>
    /// Link to the song (mostly Spotify or Soundcloud)
    /// </summary>
    [Column(SongTable.Url)]
    [MaxLength(256)]
    [Required]
    public string Url { get; set; } = string.Empty;
    
    /// <summary>
    /// Link to the song album cover
    /// </summary>
    [Column(SongTable.CoverUrl)]
    [MaxLength(256)]
    public string? CoverUrl { get; set; }
    
    /// <summary>
    /// Duration of the song in seconds
    /// </summary>
    [Column(SongTable.Duration)]
    [Required]
    public int Duration { get; set; }
    
    /// <summary>
    /// Name of the song MP3 file
    /// </summary>
    [Column(SongTable.FileName)]
    [MaxLength(128)]
    [Required]
    public string FileName { get; set; } = string.Empty;
}