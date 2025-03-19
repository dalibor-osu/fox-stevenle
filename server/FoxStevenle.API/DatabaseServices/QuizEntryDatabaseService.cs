using System.Data;
using Dapper;
using FoxStevenle.API.Models;
using FoxStevenle.API.Types.Interfaces;
using static FoxStevenle.API.Constants.DatabaseConstants;

namespace FoxStevenle.API.DatabaseServices;

public class QuizEntryDatabaseService(Func<IDbConnection> connectionFactory)
    : DatabaseServiceBase<QuizEntry>(connectionFactory, QuizEntryTable.TableName)
{
//     public bool ExistsByGuess(Guess guess)
//     {
//         const string query =
//             $"""
//                  SELECT EXISTS (
//                      SELECT 1 FROM {DatabaseConstants.DatabaseName}.""
//                          WHERE {IIdentifiable.ColumnName} ilike @text,
//                  );
//              """;
//     }

    public async Task<int> InsertAsync(QuizEntry quizEntry)
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