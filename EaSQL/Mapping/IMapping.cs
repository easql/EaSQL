using System.Data;

namespace EaSQL.Mapping
{
    internal interface IMapping<TType>
    {
        void Apply(TType target, IDataReader reader);
    }
}
