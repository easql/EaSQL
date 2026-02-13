using System.Text;

namespace EaSQL.DbInit;

internal sealed class Column(string name)
{
    private string Name { get; } = name;
    public ColumnType Type { get; set; }
    public int Length { get; set; }
    public int Precision { get; set; }
    public int Scale { get; set; }
    public bool IsPrimaryKey { get; set; }
    public bool NotNull { get; set; }
    public bool Unique { get; set; }
    
    public Reference? Reference { get; set; }

    public void CreateColumn(StringBuilder sb)
    {
        sb.Append(Name)
            .Append(' ')
            .Append(Type.ToString().ToUpperInvariant());
        if (Length > 0)
        {
            sb.Append('(').Append(Length).Append(')');
        }

        if (Precision > 0)
        {
            sb.Append('(').Append(Precision);
            if (Scale > 0)
            {
                sb.Append(',').Append(Scale);
            }
            sb.Append(')');
        }

        if (NotNull)
        {
            sb.Append(" NOT NULL");
        }
        
        if (Unique)
        {
            sb.Append(" UNIQUE");
        }

        if (IsPrimaryKey)
        {
            sb.Append(" PRIMARY KEY");
        }

        if (Reference is not null)
        {
            sb.Append(" REFERENCES ")
                .Append(Reference.Table)
                .Append('.')
                .Append(Reference.Column);
        }
        
        sb.Append(',');
    }
}