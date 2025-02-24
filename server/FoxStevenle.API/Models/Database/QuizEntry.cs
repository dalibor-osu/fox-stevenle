namespace FoxStevenle.API.Models.Database;

public class QuizEntry
{
    public int Id { get; set; }
    public int SongId { get; set; }
    
    public int QuizId { get; set; }
    public DailyQuiz? Quiz { get; set; }
}