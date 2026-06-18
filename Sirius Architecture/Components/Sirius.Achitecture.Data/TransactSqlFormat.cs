using Sirius.Architecture.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Sirius.Architecture.Data {

    /// <summary>
    /// Transact-SQL source code formatting methods. Use these to write values out as Transact-SQL literal constants.
    /// </summary>
    public sealed class TransactSqlFormat : ICustomFormatter, IFormatProvider {

        #region Private Constants

        private const string NullLiteral = "null";

        #endregion

        #region Public Static Methods - ToString for non-nullable basic types

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(Boolean value) {
            return value ? "1" : "0";
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(Byte value) {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(Int16 value) {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(Int32 value) {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(Int64 value) {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(Single value) {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(Double value) {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(Decimal value) {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(DateTime value) {
            // NOTE: Fractional seconds are deliberately stripped off the value.
            // This is because it is not possible to construct a single literal value that is
            // completely unambiguous for all possible date/time data types in SQL Server
            // for all possible millisecond values.
            if(value == value.Date) {
                return "{d'" + value.ToString(@"yyyy\-MM\-dd", CultureInfo.InvariantCulture) + "'}";
            } else {
                return "{ts'" + value.ToString(@"yyyy\-MM\-dd HH\:mm\:ss", CultureInfo.InvariantCulture) + "'}";
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(TimeSpan value) {
            // NOTE: Fractional seconds are deliberately stripped off the value.
            // This is because it is not possible to construct a single literal value that is
            // completely unambiguous for all possible date/time data types in SQL Server
            // for all possible millisecond values.
            return "{t'" + string.Format(CultureInfo.InvariantCulture, "{0:00}:{1:00}:{2:00}",
                value.Hours, value.Minutes, value.Seconds) + "'}";
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(DateTimeOffset value) {
            // DATETIMEOFFSET supports the ISO8601 literal format, so use that.
            return string.Format("convert(datetimeoffset, '{0}')", XmlConvert.ToString(value));
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(Guid value) {
            return "cast('" + value.ToString().ToUpperInvariant() + "' as uniqueidentifier)";
        }

        #endregion

        #region Public Static Methods - ToString for nullable basic types

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(Boolean? value) {
            if(!value.HasValue) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(Byte? value) {
            if(!value.HasValue) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(Int16? value) {
            if(!value.HasValue) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(Int32? value) {
            if(!value.HasValue) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(Int64? value) {
            if(!value.HasValue) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(Single? value) {
            if(!value.HasValue) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(Double? value) {
            if(!value.HasValue) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(Decimal? value) {
            if(!value.HasValue) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(DateTime? value) {
            if(!value.HasValue) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(TimeSpan? value) {
            if(!value.HasValue) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(DateTimeOffset? value) {
            if(!value.HasValue) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(Guid? value) {
            if(!value.HasValue) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(Byte[] value) {
            if(value == null) {
                return NullLiteral;
            } else {
                return "0x" + value.Select(b => b.ToString("X2", CultureInfo.InvariantCulture)).Join(string.Empty);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(String value) {
            if(value == null) {
                return NullLiteral;
            } else {
                return "'" + value.Replace("'", "''") + "'";
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(SqlUnicodeLiteral value) {
            if(value == null) {
                return NullLiteral;
            } else {
                return "N'" + value.ToString().Replace("'", "''") + "'";
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(SqlExpression value) {
            if(value == null) {
                return string.Empty;
            } else {
                return value.ToString();
            }
        }

        #endregion

        #region Public Static Methods - ToString for lists of types

        /// <summary>
        /// Format a list of values as a Transact-SQL bracketed list expression.
        /// </summary>
        public static string ToString<T>(IEnumerable<T> values) {
            if(values == null) {
                // Bomb out because "in null" is invalid syntax.
                throw new ArgumentNullException("values", Properties.Resources.ErrorListNull);
            } else if(!values.Any()) {
                // This works because null never matches anything in SQL server, even itself,
                // yet it is a valid literal to populate the bracketed list with.
                return "(null)";
            } else {
                return "(" + values.Select(value => ToString(value)).Join(", ") + ")";
            }
        }

        /// <summary>
        /// Format a list of values as a Transact-SQL bracketed list expression.
        /// </summary>
        public static string ToString(IEnumerable values) {
            if(values == null) {
                // Bomb out because "in null" is invalid syntax.
                throw new ArgumentNullException("values", Properties.Resources.ErrorListNull);
            } else {
                return ToString(values.Cast<object>());
            }
        }

        #endregion

        #region Public Static Methods - ToString for XML types

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(XContainer value) {
            if(value == null) {
                return NullLiteral;
            } else {
                return "cast(" + ToString((SqlUnicodeLiteral) value.ToString()) + " as xml)";
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(XPathNavigator value) {
            if(value == null) {
                return NullLiteral;
            } else {
                return "cast(" + ToString((SqlUnicodeLiteral) value.OuterXml) + " as xml)";
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(XmlNode value) {
            if(value == null) {
                return NullLiteral;
            } else {
                return "cast(" + ToString((SqlUnicodeLiteral) value.OuterXml) + " as xml)";
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(XmlReader value) {
            if(value == null) {
                return NullLiteral;
            } else {
                return "cast(" + ToString((SqlUnicodeLiteral) value.ReadOuterXml()) + " as xml)";
            }
        }

        #endregion

        #region Public Static Methods - ToString for SQL types

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(SqlBoolean value) {
            if(value.IsNull) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(SqlByte value) {
            if(value.IsNull) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(SqlInt16 value) {
            if(value.IsNull) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(SqlInt32 value) {
            if(value.IsNull) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(SqlInt64 value) {
            if(value.IsNull) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(SqlSingle value) {
            if(value.IsNull) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(SqlDouble value) {
            if(value.IsNull) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(SqlMoney value) {
            if(value.IsNull) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(SqlDecimal value) {
            if(value.IsNull) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(SqlDateTime value) {
            if(value.IsNull) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(SqlGuid value) {
            if(value.IsNull) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(SqlBinary value) {
            if(value.IsNull) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(SqlString value) {
            if(value.IsNull) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(SqlXml value) {
            if(value.IsNull) {
                return NullLiteral;
            } else {
                return "cast(" + ToString((SqlUnicodeLiteral) value.Value) + " as xml)";
            }
        }

        #endregion

        #region Public Static Methods - ToString for complex data access objects

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(SqlParameter value) {
            if(value == null) {
                return NullLiteral;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(SiriusCommand value) {
            if(value == null) {
                return NullLiteral;
            } else {
                return ToString(value.SqlCommand);
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(SqlStatementBuilder value) {
            if(value == null) {
                return NullLiteral;
            } else {
                return value.ToString();
            }
        }

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(SqlCommand value) {
            if(value == null) {
                return NullLiteral;
            } else {
                switch(value.CommandType) {
                case CommandType.StoredProcedure:
                    return WriteStoredProcedure(value);
                case CommandType.Text:
                    return WriteText(value);
                default:
                    throw new ArgumentOutOfRangeException("value.CommandType", value.CommandType, Properties.Resources.ErrorBadCommandType);
                }
            }
        }

        #endregion

        #region Public Static Methods - ToString for late-bound objects

        /// <summary>
        /// Format a value as a Transact-SQL expression.
        /// </summary>
        public static string ToString(object value) {
            if(value == null || value is DBNull) {
                return NullLiteral;
            } else if(value is Boolean) {
                return ToString((Boolean) value);
            } else if(value is Byte) {
                return ToString((Byte) value);
            } else if(value is Int16) {
                return ToString((Int16) value);
            } else if(value is Int32) {
                return ToString((Int32) value);
            } else if(value is Int64) {
                return ToString((Int64) value);
            } else if(value is Single) {
                return ToString((Single) value);
            } else if(value is Double) {
                return ToString((Double) value);
            } else if(value is Decimal) {
                return ToString((Decimal) value);
            } else if(value is DateTime) {
                return ToString((DateTime) value);
            } else if(value is TimeSpan) {
                return ToString((TimeSpan) value);
            } else if(value is DateTimeOffset) {
                return ToString((DateTimeOffset) value);
            } else if(value is Guid) {
                return ToString((Guid) value);
            } else if(value is Byte[]) {
                return ToString((Byte[]) value);
            } else if(value is String) {
                return ToString((String) value);
            } else if(value is XContainer) {
                return ToString((XContainer) value);
            } else if(value is XPathNavigator) {
                return ToString((XPathNavigator) value);
            } else if(value is XmlNode) {
                return ToString((XmlNode) value);
            } else if(value is XmlReader) {
                return ToString((XmlReader) value);
            } else if(value is SqlBoolean) {
                return ToString((SqlBoolean) value);
            } else if(value is SqlByte) {
                return ToString((SqlByte) value);
            } else if(value is SqlInt16) {
                return ToString((SqlInt16) value);
            } else if(value is SqlInt32) {
                return ToString((SqlInt32) value);
            } else if(value is SqlInt64) {
                return ToString((SqlInt64) value);
            } else if(value is SqlSingle) {
                return ToString((SqlSingle) value);
            } else if(value is SqlDouble) {
                return ToString((SqlDouble) value);
            } else if(value is SqlMoney) {
                return ToString((SqlMoney) value);
            } else if(value is SqlDecimal) {
                return ToString((SqlDecimal) value);
            } else if(value is SqlDateTime) {
                return ToString((SqlDateTime) value);
            } else if(value is SqlGuid) {
                return ToString((SqlGuid) value);
            } else if(value is SqlBinary) {
                return ToString((SqlBinary) value);
            } else if(value is SqlString) {
                return ToString((SqlString) value);
            } else if(value is SqlXml) {
                return ToString((SqlXml) value);
            } else if(value is SqlParameter) {
                return ToString((SqlParameter) value);
            } else if(value is SqlCommand) {
                return ToString((SqlCommand) value);
            } else if(value is SiriusCommand) {
                return ToString((SiriusCommand) value);
            } else if(value is SqlStatementBuilder) {
                return ToString((SqlStatementBuilder) value);
            } else if(value is SqlExpression) {
                return ToString((SqlExpression) value);
            } else if(value is SqlUnicodeLiteral) {
                return ToString((SqlUnicodeLiteral) value);
            } else if(value is Enum) {
                return ToString(((IConvertible) value).ToInt64(null));
            } else if(value is IEnumerable) {
                return ToString((IEnumerable) value);
            } else {
                throw new InvalidCastException(string.Format(Properties.Resources.ErrorToSqlInvalidType, value.GetType()));
            }
        }

        #endregion

        #region Private Static Methods

        private static string WriteStoredProcedure(SqlCommand value) {
            var separator = Environment.NewLine + "    ";
            var declaration = new StringBuilder();
            var input = new StringBuilder();
            var call1 = new StringBuilder("execute ");
            var call2 = new StringBuilder(value.CommandText);
            var output = new StringBuilder();
            var declarationSep = new Separator("declare" + separator, "," + separator);
            var inputSep = new Separator("select" + separator, "," + separator);
            var outputSep = new Separator("select" + separator, "," + separator);
            var call2Sep = new Separator(separator, "," + separator);

            foreach(SqlParameter parameter in value.Parameters) {
                var variableName = parameter.ParameterName;
                if(!variableName.StartsWith("@")) {
                    variableName = "@" + variableName;
                }
                WriteDeclaration(declaration, declarationSep, parameter, variableName);
                switch(parameter.Direction) {
                case ParameterDirection.ReturnValue:
                    WriteOutput(output, outputSep, parameter, variableName);
                    call1.Append(variableName);
                    call1.Append(" = ");
                    break;
                case ParameterDirection.Input:
                    WriteInput(input, inputSep, parameter, variableName);
                    call2.Append(call2Sep);
                    call2.Append(parameter.ParameterName);
                    call2.Append(" = ");
                    call2.Append(variableName);
                    break;
                case ParameterDirection.InputOutput:
                    WriteInput(input, inputSep, parameter, variableName);
                    WriteOutput(output, outputSep, parameter, variableName);
                    call2.Append(call2Sep);
                    call2.Append(parameter.ParameterName);
                    call2.Append(" = ");
                    call2.Append(variableName);
                    call2.Append(" output");
                    break;
                case ParameterDirection.Output:
                    WriteOutput(output, outputSep, parameter, variableName);
                    call2.Append(call2Sep);
                    call2.Append(parameter.ParameterName);
                    call2.Append(" = ");
                    call2.Append(variableName);
                    call2.Append(" output");
                    break;
                }
            }

            var sqlText = new StringBuilder();
            if(declaration.Length > 0) {
                sqlText.AppendLine(declaration.ToString());
            }
            if(input.Length > 0) {
                sqlText.AppendLine(input.ToString());
            }
            sqlText.Append(call1);
            sqlText.AppendLine(call2.ToString());
            if(output.Length > 0) {
                sqlText.AppendLine(output.ToString());
            }
            return sqlText.ToString();
        }

        private static string WriteText(SqlCommand value) {
            var separator = Environment.NewLine + "    ";
            var declaration = new StringBuilder();
            var input = new StringBuilder();
            var callText = value.CommandText.Trim();
            var output = new StringBuilder();
            var declarationSep = new Separator("declare" + separator, "," + separator);
            var inputSep = new Separator("select" + separator, "," + separator);
            var outputSep = new Separator("select" + separator, "," + separator);

            foreach(SqlParameter parameter in value.Parameters) {
                var variableName = parameter.ParameterName;
                if(!variableName.StartsWith("@")) {
                    variableName = "@" + variableName;
                }
                WriteDeclaration(declaration, declarationSep, parameter, variableName);
                switch(parameter.Direction) {
                case ParameterDirection.ReturnValue:
                    WriteOutput(output, outputSep, parameter, variableName);
                    break;
                case ParameterDirection.Input:
                    WriteInput(input, inputSep, parameter, variableName);
                    break;
                case ParameterDirection.InputOutput:
                    WriteInput(input, inputSep, parameter, variableName);
                    WriteOutput(output, outputSep, parameter, variableName);
                    break;
                case ParameterDirection.Output:
                    WriteOutput(output, outputSep, parameter, variableName);
                    break;
                }
            }

            var sqlText = new StringBuilder();
            if(declaration.Length > 0) {
                sqlText.AppendLine(declaration.ToString());
            }
            if(input.Length > 0) {
                sqlText.AppendLine(input.ToString());
            }
            sqlText.AppendLine(callText);
            if(output.Length > 0) {
                sqlText.AppendLine(output.ToString());
            }
            return sqlText.ToString();
        }

        private static void WriteDeclaration(StringBuilder sqlText, Separator separator, SqlParameter parameter, string variableName) {
            sqlText.Append(separator);
            sqlText.Append(variableName);
            sqlText.Append(" ");
            sqlText.Append(ParameterTypeToString(parameter));
        }

        private static void WriteInput(StringBuilder sqlText, Separator separator, SqlParameter parameter, string variableName) {
            sqlText.Append(separator);
            sqlText.Append(variableName);
            sqlText.Append(" = ");
            sqlText.Append(ToString(parameter));
        }

        private static void WriteOutput(StringBuilder sqlText, Separator separator, SqlParameter parameter, string variableName) {
            sqlText.Append(separator);
            sqlText.Append(variableName);
            sqlText.Append(" as [");
            sqlText.Append(parameter.ParameterName);
            sqlText.Append("]");
        }

        /// <summary>
        /// Construct a syntactically valid parameter data type definition.
        /// </summary>
        /// <remarks>
        /// Also called from SiriusCommand.
        /// </remarks>
        internal static string ParameterTypeToString(SqlParameter parameter) {
            // Almost all of the SqlDbType enum values match the underlying SQL type names, which is useful.
            var text = parameter.SqlDbType.ToString().ToLowerInvariant();
            // Make type-specific adjustments.
            switch(parameter.SqlDbType) {
            case SqlDbType.Decimal:
                text += string.Format(CultureInfo.InvariantCulture, "({0}, {1})", parameter.Precision, parameter.Scale);
                break;
            case SqlDbType.Binary:
            case SqlDbType.Char:
            case SqlDbType.NChar:
                text += string.Format(CultureInfo.InvariantCulture, "({0})", parameter.Size);
                break;
            case SqlDbType.VarBinary:
            case SqlDbType.VarChar:
                if(parameter.Size >= 1 && parameter.Size <= 8000) {
                    text += string.Format(CultureInfo.InvariantCulture, "({0})", parameter.Size);
                } else {
                    text += "(max)";
                }
                break;
            case SqlDbType.NVarChar:
                if(parameter.Size >= 1 && parameter.Size <= 4000) {
                    text += string.Format(CultureInfo.InvariantCulture, "({0})", parameter.Size);
                } else {
                    text += "(max)";
                }
                break;
            case SqlDbType.Structured:
                if(!string.IsNullOrEmpty(parameter.TypeName)) {
                    text = parameter.TypeName;
                } else {
                    text = "unknown_tvp";
                }
                break;
            case SqlDbType.Udt:
                if(!string.IsNullOrEmpty(parameter.UdtTypeName)) {
                    text = parameter.UdtTypeName;
                } else {
                    text = "unknown_udt";
                }
                break;
            case SqlDbType.Variant:
                text = "sql_variant";
                break;
            }
            return text;
        }

        #endregion

        #region ICustomFormatter Members

        string ICustomFormatter.Format(string format, object value, IFormatProvider dummy) {
            return ToString(value);
        }

        #endregion

        #region IFormatProvider Members

        object IFormatProvider.GetFormat(Type formatType) {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }

        #endregion
    }
}
