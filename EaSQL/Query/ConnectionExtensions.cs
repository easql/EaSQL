using System.Data;
using System.Runtime.CompilerServices;

namespace EaSQL.Query
{
    public static class ConnectionExtensions
    {
        public static IDataReader RunQuery(
            this IDbConnection connection, 
            [InterpolatedStringHandlerArgument(nameof(connection))] SqlQueryStringHandler stringHandler)
        {
            IDbCommand command = stringHandler.GetCommand();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return command.ExecuteReader();
        }
    }
}
