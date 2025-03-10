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
    
    [Column(QuizEntryTable.SongNumber)]
    [Required]
    public short SongNumber { get; set; }
    
    [Column(QuizEntryTable.SongId)]
    [Required]
    public int SongId { get; set; }
    
    public Song? Song { get; set; }
    
    [Column(QuizEntryTable.QuizId)]
    [Required]
    public int QuizId { get; set; }
    
    public DailyQuiz? Quiz { get; set; }
}