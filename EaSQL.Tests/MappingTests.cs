using EaSQL.Mapping;
using EaSQL.Tests.Mocks;

namespace EaSQL.Tests
{
    public class MappingTests
    {
        [Fact]
        public void Apply_ItCallsMappingFunction()
        {
            int callResult = 0;

            Mapping<int, int> mapping = new((i, r) => callResult = i + r.RecordsAffected);

            mapping.Apply(4, new MockDataReader());

            Assert.Equal(14, callResult);
        }
    }
}
