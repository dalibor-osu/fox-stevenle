using System.Data;
using FoxStevenle.API.Constants;
using FoxStevenle.API.Models;
using FoxStevenle.API.Models.DataTransferObjects;
using FoxStevenle.API.Types.Interfaces;

namespace FoxStevenle.API.DatabaseServices;

public class QuizEntryDatabaseService(Func<IDbConnection> connectionFactory) : DatabaseServiceBase<QuizEntry>(connectionFactory, DatabaseConstants.QuizEntryTable.TableName)
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
}