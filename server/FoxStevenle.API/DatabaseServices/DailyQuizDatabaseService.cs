using System.Data;
using Dapper;
using FoxStevenle.API.Constants;
using FoxStevenle.API.Models;

namespace FoxStevenle.API.DatabaseServices;

public class DailyQuizDatabaseService(Func<IDbConnection> connectionFactory) : DatabaseServiceBase<DailyQuiz>(connectionFactory, DatabaseConstants.DailyQuizTable.TableName)
{
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
}