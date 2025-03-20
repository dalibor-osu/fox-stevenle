using System.Data;
using Dapper;
using FoxStevenle.API.Models;
using FoxStevenle.API.Types.Interfaces;
using static FoxStevenle.API.Constants.DatabaseConstants;

namespace FoxStevenle.API.DatabaseServices;

public class QuizEntryDatabaseService(Func<IDbConnection> connectionFactory)
    : DatabaseServiceBase<QuizEntry>(connectionFactory, QuizEntryTable.TableName)
{
    /// <summary>
    /// Adds a new <see cref="QuizEntry"/> to the database
    /// </summary>
    /// <param name="quizEntry"><see cref="QuizEntry"/> to add</param>
    /// <returns>ID of the new record</returns>
    public async Task<int> AddAsync(QuizEntry quizEntry)
    {
        const string query =
            $"""
                 INSERT INTO {DatabaseName}.{QuizEntryTable.TableName} (
                    {QuizEntryTable.QuizId},
                    {QuizEntryTable.SongId},
                    {QuizEntryTable.SongNumber}
                 )
                 VALUES (
                    @quizId,
                    @songId,
                    @songNumber
                 )
                 RETURNING {IIdentifiable.ColumnName};
             """;

        using var connection = ConnectionFactory();
        return await connection.QueryFirstAsync<int>(query, quizEntry);
    }

    /// <summary>
    /// Gets a full (with filled <see cref="Song"/> and <see cref="DailyQuiz"/>) <see cref="QuizEntry"/>  by date and song number
    /// </summary>
    /// <param name="date">Date to get the <see cref="QuizEntry"/> for</param>
    /// <param name="songNumber">Number of a song to get the <see cref="QuizEntry"/> for</param>
    /// <returns>Retrieved <see cref="QuizEntry"/> or null</returns>
    public async Task<QuizEntry?> GetFullForDateAndSongNumber(DateOnly date, int songNumber)
    {
        const string query =
            $"""
                SELECT
                    {QuizEntryTable.Prefix}.*,
                    {DailyQuizTable.Prefix}.*,
                    {SongTable.Prefix}.*
                FROM {QuizEntryTable.TableName} as {QuizEntryTable.Prefix}
                LEFT JOIN {DailyQuizTable.TableName} as {DailyQuizTable.Prefix}
                    ON {QuizEntryTable.Prefix}.{QuizEntryTable.QuizId} = {DailyQuizTable.Prefix}.{IIdentifiable.ColumnName}
                LEFT JOIN {SongTable.TableName} as {SongTable.Prefix}
                    ON {QuizEntryTable.Prefix}.{QuizEntryTable.SongId} = {SongTable.Prefix}.{IIdentifiable.ColumnName}
                WHERE {DailyQuizTable.Prefix}.{DailyQuizTable.Date} = @date
                    AND {QuizEntryTable.Prefix}.{QuizEntryTable.SongNumber} = @songNumber;
             """;

        using var connection = ConnectionFactory();
        var result = new Dictionary<int, QuizEntry>();
        await connection.QueryAsync<QuizEntry, DailyQuiz, Song, QuizEntry>(query, (qe, dq, s) =>
        {
            if (!result.TryGetValue(qe.QuizId, out var quizEntry))
            {
                quizEntry = qe;
                result.Add(qe.QuizId, qe);
            }
            quizEntry.Quiz = dq;
            quizEntry.Song = s;
            return quizEntry;
        }, new { date, songNumber });

        return result.Count > 0 ? result.Values.First() : null;
    }
}