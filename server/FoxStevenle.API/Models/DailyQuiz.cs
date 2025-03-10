using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FoxStevenle.API.Types.Interfaces;
using static FoxStevenle.API.Constants.DatabaseConstants;

namespace FoxStevenle.API.Models;

[Table(DailyQuizTable.TableName)]
public class DailyQuiz : IIdentifiable
{
    /// <inheritdoc cref="IIdentifiable.Id"/>
    [Column(IIdentifiable.ColumnName)]
    [Key]
    public int Id { get; set; }
    
    [Column(DailyQuizTable.Date, TypeName = "date")]
    [Required]
    public DateOnly Date { get; set; }

    [NotMapped]
    public List<QuizEntry> Entries { get; set; } = [];
}