using System.Data.Common;

namespace EaSQL.DbInit;

internal sealed class ConditionalOperation(DbCommand guardQuery, IOperation operation) : IOperation
{
    public async Task Execute(DbConnection connection,  CancellationToken token = default)
    {
        guardQuery.Connection = connection;
        if (!await (await guardQuery.ExecuteReaderAsync(token)).ReadAsync(token))
        {
            await operation.Execute(connection, token);
        }
    }
}