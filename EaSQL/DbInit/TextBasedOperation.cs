using System.Data.Common;

namespace EaSQL.DbInit;

internal sealed class TextBasedOperation(string command) : IOperation
{
    public async Task Execute(DbConnection connection,  CancellationToken token = default)
    {
        DbCommand dbCommand = connection.CreateCommand();
        dbCommand.CommandText = command;

        await dbCommand.ExecuteNonQueryAsync(token);
    }
}