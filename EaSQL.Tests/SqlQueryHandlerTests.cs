using EaSQL.Query;
using EaSQL.Tests.Mocks;
using NSubstitute;
using System.Data;

namespace EaSQL.Tests
{
    public class SqlQueryHandlerTests
    {
        private readonly IDbConnection _connection = Substitute.For<IDbConnection>();

        public SqlQueryHandlerTests()
        {
            _connection.CreateCommand().Returns(new MockCommand());
        }

        [Fact]
        public void CreateCommand_ItCreatesCommandIncludingParameters()
        {
            SqlQueryHandler handler = new(_connection);

            IDbCommand command = handler.CreateCommand($"select * from table where id = {11} and name = {"someName"}");

            Assert.NotNull(command);
            Assert.Equal("select * from table where id = @p0 and name = @p1", command.CommandText);
            Assert.Collection((IEnumerable<IDbDataParameter>)command.Parameters,
                p => { Assert.Equal("@p0", p.ParameterName); Assert.Equal(11, p.Value); },
                p => { Assert.Equal("@p1", p.ParameterName); Assert.Equal("someName", p.Value); });
        }
    }
}
