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
    
    [Column(SongTable.Title)]
    [MaxLength(64)]
    [Required]
    public string Title { get; set; } = string.Empty;
    
    [Column(SongTable.Authors)]
    [MaxLength(128)]
    [Required]
    public string Authors { get; set; } = string.Empty;
    
    [Column(SongTable.Suffix)]
    [MaxLength(128)]
    public string Suffix { get; set; } = string.Empty;
    
    [Column(SongTable.Url)]
    [MaxLength(256)]
    [Required]
    public string Url { get; set; } = string.Empty;
    
    [Column(SongTable.CoverUrl)]
    [MaxLength(256)]
    public string? CoverUrl { get; set; }
    
    [Column(SongTable.Duration)]
    [Required]
    public int Duration { get; set; }
}