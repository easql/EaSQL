using System.Data;
using System.Runtime.CompilerServices;

namespace EaSQL.Query
{
    public sealed class SqlQueryHandler(IDbConnection connection) : ICommandProvider
    {
        private readonly IDbConnection _connection = connection;

        public IDbCommand CreateCommand([InterpolatedStringHandlerArgument("")] SqlQueryStringHandler interpolatedQuery)
        {
            return interpolatedQuery.GetCommand();
        }

        public IDbCommand CreateCommand()
        {
            return _connection.CreateCommand();
        }
    }
}
