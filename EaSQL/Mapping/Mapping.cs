using System.Data;
using System.Data.Common;

namespace EaSQL.Mapping
{
    internal sealed class Mapping<TType, TProperty>(Func<TType, DbDataReader, TProperty> mappingFunction) :
        IMapping<TType>
    {
        private readonly Func<TType, DbDataReader, TProperty> _mappingFunction = mappingFunction;

        public void Apply(TType target, DbDataReader reader)
        {
            _mappingFunction(target, reader);
        }
    }
}
