using System.Data;
using System.Runtime.CompilerServices;

namespace EaSQL.Query
{
    /// <summary>
    /// Extension methods for <see cref="IDbConnection"/> allowing to run interpolated SQL queries.
    /// </summary>
    public static class ConnectionExtensions
    {
        /// <summary>
        /// Run a query via a connection.
        /// </summary>
        /// <param name="connection">The connection to the database</param>
        /// <param name="stringHandler">The query to execute</param>
        /// <returns>An IDataReader with the query result(s)</returns>
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

        /// <summary>
        /// Run a command via a connection.
        /// </summary>
        /// <param name="connection">The connection to the database</param>
        /// <param name="stringHandler">The command to execute</param>
        /// <returns>The number of effected rows by the command.</returns>
        public static int RunCommand(
            this IDbConnection connection,
            [InterpolatedStringHandlerArgument(nameof(connection))] SqlQueryStringHandler stringHandler)
        {
            IDbCommand command = stringHandler.GetCommand();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Run a query against the database. Expects the query to return exactly one value.
        /// </summary>
        /// <param name="connection">The connection to the database</param>
        /// <param name="stringHandler">The query to execute</param>
        /// <returns>Query result value.</returns>
        public static object? GetSingleValue(
            this IDbConnection connection,
            [InterpolatedStringHandlerArgument(nameof(connection))] SqlQueryStringHandler stringHandler)
        {
            IDbCommand command = stringHandler.GetCommand();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return command.ExecuteScalar();
        }
    }
}
