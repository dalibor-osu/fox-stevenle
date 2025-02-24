namespace FoxStevenle.API.Models.Database;

public class DailyQuiz
{
    public int Id { get; set; }
    public DateTime Date { get; set; }

    public List<QuizEntry> Entries { get; set; } = [];
}