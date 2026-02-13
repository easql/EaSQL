namespace EaSQL.DbInit;

/// <summary>
/// Defines the setup of a new table.
/// </summary>
public class TableOptions
{
    private readonly Table _table;

    internal TableOptions(Table table)
    {
        _table = table;
    }

    /// <summary>
    /// Adds a column to the table.
    /// </summary>
    /// <param name="columnName">Column name</param>
    /// <param name="columnConfiguration">Column properties</param>
    public void AddColumn(string columnName, Action<ColumnOptions> columnConfiguration)
    {
        Column column = new(columnName);
        ColumnOptions options = new(column);
        columnConfiguration(options);
        
        _table.Columns.Add(column);
    }
}