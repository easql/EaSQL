using System.Data.Common;

namespace EaSQL.DbInit;

/// <summary>
/// Defines operations for a new version.
/// </summary>
public sealed class VersionSetup
{
    private readonly Version _version;

    internal VersionSetup(Version version)
    {
        _version = version;
    }

    /// <summary>
    /// Create a new table.
    /// </summary>
    /// <param name="tableName">The new table's name</param>
    /// <param name="tableSetup">The new table's definition</param>
    public void AddTable(string tableName, Action<TableOptions> tableSetup)
    {
        Table table = new(tableName);
        TableOptions options = new(table);
        tableSetup(options);

        _version.Operations.Add(IOperation.CreateFrom(table.CreateCommandText()));
    }

    /// <summary>
    /// Allows running an arbitrary command on the database. The command will not be executed as query.
    /// </summary>
    /// <param name="command">Command to execute.</param>
    public void AddExecutableCommand(string command)
    {
        _version.Operations.Add(IOperation.CreateFrom(command));
    }

    /// <summary>
    /// Allows running an arbitrary command on the database. The command will not be executed as query.
    /// </summary>
    /// <param name="command">DB command to execute.</param>
    public void AddExecutableCommand(DbCommand command)
    {
        _version.Operations.Add(IOperation.CreateFrom(command));
    }

    /// <summary>
    /// Allows running an arbitrary command on the database, unless a second command is yielding results.
    /// </summary>
    /// <param name="command">DB command to execute.</param>
    /// <param name="guardQuery">Query that is run before executing the <paramref name="command"/>.
    /// If the query yields any results, the <paramref name="command"/> will <strong>not</strong> be executed.</param>
    public void AddExecutableCommandUnless(DbCommand command, DbCommand guardQuery)
    {
        _version.Operations.Add(IOperation.CreateFrom(command).IfNot(guardQuery));
    }
}