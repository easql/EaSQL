using System.Data;

namespace EaSQL.Query
{
    public interface ICommandProvider
    {
        IDbCommand CreateCommand();
    }
}
