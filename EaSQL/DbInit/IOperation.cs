using System.Data.Common;

namespace EaSQL.DbInit;

internal interface IOperation
{
    Task Execute(DbConnection connection, CancellationToken token = default);
    
    public static IOperation CreateFrom(string commandText)
    {
        return new TextBasedOperation(commandText);
    }
    
    public static IOperation CreateFrom(DbCommand command)
    {
        return new CommandBasedOperation(command);
    }
}

internal static class OperationExtensions
{
    public static IOperation IfNot(this IOperation operation, DbCommand guardQuery)
    {
        return new ConditionalOperation(guardQuery, operation);
    }
}