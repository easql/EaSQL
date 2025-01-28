using System.Data;

namespace EaSQL.Tests.Mocks
{
    internal class MockParameterCollection : List<IDbDataParameter>, IDataParameterCollection
    {
        public object this[string parameterName]
        {
            get
            {
                return Find(p => p.ParameterName == parameterName)!;
            }
            set
            {
                int index = IndexOf(parameterName);
                if (index == -1)
                {
                    Add((IDbDataParameter)value);
                }
                else
                {
                    this[index] = (IDbDataParameter)value;
                }
            }
        }

        public bool Contains(string parameterName)
        {
            return Find(p => p.ParameterName == parameterName) == null;
        }

        public int IndexOf(string parameterName)
        {
            return FindIndex(p => p.ParameterName == parameterName);
        }

        public void RemoveAt(string parameterName)
        {
            RemoveAll(p => p.ParameterName == parameterName);
        }
    }
}
