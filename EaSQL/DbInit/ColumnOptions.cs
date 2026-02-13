namespace EaSQL.DbInit;

/// <summary>
/// Defines options for  a new column
/// </summary>
public class ColumnOptions
{
    private readonly Column _column;
    
    internal ColumnOptions(Column column)
    {
        _column = column;        
    }
    
    /// <summary>
    /// Determines the column's type
    /// </summary>
    /// <param name="columnType">Column type</param>
    /// <returns>This instance to allow method chaining.</returns>
    public ColumnOptions HasType(ColumnType columnType)
    {
        _column.Type = columnType;

        return this;
    }

    /// <summary>
    /// Defines the column's length. Especially for character columns.
    /// </summary>
    /// <param name="length">Column length</param>
    /// <returns>This instance to allow method chaining.</returns>
    public ColumnOptions WithLength(int length)
    {
        _column.Length = length;

        return this;
    }

    /// <summary>
    /// Defines the column's precision. Especially for floating point columns.
    /// </summary>
    /// <param name="precision">Overall number of digits.</param>
    /// <param name="scale">Number of decimals</param>
    /// <returns>This instance to allow method chaining.</returns>
    public ColumnOptions WithPrecision(int precision, int scale)
    {
        _column.Precision = precision;
        _column.Scale = scale;

        return this;
    }

    /// <summary>
    /// Defines the column as mandatory.
    /// </summary>
    /// <returns>This instance to allow method chaining.</returns>
    public ColumnOptions IsNotNull()
    {
        _column.NotNull = true;

        return this;
    }

    /// <summary>
    /// Defines the column to have unique values.
    /// </summary>
    /// <returns>This instance to allow method chaining.</returns>
    public ColumnOptions IsUnique()
    {
        _column.Unique = true;

        return this;
    }

    /// <summary>
    /// Defines the column as the table's primary key.
    /// </summary>
    /// <returns>This instance to allow method chaining.</returns>
    public ColumnOptions AsPrimaryKey()
    {
        _column.IsPrimaryKey = true;

        return this;
    }

    /// <summary>
    /// Establishes a foreign key relationship to another table's column.
    /// </summary>
    /// <param name="table">Referenced table</param>
    /// <param name="column">Referenced column</param>
    /// <returns>This instance to allow method chaining.</returns>
    public ColumnOptions HasReferenceTo(string table, string column)
    {
        _column.Reference = new(table, column);

        return this;
    }
}