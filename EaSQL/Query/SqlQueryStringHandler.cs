using System.Data;
using System.Runtime.CompilerServices;
using System.Text;

namespace EaSQL.Query
{
    [InterpolatedStringHandler]
#pragma warning disable CS9113 // Parameter is unread.
    public ref struct SqlQueryStringHandler(int literalLength, int formattedCount, ICommandProvider commandProvider)
#pragma warning restore CS9113 // Parameter is unread.
    {
        private readonly StringBuilder _builder = new(literalLength + ((3 + (formattedCount/10)) * formattedCount));
        private readonly IDbCommand _command = commandProvider.CreateCommand();
        private int _paramCount = 0;

        public readonly void AppendLiteral(string s)
        {
            _builder.Append(s);
        }

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
