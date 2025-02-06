using EaSQL.Mapping;
using EaSQL.Tests.Mocks;

namespace EaSQL.Tests
{
    public class MapperTests
    {
        private class TestDTO
        {
            public int Value1 { get; set; }
            public decimal Value2 { get; set; }
            public string? Value3 { get; set; }
            public bool Value4 { get; set; }
        }

        [Fact]
        public void DefineMapping_ItReturnsSameMapperInstance()
        {
            Mapper<TestDTO> mapper = new();

            Mapper<TestDTO> returnedMapper = mapper.DefineMapping(d => d.Value1, "column");

            Assert.Same(mapper, returnedMapper);
        }

        [Fact]
        public void ApplyMapping_ItMappsAllValues()
        {
            Mapper<TestDTO> mapper = new Mapper<TestDTO>()
                .DefineMapping(d => d.Value1, "Column")
                .DefineMapping(d => d.Value2, "Column")
                .DefineMapping(d => d.Value3, "Column")
                .DefineMapping(d => d.Value4, "Column");

            TestDTO mapped = mapper.ApplyMapping(new(), new MockDataReader());

            Assert.Equal(42, mapped.Value1);
            Assert.Equal(4.1M, mapped.Value2);
            Assert.Equal("43", mapped.Value3);
            Assert.True(mapped.Value4);
        }

        [Fact]
        public void ApplyMapping_ItConvertsAndMappsAllValues()
        {
            Mapper<TestDTO> mapper = new Mapper<TestDTO>()
                .DefineMapping(d => d.Value1, "Column", (string s) => int.Parse(s))
                .DefineMapping(d => d.Value2, "Column")
                .DefineMapping(d => d.Value3, "Column", (int i) => i.ToString())
                .DefineMapping(d => d.Value4, "Column");

            TestDTO mapped = mapper.ApplyMapping(new(), new MockDataReader());

            Assert.Equal(43, mapped.Value1);
            Assert.Equal(4.1M, mapped.Value2);
            Assert.Equal("42", mapped.Value3);
            Assert.True(mapped.Value4);
        }
    }
}
