namespace FoxStevenle.API.Constants;

public static class DatabaseConstants
{
    public const string DatabaseName = "fox_stevenle";

    public static class DailyQuizTable
    {
        public const string TableName = "daily_quizzes";
        public const string Date = "date";
    }

    public static class QuizEntryTable
    {
        public const string TableName = "quiz_entries";
        public const string SongId = "song_id";
        public const string SongNumber = "song_number";
        public const string QuizId = "quiz_id";
    }

    public static class SongTable
    {
        public const string TableName = "songs";
        public const string Title = "title";
        public const string Url = "url";
        public const string CoverUrl = "cover_url";
        public const string Duration = "duration";
    }
}