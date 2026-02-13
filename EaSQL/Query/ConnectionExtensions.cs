using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

// do not convert to extension block because it breaks capturing the connection argument
// ReSharper disable ConvertToExtensionBlock

// disable warnings that connection argument is only name captured. this is necessary for SqlQueryStringHandler!
// ReSharper disable EntityNameCapturedOnly.Global

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
            this DbConnection connection,
            [InterpolatedStringHandlerArgument(nameof(connection))]
            SqlQueryStringHandler stringHandler)
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
            this DbConnection connection,
            [InterpolatedStringHandlerArgument(nameof(connection))]
            SqlQueryStringHandler stringHandler)
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
            this DbConnection connection,
            [InterpolatedStringHandlerArgument(nameof(connection))]
            SqlQueryStringHandler stringHandler)
        {
            IDbCommand command = stringHandler.GetCommand();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return command.ExecuteScalar();
        }

        /// <summary>
        /// Run a query via a connection.
        /// </summary>
        /// <param name="connection">The connection to the database</param>
        /// <param name="stringHandler">The query to execute</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>An IDataReader with the query result(s)</returns>
        public static Task<IDataReader> RunQueryAsync(
            this DbConnection connection,
            [InterpolatedStringHandlerArgument(nameof(connection))]
            SqlQueryStringHandler stringHandler,
            CancellationToken cancellationToken = default)
        {
            DbCommand command = stringHandler.GetCommand();

            return InternalRunQueryAsync(connection, command, cancellationToken);
        }

        private static async Task<IDataReader> InternalRunQueryAsync(
            DbConnection connection, 
            DbCommand command,
            CancellationToken cancellationToken)
        {
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken);
            }

            return await command.ExecuteReaderAsync(cancellationToken);
        }

        /// <summary>
        /// Run a command via a connection.
        /// </summary>
        /// <param name="connection">The connection to the database</param>
        /// <param name="stringHandler">The command to execute</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The number of effected rows by the command.</returns>
        public static Task<int> RunCommandAsync(
            this DbConnection connection,
            [InterpolatedStringHandlerArgument(nameof(connection))]
            SqlQueryStringHandler stringHandler,
            CancellationToken cancellationToken = default)
        {
            DbCommand command = stringHandler.GetCommand();

            return InternalRunCommandAsync(connection, command, cancellationToken);
        }

        private static async Task<int> InternalRunCommandAsync(
            DbConnection connection,
            DbCommand command,
            CancellationToken cancellationToken)
        {
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken);
            }

            return await command.ExecuteNonQueryAsync(cancellationToken);
        }

        /// <summary>
        /// Run a query against the database. Expects the query to return exactly one value.
        /// </summary>
        /// <param name="connection">The connection to the database</param>
        /// <param name="stringHandler">The query to execute</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Query result value.</returns>
        public static Task<object?> GetSingleValueAsync(
            this DbConnection connection,
            [InterpolatedStringHandlerArgument(nameof(connection))]
            SqlQueryStringHandler stringHandler,
            CancellationToken cancellationToken = default)
        {
            DbCommand command = stringHandler.GetCommand();

            return connection.InternalGetSingleValueAsync(command, cancellationToken);
        }

        private static async Task<object?> InternalGetSingleValueAsync(
            this DbConnection connection,
            DbCommand command,
            CancellationToken cancellationToken)
        {
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken);
            }

            return await command.ExecuteScalarAsync(cancellationToken);
        }

        /// <summary>
        /// Creates a command that can be executed later on.
        /// </summary>
        /// <param name="connection">The connection to be used to create this command.</param>
        /// <param name="stringHandler">The query to execute</param>
        /// <returns>Created command object</returns>
        public static DbCommand CreateCommand(
            this DbConnection connection,
            [InterpolatedStringHandlerArgument(nameof(connection))]
            SqlQueryStringHandler stringHandler)
        {
            return stringHandler.GetCommand();
        }
    }
}