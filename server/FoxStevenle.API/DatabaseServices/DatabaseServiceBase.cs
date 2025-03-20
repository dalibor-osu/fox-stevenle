using System.Data;
using Dapper;
using FoxStevenle.API.Constants;
using FoxStevenle.API.Types.Interfaces;

namespace FoxStevenle.API.DatabaseServices;

public abstract class DatabaseServiceBase<T>(Func<IDbConnection> connectionFactory, string tableName) where T : class
{
    protected Func<IDbConnection> ConnectionFactory { get; } = connectionFactory;

    /// <summary>
    /// Gets the record from database by its ID
    /// </summary>
    /// <param name="id">ID of a record to get</param>
    /// <returns>Retrieved record or null</returns>
    public async Task<T?> GetAsync(int id)
    {
        string query =
            $"""
                 SELECT * FROM {DatabaseConstants.DatabaseName}.{tableName}
                     WHERE {IIdentifiable.ColumnName} = @id;
             """;

        using var connection = ConnectionFactory();
        return await connection.QuerySingleOrDefaultAsync<T?>(query, new { id });
    }

    /// <summary>
    /// Checks if record exists in the database by its ID
    /// </summary>
    /// <param name="id">ID of a record to check</param>
    /// <returns>true if it exists, false if not</returns>
    public async Task<bool> ExistsAsync(int id)
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

    /// <summary>
    /// Counts all rows in the table
    /// </summary>
    /// <returns>Number of rows as <see cref="int"/></returns>
    public async Task<int> CountAllAsync()
    {
        string query = $"SELECT COUNT(*) FROM {DatabaseConstants.DatabaseName}.{tableName};";
        using var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<int>(query);
    }
}