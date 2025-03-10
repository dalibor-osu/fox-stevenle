using System.Data;
using Dapper;
using FoxStevenle.API.Constants;
using FoxStevenle.API.Types.Interfaces;

namespace FoxStevenle.API.DatabaseServices;

public abstract class DatabaseServiceBase<T>(Func<IDbConnection> connectionFactory, string tableName) where T : class
{
    protected Func<IDbConnection> ConnectionFactory { get; } = connectionFactory;

    protected async Task<T> GetAsync(Guid id)
    {
        string query =
            $"""
                 SELECT * FROM {DatabaseConstants.DatabaseName}.{tableName}
                     WHERE {IIdentifiable.ColumnName} = @id;
             """;

        using var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<T>(query, new { id });
    }

    protected async Task<bool> ExistsAsync(Guid id)
    {
        string query =
            $"""
                 SELECT EXISTS (
                     SELECT 1 FROM {DatabaseConstants.DatabaseName}.{tableName}
                         WHERE {IIdentifiable.ColumnName} = @id
                 );
             """;

        using var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<bool>(query, new { id });
    }
}