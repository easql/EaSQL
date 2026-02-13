using System.Data.Common;

namespace EaSQL.DbInit;

internal sealed class CommandBasedOperation(DbCommand command) : IOperation
{
    public async Task Execute(DbConnection connection,  CancellationToken token = default)
    {
        command.Connection = connection;
        await command.ExecuteNonQueryAsync(token);
    }
}