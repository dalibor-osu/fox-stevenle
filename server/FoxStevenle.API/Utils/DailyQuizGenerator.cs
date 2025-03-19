using FFMpegCore;
using FFMpegCore.Arguments;
using FoxStevenle.API.Constants;
using FoxStevenle.API.DatabaseServices;
using FoxStevenle.API.Extensions;
using FoxStevenle.API.Models;

namespace FoxStevenle.API.Utils;

public class DailyQuizGenerator(
    ILogger<DailyQuizGenerator> logger,
    SongDatabaseService songDatabaseService,
    DailyQuizDatabaseService dailyQuizDatabaseService,
    QuizEntryDatabaseService quizEntryDatabaseService)
{
    public async Task GenerateForDate(DateOnly date)
    {
        bool exists = await dailyQuizDatabaseService.GetByDateAsync(date) != null;
        if (exists)
        {
            logger.LogWarning("Daily Quiz for date {Date} already exists. Skipping...", date);
            return;
        }

        var quiz = new DailyQuiz { Date = date };
        quiz.Id = await dailyQuizDatabaseService.InsertAsync(quiz);

        int songCount = await songDatabaseService.CountAllAsync();

        var quizEntries = new QuizEntry[GeneralConstants.SongCountPerDay];
        for (int i = 0; i < GeneralConstants.SongCountPerDay; i++)
        {
            var random = new Random();
            int randomId;
            do
            {
                randomId = random.Next(1, songCount + 1); // Song IDs are 1 based
            } while (quizEntries.Any(q => (q?.Song?.Id ?? -1) == randomId));

            var song = await songDatabaseService.GetAsync(randomId);
            quizEntries[i] = new QuizEntry
            {
                QuizId = quiz.Id,
                Quiz = quiz,
                SongId = song.Id,
                Song = song,
                SongNumber = (short)(i + 1)
            };
        }

        string dateKey = date.GetDateKey();
        string hitsFolder = Path.Join(GeneralConstants.HintsDir, dateKey);
        var dayDirInfo = Directory.CreateDirectory(hitsFolder);
        for (int i = 0; i < GeneralConstants.SongCountPerDay; i++)
        {
            _ = dayDirInfo.CreateSubdirectory(quizEntries[i].SongNumber.ToString());
            await GenerateFilesForEntry(quizEntries[i], logger);
        }

        foreach (var quizEntry in quizEntries)
        {
            await quizEntryDatabaseService.InsertAsync(quizEntry);
        }
    }

    private static async Task GenerateFilesForEntry(QuizEntry quizEntry, ILogger<DailyQuizGenerator> logger)
    {
        if (quizEntry.Song == null)
        {
            logger.LogCritical("Song is null");
            return;
        }

        if (quizEntry.Quiz == null)
        {
            logger.LogCritical("Quiz is null");
            return;
        }

        var song = quizEntry.Song;
        string sourceSongPath = Path.Join(GeneralConstants.SongsDir, song.FileName);
        string firstHintOutputPath = GetHintPath(quizEntry, 0);
        string secondHintOutputPath = GetHintPath(quizEntry, 1);
        string thirdHintOutputPath = GetHintPath(quizEntry, 2);

        var random = new Random();
        int paddingSeconds = (int)(song.Duration * 0.1);
        int firstHintStartSeconds = random.Next(paddingSeconds, song.Duration - paddingSeconds);
        int secondHintStartSeconds = random.Next(paddingSeconds, song.Duration - paddingSeconds);

        bool firstHintResult = await GenerateHint(sourceSongPath, firstHintOutputPath, firstHintStartSeconds,
            GeneralConstants.FirstHintLengthMillis, logger);
        if (!firstHintResult)
        {
            logger.LogCritical(
                "Failed to generate first hint. Source song: {SourceSong}, Output Path: {OutputPath}, StartSeconds: {StartSeconds}",
                sourceSongPath, firstHintOutputPath, firstHintStartSeconds);
        }

        bool secondHintResult = await GenerateHint(sourceSongPath, secondHintOutputPath, secondHintStartSeconds,
            GeneralConstants.SecondHintLengthMillis, logger);
        if (!secondHintResult)
        {
            logger.LogCritical(
                "Failed to generate second hint. Source song: {SourceSong}, Output Path: {OutputPath}, StartSeconds: {StartSeconds}",
                sourceSongPath, secondHintOutputPath, secondHintStartSeconds);
        }

        bool thirdHintResult =
            await GenerateHint(sourceSongPath, thirdHintOutputPath, 0, GeneralConstants.ThirdHintLengthMillis, logger);
        if (!thirdHintResult)
        {
            logger.LogCritical(
                "Failed to generate third hint. Source song: {SourceSong}, Output Path: {OutputPath}",
                sourceSongPath, firstHintOutputPath);
        }
    }

    private static async Task<bool> GenerateHint(string sourceSongPath, string outputPath, int startSeconds,
        int lengthMillis, ILogger<DailyQuizGenerator> logger)
    {
        try
        {
            return await FFMpegArguments
                .FromFileInput(sourceSongPath, false)
                .OutputToFile(outputPath, true, options => options
                        .WithArgument(new SeekArgument(TimeSpan.FromSeconds(startSeconds))) // Start time: 10 seconds
                        .WithArgument(
                            new DurationArgument(
                                TimeSpan.FromMilliseconds(lengthMillis))) // Duration: 10 seconds
                        .WithArgument(new AudioCodecArgument("copy")) // Avoid re-encoding
                ).ProcessAsynchronously();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to generate hint");
            return false;
        }
    }

    private static string GetHintPath(QuizEntry quizEntry, int hintIndex) =>
        Path.Join(
            GeneralConstants.HintsDir,
            quizEntry.Quiz!.Date.GetDateKey(),
            quizEntry.SongNumber.ToString(),
            $"{hintIndex}.mp3");
}