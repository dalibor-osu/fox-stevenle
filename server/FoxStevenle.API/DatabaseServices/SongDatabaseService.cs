using System.Data;
using FoxStevenle.API.Constants;
using FoxStevenle.API.Models;

namespace FoxStevenle.API.DatabaseServices;

public class SongDatabaseService(Func<IDbConnection> connectionFactory) : DatabaseServiceBase<Song>(connectionFactory, DatabaseConstants.SongTable.TableName)
{
    
}