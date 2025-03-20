using System.Data;
using Dapper;
using FoxStevenle.API.Constants;
using FoxStevenle.API.Models;
using FoxStevenle.API.Types.Interfaces;

namespace FoxStevenle.API.DatabaseServices;

public class DailyQuizDatabaseService(Func<IDbConnection> connectionFactory) : DatabaseServiceBase<DailyQuiz>(connectionFactory, DatabaseConstants.DailyQuizTable.TableName)
{
    /// <summary>
    /// Gets the <see cref="DailyQuiz"/> by date
    /// </summary>
    /// <param name="date">Date to get the <see cref="DailyQuiz"/> for</param>
    /// <returns>Retrieved <see cref="DailyQuiz"/> or null</returns>
    public async Task<DailyQuiz?> GetByDateAsync(DateOnly date)
    {
        const string query = 
            $"""
                 SELECT * FROM {DatabaseConstants.DatabaseName}.{DatabaseConstants.DailyQuizTable.TableName}
                     WHERE {DatabaseConstants.DailyQuizTable.Date} = @date;
             """;
        
        using var connection = ConnectionFactory();
        return await connection.QueryFirstOrDefaultAsync<DailyQuiz?>(query, new { date });
    }

    /// <summary>
    /// Checks if <see cref="DailyQuiz"/> exists for the specified date
    /// </summary>
    /// <param name="date">Date to check the <see cref="DailyQuiz"/> for</param>
    /// <returns>true if <see cref="DailyQuiz"/> exists, false if not</returns>
    public async Task<bool> ExistsByDateAsync(DateOnly date) => await GetByDateAsync(date) != null;
    

    /// <summary>
    /// Adds a new <see cref="DailyQuiz"/> to the database
    /// </summary>
    /// <param name="dailyQuiz"><see cref="DailyQuiz"/> to add</param>
    /// <returns>ID of the new record</returns>
    public async Task<int> AddAsync(DailyQuiz dailyQuiz)
    {
        const string query = 
            $"""
                 INSERT INTO {DatabaseConstants.DatabaseName}.{DatabaseConstants.DailyQuizTable.TableName} (
                    {DatabaseConstants.DailyQuizTable.Date}
                 )
                 VALUES (@date)
                 RETURNING {IIdentifiable.ColumnName};
             """;
        
        using var connection = ConnectionFactory();
        return await connection.QueryFirstAsync<int>(query, dailyQuiz);
    }
}