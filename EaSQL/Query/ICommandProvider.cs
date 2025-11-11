using System.Data;

namespace EaSQL.Query
{
    /// <summary>
    /// Internal interface to provide the <see cref="SqlQueryStringHandler"/> with a database command.
    /// </summary>
    [Obsolete("Use IDbConnection extension method instead!")]
    public interface ICommandProvider
    {
        /// <summary>
        /// Creates a new database command.
        /// </summary>
        /// <returns>The created command.</returns>
        IDbCommand CreateCommand();
    }
}
