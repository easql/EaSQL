using System.Data;

namespace EaSQL.Mapping
{
    /// <summary>
    /// Represents a single mapping for a given <typeparamref name="TType"/>
    /// </summary>
    /// <typeparam name="TType">Type this mapping is defined for</typeparam>
    internal interface IMapping<TType>
    {
        /// <summary>
        /// Applies the mapping to the <paramref name="target"/> reading the mapped value from the given <paramref name="reader"/>.
        /// </summary>
        /// <param name="target">Target to apply the mapping to.</param>
        /// <param name="reader">Database reader to read the value from.</param>
        void Apply(TType target, IDataReader reader);
    }
}
