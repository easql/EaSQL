using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace EaSQL.Tests.Mocks
{
    internal class MockDataReader : IDataReader
    {
        public object this[int i] => new();

        public object this[string name] => new();

        public int Depth => 1;

        public bool IsClosed => true;

        public int RecordsAffected => 10;

        public int FieldCount => 0;

        public void Close()
        {
        }

        public void Dispose()
        {
        }

        public bool GetBoolean(int i)
        {
            return true;
        }

        public byte GetByte(int i)
        {
            return default;
        }

        public long GetBytes(int i, long fieldOffset, byte[]? buffer, int bufferoffset, int length)
        {
            return default;
        }

        public char GetChar(int i)
        {
            return default;
        }

        public long GetChars(int i, long fieldoffset, char[]? buffer, int bufferoffset, int length)
        {
            return default;
        }

        public IDataReader GetData(int i)
        {
            return this;
        }

        public string GetDataTypeName(int i)
        {
            return string.Empty;
        }

        public DateTime GetDateTime(int i)
        {
            return default;
        }

        public decimal GetDecimal(int i)
        {
            return 4.1M;
        }

        public double GetDouble(int i)
        {
            return default;
        }

        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)]
        public Type GetFieldType(int i)
        {
            return typeof(object);
        }

        public float GetFloat(int i)
        {
            return default;
        }

        public Guid GetGuid(int i)
        {
            return default;
        }

        public short GetInt16(int i)
        {
            return default;
        }

        public int GetInt32(int i)
        {
            return 42;
        }

        public long GetInt64(int i)
        {
            return default;
        }

        public string GetName(int i)
        {
            return string.Empty;
        }

        public int GetOrdinal(string name)
        {
            return default;
        }

        public DataTable? GetSchemaTable()
        {
            return default;
        }

        public string GetString(int i)
        {
            return "43";
        }

        public object GetValue(int i)
        {
            return new();
        }

        public int GetValues(object[] values)
        {
            return default;
        }

        public bool IsDBNull(int i)
        {
            return default;
        }

        public bool NextResult()
        {
            return default;
        }

        public bool Read()
        {
            return default;
        }
    }
}
