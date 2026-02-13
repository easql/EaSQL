using System.Text;

namespace EaSQL.DbInit;

internal sealed class Table(string name)
{
    private string Name { get; } = name;

    public List<Column> Columns { get; } = [];

    public string CreateCommandText()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("CREATE TABLE ")
            .Append(Name)
            .Append(" (");
        foreach (Column column in Columns)
        {
            column.CreateColumn(sb);
        }
        sb.Remove(sb.Length - 1, 1);
        sb.Append(");");
        return sb.ToString();
    }
}