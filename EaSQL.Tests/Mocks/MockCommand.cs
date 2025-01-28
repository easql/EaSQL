using System.Data;

namespace EaSQL.Tests.Mocks
{
    internal class MockCommand : IDbCommand
    {
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        public string CommandText { get; set; } = string.Empty;
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        public int CommandTimeout { get; set; }
        public CommandType CommandType { get; set; }
        public IDbConnection? Connection { get; set; }

        public IDataParameterCollection Parameters { get; } = new MockParameterCollection();

        public IDbTransaction? Transaction { get; set; }
        public UpdateRowSource UpdatedRowSource { get; set; }

        public void Cancel()
        {
        }

        public IDbDataParameter CreateParameter()
        {
            return new MockParameter();
        }

        public void Dispose()
        {
        }

        public int ExecuteNonQuery()
        {
            return 0;
        }

        public IDataReader ExecuteReader()
        {
            return new MockDataReader();
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            return new MockDataReader();
        }

        public object? ExecuteScalar()
        {
            return null;
        }

        public void Prepare()
        {
        }
    }
}
