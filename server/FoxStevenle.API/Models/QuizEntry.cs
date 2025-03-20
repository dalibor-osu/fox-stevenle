using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FoxStevenle.API.Types.Interfaces;
using static FoxStevenle.API.Constants.DatabaseConstants;

namespace FoxStevenle.API.Models;

[Table(QuizEntryTable.TableName)]
public class QuizEntry : IIdentifiable
{
    /// <inheritdoc cref="IIdentifiable.Id"/>
    [Column(IIdentifiable.ColumnName)]
    [Key]
    public int Id { get; set; }
    
    /// <summary>
    /// Number of the song in quiz
    /// </summary>
    [Column(QuizEntryTable.SongNumber)]
    [Required]
    public short SongNumber { get; set; }
    
    /// <summary>
    /// ID of a related <see cref="Song"/>
    /// </summary>
    [Column(QuizEntryTable.SongId)]
    [Required]
    public int SongId { get; set; }
    
    /// <summary>
    /// Instance of the related <see cref="Song"/>
    /// </summary>
    public Song? Song { get; set; }
    
    /// <summary>
    /// ID of the related <see cref="DailyQuiz"/>
    /// </summary>
    [Column(QuizEntryTable.QuizId)]
    [Required]
    public int QuizId { get; set; }
    
    /// <summary>
    /// Instance of the related <see cref="DailyQuiz"/>
    /// </summary>
    public DailyQuiz? Quiz { get; set; }
}