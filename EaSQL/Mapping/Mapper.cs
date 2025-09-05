﻿using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace EaSQL.Mapping
{
    /// <summary>
    /// Type for mapping values from a data reader to a data object.
    /// </summary>
    /// <typeparam name="TType">Type of the data object.</typeparam>
    public sealed class Mapper<TType> where TType : new()
    {
        private static readonly Type _dataRecordType = typeof(IDataRecord);
        private static readonly Type _dataReaderType = typeof(IDataReader);
        private static readonly MethodInfo _getOrdinalInfo = typeof(IDataRecord).GetMethod("GetOrdinal")!;
        private readonly Dictionary<Type, MethodInfo> _memberCache = [];
        private readonly List<IMapping<TType>> _mappingFunctions = [];

        /// <summary>
        /// Define a mapping between a named column and a data type property
        /// </summary>
        /// <typeparam name="TProperty">Type of the mapped property in the data type <typeparamref name="TType"/></typeparam>
        /// <param name="propertySelector">Expression pointing to the property to map</param>
        /// <param name="columnName">Name of the database column to map</param>
        /// <returns>This instance of the mapper to enable chaining of mapping definitions</returns>
        public Mapper<TType> DefineMapping<TProperty>(
            Expression<Func<TType, TProperty>> propertySelector,
            string? columnName = null)
        {
            columnName ??= GetPropertyName(propertySelector);

            Expression<Func<TType, IDataReader, TProperty>> rewritten =
                Rewrite(propertySelector, columnName);

            _mappingFunctions.Add(new Mapping<TType, TProperty>(rewritten.Compile()));

            return this;
        }

        /// <summary>
        /// Define a mapping between a named column and a data type property using a conversion
        /// </summary>
        /// <typeparam name="TProperty">Type of the mapped property in the data type <typeparamref name="TType"/></typeparam>
        /// <typeparam name="TColumn">Type of the column in the database</typeparam>
        /// <param name="propertySelector">Expression pointing to the property to map</param>
        /// <param name="columnName">Name of the database column to map</param>
        /// <param name="conversion">Conversion function from the database column to the property</param>
        /// <returns>This instance of the mapper to enable chaining of mapping definitions</returns>
        public Mapper<TType> DefineMapping<TProperty, TColumn>(
            Expression<Func<TType, TProperty>> propertySelector,
            string columnName,
            Expression<Func<TColumn, TProperty>> conversion)
        {
            Expression<Func<TType, IDataReader, TProperty>> rewritten =
                Rewrite(propertySelector, columnName, conversion);

            _mappingFunctions.Add(new Mapping<TType, TProperty>(rewritten.Compile()));

            return this;
        }

        private Expression<Func<TType, IDataReader, TProperty>> Rewrite<TProperty>(
            Expression<Func<TType, TProperty>> propertySelector,
            string columnName)
        {
            return Rewrite<TProperty, object>(propertySelector, columnName, null);
        }

        private Expression<Func<TType, IDataReader, TProperty>> Rewrite<TProperty, TColumn>(
            Expression<Func<TType, TProperty>> propertySelector, 
            string columnName,
            Expression<Func<TColumn, TProperty>>? conversion)
        {
            ParameterExpression targetParameter = propertySelector.Parameters.Single();
            ParameterExpression readerParameter = Expression.Parameter(_dataReaderType);

            MemberExpression propertyAccess = (MemberExpression)propertySelector.Body;
            MethodCallExpression getOrdinal = Expression.Call(readerParameter, _getOrdinalInfo, Expression.Constant(columnName));
            MethodCallExpression readCall = Expression.Call(
                readerParameter,
                GetRetrieverFor(conversion?.Parameters[0].Type ?? propertyAccess.Type), 
                getOrdinal);
            InvocationExpression? convertCall = null;
            if (conversion != null)
            {
                convertCall = Expression.Invoke(conversion, readCall);
            }
            BinaryExpression assignment = Expression.Assign(propertyAccess, convertCall ?? (Expression)readCall);

            return (Expression<Func<TType, IDataReader, TProperty>>)Expression.Lambda(assignment, targetParameter, readerParameter);
        }

        private MethodInfo GetRetrieverFor(Type columnType)
        {
            if (!_memberCache.TryGetValue(columnType, out MethodInfo? memberInfo))
            {
                memberInfo = _dataRecordType.GetMethod($"Get{columnType.Name}");
                if (memberInfo == null)
                {
                    throw new InvalidOperationException();
                }
                _memberCache.Add(columnType, memberInfo);
            }

            return memberInfo;
        }

        private string GetPropertyName<TProperty>(Expression<Func<TType, TProperty>> propertySelector)
        {
            MemberExpression propertyAccess = (MemberExpression)propertySelector.Body;
            return propertyAccess.Member.Name;
        }

        public TType ApplyMapping(TType target, IDataReader reader)
        {
            _mappingFunctions.ForEach(f => f.Apply(target, reader));

            return target;
        }

        public IEnumerable<TType> ApplyAll(IDataReader reader)
        {
            while (reader.Read())
            {
                yield return ApplyMapping(new(), reader);
            }
        }
    }
}
