using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace EaSQL.Query
{
    public sealed class SqlQueryHandler(IDbConnection connection) : ICommandProvider
    {
        private readonly IDbConnection _connection = connection;

        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Instance is needed for callback from SqlQueryStringHandler")]
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
