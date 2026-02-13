namespace EaSQL.DbInit;

/// <summary>
/// Defines different data types for database columns
/// </summary>
public enum ColumnType
{
    /// <summary>
    /// Fixed length string
    /// </summary>
    Char,
    /// <summary>
    /// Variable length string
    /// </summary>
    Varchar,
    /// <summary>
    /// Integer value
    /// </summary>
    Int,
    /// <summary>
    /// Floating point value
    /// </summary>
    Decimal,
    /// <summary>
    /// Boolean value
    /// </summary>
    Bit,
    /// <summary>
    /// Variable length binary data
    /// </summary>
    Varbinary
}