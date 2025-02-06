using EaSQL.Query;
using EaSQL.Tests.Mocks;
using NSubstitute;
using System.Data;

namespace EaSQL.Tests
{
    public class SqlQueryStringHandlerTests
    {
        private readonly ICommandProvider _commandProvider = Substitute.For<ICommandProvider>();

        public SqlQueryStringHandlerTests()
        {
            _commandProvider.CreateCommand().Returns(new MockCommand());
        }

        [Fact]
        public void AppendLiteral_ItAppendsGivenString()
        {
            SqlQueryStringHandler sqlQueryStringHandler = new(4, 0, _commandProvider);

            sqlQueryStringHandler.AppendLiteral("Test");

            Assert.Equal("Test", sqlQueryStringHandler.GetCommand().CommandText);
        }

        [Fact]
        public void AppendFormatted_ItAppendsGivenValueAsParameter()
        {
            SqlQueryStringHandler sqlQueryStringHandler = new(0, 1, _commandProvider);

            sqlQueryStringHandler.AppendFormatted(1);

            IDbCommand command = sqlQueryStringHandler.GetCommand();

            Assert.Equal("@p0", command.CommandText);
            Assert.Single((IEnumerable<IDbDataParameter>)command.Parameters, (IDbDataParameter p) => p.ParameterName == "@p0" && (int)p.Value! == 1);
        }
    }
}
