using System.Data;
using System.Runtime.CompilerServices;
using System.Text;

namespace EaSQL.Query
{
    /// <summary>
    /// Handles strings using string interpolation in order to create <see cref="IDbCommand"/>s.
    /// This type converts a given string into a database command, replacing interpolated values with actual command parameters.
    /// </summary>
    [InterpolatedStringHandler]
    public ref struct SqlQueryStringHandler
    {
        private readonly StringBuilder _builder;
        private readonly IDbCommand _command;
        private int _paramCount = 0;

        /// <param name="literalLength">Number of characters in the given sting, not counting given interpolation parameters.</param>
        /// <param name="formattedCount">Number of given interpolation parameters in the given string.</param>
        /// <param name="commandProvider">A provider to create a new command.</param>
        [Obsolete("Use IDbConnection extension method instead!")]
        public SqlQueryStringHandler(int literalLength, int formattedCount, ICommandProvider commandProvider)
        {
            _builder = new(literalLength + ((3 + (formattedCount/10)) * formattedCount));
            _command = commandProvider.CreateCommand();
        }

        /// <param name="literalLength">Number of characters in the given sting, not counting given interpolation parameters.</param>
        /// <param name="formattedCount">Number of given interpolation parameters in the given string.</param>
        /// <param name="connection">DB connection to run the command on.</param>
        public SqlQueryStringHandler(int literalLength, int formattedCount, IDbConnection connection)
        {
            _builder = new(literalLength + ((3 + (formattedCount / 10)) * formattedCount));
            _command = connection.CreateCommand();
        }

        /// <summary>
        /// Appends a literal part of the string.
        /// </summary>
        /// <param name="s"><see cref="string"/> to add.</param>
        public readonly void AppendLiteral(string s)
        {
            _builder.Append(s);
        }

        /// <summary>
        /// Adds a parameter to the command.
        /// </summary>
        /// <typeparam name="T">Type of the interpolated value</typeparam>
        /// <param name="value">Parameter value</param>
        public void AppendFormatted<T>(T value)
        {
            string argumentName = $"@p{_paramCount++}";
            _builder.Append(argumentName);

            IDbDataParameter param = _command.CreateParameter();
            param.ParameterName = argumentName;
            param.Value = value;
            _command.Parameters.Add(param);
        }

        internal readonly IDbCommand GetCommand()
        {
            _command.CommandText = _builder.ToString();

            return _command;
        }
    }
}
