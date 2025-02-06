using System.Data;

namespace EaSQL.Mapping
{
    internal sealed class Mapping<TType, TProperty>(Func<TType, IDataReader, TProperty> mappingFunction) :
        IMapping<TType>
    {
        private readonly Func<TType, IDataReader, TProperty> _mappingFunction = mappingFunction;

        public void Apply(TType target, IDataReader reader)
        {
            _mappingFunction(target, reader);
        }
    }
}
