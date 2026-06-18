using Sirius.Architecture.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sirius.Architecture.Data {

    /// <summary>
    /// Provides an object representation of a Transact-SQL qualified name and easy access to the parts of the name.
    /// </summary>
    public static class TransactSqlName {

        #region Private Static Fields

        private const string _namePartSeparator = ".";

        // NOTE: Static data.
        // All reserved words in SQL Server 7, 2000 and 2005, in alphabetical order.
        // Initialising this set is thread-safe, but searching it is not,
        // so we protect it with a monitor lock.
        private static readonly HashSet<string> _reservedWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
            "ADD", "ALL", "ALTER", "AND", "ANY", "AS", "ASC", "AUTHORIZATION", "AVG",
            "BACKUP", "BEGIN", "BETWEEN", "BREAK", "BROWSE", "BULK", "BY",
            "CASCADE", "CASE", "CHECK", "CHECKPOINT", "CLOSE", "CLUSTERED", "COALESCE", "COLLATE", "COLUMN", "COMMIT", "COMMITTED", "COMPUTE", "CONFIRM", "CONSTRAINT", "CONTAINS", "CONTAINSTABLE", "CONTINUE", "CONTROLROW", "CONVERT", "COUNT", "CREATE", "CROSS", "CURRENT", "CURRENT_DATE", "CURRENT_TIME", "CURRENT_TIMESTAMP", "CURRENT_USER", "CURSOR",
            "DATABASE", "DBCC", "DEALLOCATE", "DECLARE", "DEFAULT", "DELETE", "DENY", "DESC", "DISK", "DISTINCT", "DISTRIBUTED", "DOUBLE", "DROP", "DUMMY", "DUMP",
            "ELSE", "END", "ERRLVL", "ERROREXIT", "ESCAPE", "EXCEPT", "EXEC", "EXECUTE", "EXISTS", "EXIT", "EXTERNAL",
            "FETCH", "FILE", "FILLFACTOR", "FLOPPY", "FOR", "FOREIGN", "FREETEXT", "FREETEXTTABLE", "FROM", "FULL", "FUNCTION",
            "GOTO", "GRANT", "GROUP",
            "HAVING", "HOLDLOCK",
            "IDENTITY", "IDENTITY_INSERT", "IDENTITYCOL", "IF", "IN", "INDEX", "INNER", "INSERT", "INTERSECT", "INTO", "IS", "ISOLATION",
            "JOIN",
            "KEY", "KILL",
            "LEFT", "LEVEL", "LIKE", "LINENO", "LOAD",
            "MAX", "MIN", "MIRROREXIT",
            "NATIONAL", "NOCHECK", "NONCLUSTERED", "NOT", "NULL", "NULLIF",
            "OF", "OFF", "OFFSETS", "ON", "ONCE", "ONLY", "OPEN", "OPENDATASOURCE", "OPENQUERY", "OPENROWSET", "OPENXML", "OPTION", "OR", "ORDER", "OUTER", "OVER",
            "PERCENT", "PERM", "PERMANENT", "PIPE", "PIVOT", "PLAN", "PRECISION", "PREPARE", "PRIMARY", "PRINT", "PRIVILEGES", "PROC", "PROCEDURE", "PROCESSEXIT", "PUBLIC",
            "RAISERROR", "READ", "READTEXT", "RECONFIGURE", "REFERENCES", "REPEATABLE", "REPLICATION", "RESTORE", "RESTRICT", "RETURN", "REVERT", "REVOKE", "RIGHT", "ROLLBACK", "ROWCOUNT", "ROWGUIDCOL", "RULE",
            "SAVE", "SCHEMA", "SECURITYAUDIT", "SELECT", "SERIALIZABLE", "SESSION_USER", "SET", "SETUSER", "SHUTDOWN", "SOME", "STATISTICS", "SUM", "SYSTEM_USER",
            "TABLE", "TABLESAMPLE", "TAPE", "TEMP", "TEMPORARY", "TEXTSIZE", "THEN", "TO", "TOP", "TRAN", "TRANSACTION", "TRIGGER", "TRUNCATE", "TSEQUAL",
            "UNCOMMITTED", "UNION", "UNIQUE", "UNPIVOT", "UPDATE", "UPDATETEXT", "USE", "USER",
            "VALUES", "VARYING", "VIEW",
            "WAITFOR", "WHEN", "WHERE", "WHILE", "WITH", "WORK", "WRITETEXT"
        };

        // NOTE: Static data.
        // Lock control object for the set to be searched.
        private static readonly object _reservedWordsLock = new object();

        #endregion

        #region Private Fields

        // All possible parts of the name.
        // Private _serverName As String
        // Private _databaseName As String
        // Private _schemaName As String
        // Private _objectName As String
        // Private _columnName As String

        #endregion

        #region Constructors

        // TODO: P5 - Implement constructors and parse functions for this class.

        // Valid combinations for a qualified object name:
        // server.database.schema.object
        // server.database..object
        // server..schema.object
        // server...object
        // database.schema.object
        // database..object
        // schema.object
        // object

        // Valid combinations for a qualified column name:
        // database.schema.object.column
        // database..object.column
        // schema.object.column
        // object.column
        // column

        ///// <summary>
        ///// This constructor is not callable for this class.
        ///// </summary>
        //private TransactSqlName() {
        //}

        #endregion

        #region Public Properties

        //     ''' <summary>
        //     ''' Linked server name.
        //     ''' </summary>
        //     Public ReadOnly Property ServerName() As String
        //         Get
        //             Return _serverName
        //         End Get
        //     End Property

        //     ''' <summary>
        //     ''' Database name.
        //     ''' </summary>
        //     Public ReadOnly Property DatabaseName() As String
        //         Get
        //             Return _databaseName
        //         End Get
        //     End Property

        //     ''' <summary>
        //     ''' Schema name (SQL Server 2005 or above) or Owner name (SQL Server 2000 or below).
        //     ''' </summary>
        //     Public ReadOnly Property SchemaName() As String
        //         Get
        //             Return _schemaName
        //         End Get
        //     End Property

        //     ''' <summary>
        //     ''' Object name.
        //     ''' </summary>
        //     Public ReadOnly Property ObjectName() As String
        //         Get
        //             Return _objectName
        //         End Get
        //     End Property

        //     ''' <summary>
        //     ''' Column name.
        //     ''' </summary>
        //     Public ReadOnly Property ColumnName() As String
        //         Get
        //             Return _columnName
        //         End Get
        //     End Property

        #endregion

        #region Public Methods

        //     Public Overrides Function ToString() As String

        //         Dim text As New StringBuilder
        //         Dim dot As New Separator(_separator)

        //         If Not String.IsNullOrEmpty(_serverName) Then
        //             text.Append(dot)
        //             text.Append(Delimit(_serverName))
        //         End If
        //         If Not String.IsNullOrEmpty(_databaseName) Then
        //             text.Append(dot)
        //             text.Append(Delimit(_databaseName))
        //         End If
        //         If Not String.IsNullOrEmpty(_schemaName) Then
        //             text.Append(dot)
        //             text.Append(Delimit(_schemaName))
        //         End If
        //         If Not String.IsNullOrEmpty(_objectName) Then
        //             text.Append(dot)
        //             text.Append(Delimit(_objectName))
        //         End If
        //         If Not String.IsNullOrEmpty(_columnName) Then
        //             text.Append(dot)
        //             text.Append(Delimit(_columnName))
        //         End If

        //         Return text.ToString()

        //     End Function

        #endregion

        #region Public Static Methods - Delimit

        /// <summary>
        /// Format a name as a valid SQL Server delimited identifier, regardless of its content.
        /// </summary>
        /// <param name="name">The single-part name to delimit.</param>
        /// <returns>The delimited identifier.</returns>
        /// <exception cref="ArgumentNullException">The name is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The name is blank.</exception>
        /// <remarks>
        /// This function does not parse the name in any way; it assumes the entire contents of the name
        /// represents a single-part name, and delimits accordingly. To delimit multi-part names, create an instance
        /// of this class and call its properties and methods.
        /// </remarks>
        public static string Delimit(string name) {
            if(name == null) {
                throw new ArgumentNullException("name");
            }
            if(name.Length == 0) {
                throw new ArgumentOutOfRangeException("name", name, "The name is blank.");
            }
            return "[" + name.Replace("]", "]]") + "]";
        }

        /// <summary>
        /// Format a name as a valid SQL Server delimited identifier only if it needs delimiting, otherwise return it unchanged.
        /// </summary>
        /// <param name="name">The single-part name to delimit.</param>
        /// <param name="kind">The type of name (this affects the naming rules applied).</param>
        /// <returns>The identifier, delimited if necessary.</returns>
        /// <exception cref="ArgumentNullException">The name is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The name is blank.</exception>
        /// <remarks>
        /// This function does not parse the name in any way; it assumes the entire contents of the name
        /// represents a single-part name, and delimits accordingly. To delimit multi-part names, create an instance
        /// of this class and call its properties and methods.
        /// </remarks>
        public static string MayDelimit(string name, TransactSqlNameKind kind) {
            return NeedsDelimiting(name, kind) ? Delimit(name) : name;
        }

        /// <summary>
        /// Test whether a name needs delimiting or not before it can be used in a SQL Server command.
        /// </summary>
        /// <param name="name">The single-part name to test.</param>
        /// <param name="kind">The type of name (this affects the naming rules applied).</param>
        /// <returns>True if the name needs delimiting, or false if the name is a legal identifier as it is.</returns>
        /// <exception cref="ArgumentNullException">The name is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The name is blank.</exception>
        /// <remarks>
        /// This function does not parse the name in any way; it assumes the entire contents of the name
        /// represents a single-part name, and delimits accordingly. To test multi-part names, create an instance
        /// of this class and call its properties and methods.
        /// </remarks>
        public static bool NeedsDelimiting(string name, TransactSqlNameKind kind) {

            // Safety check.
            if(name == null) {
                throw new ArgumentNullException("name");
            }
            if(name.Length == 0) {
                throw new ArgumentOutOfRangeException("name", name, "The name is blank.");
            }

            // Check for characters that would be illegal in a SQL Server identifier.
            var firstChar = true;
            for(var i = 0; i <= name.Length - 1; i++) {
                var nameChar = name[i];
                if(('A' <= nameChar && nameChar <= 'Z') || ('a' <= nameChar && nameChar <= 'z') || (nameChar == '_')) {
                    // OK.
                } else if(('0' <= nameChar && nameChar <= '9') || (nameChar == '$')) {
                    // OK if not first character.
                    if(firstChar) {
                        return true;
                    }
                } else if(nameChar == '#') {
                    // OK if not first character or if table name.
                    if(firstChar && kind != TransactSqlNameKind.Table) {
                        return true;
                    }
                } else if(nameChar == '@') {
                    // OK if not first character or if variable name.
                    if(firstChar && kind != TransactSqlNameKind.Variable) {
                        return true;
                    }
                } else {
                    // Bad.
                    return true;
                }
                firstChar = false;
            }

            // If we got this far, then return true if the name matches a reserved word, otherwise false.
            return IsReservedWord(name);
        }

        /// <summary>
        /// Convert a (possibly illegal) name to a new legal SQL Server identifier that does not need delimiting, by replacing all illegal characters with underscores.
        /// </summary>
        /// <param name="name">The single-part name to convert.</param>
        /// <param name="kind">The type of name (this affects the naming rules applied).</param>
        /// <returns>The identifier (without delimiters).</returns>
        /// <remarks>
        /// This function does not parse the name in any way; it assumes the entire contents of the name
        /// represents a single-part name, and delimits accordingly. To convert multi-part names, create an instance
        /// of this class and call its properties and methods.
        /// </remarks>
        public static string MakeLegal(string name, TransactSqlNameKind kind) {

            // A blank name is always OK.
            if(name == null) {
                return string.Empty;
            }
            if(name.Length == 0) {
                return string.Empty;
            }

            // Replace characters that would be illegal in a SQL Server identifier.
            const char replacementChar = '_';
            var result = new StringBuilder(name, name.Length + 1);
            var firstChar = true;
            for(int i = 0; i <= name.Length - 1; i++) {
                var nameChar = name[i];
                if(('A' <= nameChar && nameChar <= 'Z') || ('a' <= nameChar && nameChar <= 'z') || (nameChar == '_')) {
                    // OK.
                } else if(('0' <= nameChar && nameChar <= '9') || (nameChar == '$')) {
                    // OK if not first character.
                    if(firstChar) {
                        result[i] = replacementChar;
                    }
                } else if(nameChar == '#') {
                    // OK if not first character or if table name.
                    if(firstChar && kind != TransactSqlNameKind.Table) {
                        result[i] = replacementChar;
                    }
                } else if(nameChar == '@') {
                    // OK if not first character or if variable name.
                    if(firstChar && kind != TransactSqlNameKind.Variable) {
                        result[i] = replacementChar;
                    }
                } else {
                    // Bad.
                    result[i] = replacementChar;
                }
                firstChar = false;
            }

            // Append an underscore if the name matches a reserved word.
            // NOTE: This is why there is a +1 in the capacity argument to the StringBuilder.
            if(IsReservedWord(result.ToString())) {
                result.Append("_");
            }

            return result.ToString();
        }

        #endregion

        #region Public Static Methods - Parse

        /// <summary>
        /// Parse all the parts of a SQL Server multi-part name and remove any delimiters.
        /// </summary>
        /// <param name="name">The multi-part name to parse.</param>
        /// <returns>A list of all the name parts without delimiters. The list is guaranteed to contain at least one element, and the first and last elements are guaranteed to be non-blank. NOTE: These may require delimiting before they can be used as legal identifiers.</returns>
        /// <exception cref="ArgumentNullException">The multi-part name is null.</exception>
        /// <exception cref="FormatException">The multi-part name is not syntactically valid.</exception>
        public static IList<string> Split(string name) {
            var parts = new List<string>();
            do {
                string syntaxError;
                parts.Add(TransactSqlName.ParseNamePart(name, out name, out syntaxError));
                if(syntaxError != null) {
                    throw new FormatException(syntaxError);
                }
            } while(name.Length > 0);
            if(parts.Count == 0) {
                throw new FormatException(Properties.Resources.ErrorTransactSqlNameNoNamePartsFound);
            }
            if(parts.Count > 4) {
                throw new FormatException(Properties.Resources.ErrorTransactSqlNameMoreThanFourNamePartsFound);
            }
            if(parts.First().Length == 0) {
                throw new FormatException(Properties.Resources.ErrorTransactSqlNameFirstNamePartMustBeSpecified);
            }
            if(parts.Last().Length == 0) {
                throw new FormatException(Properties.Resources.ErrorTransactSqlNameLastNamePartMustBeSpecified);
            }
            return parts;
        }

        /// <summary>
        /// Parse out the first part of a SQL Server multi-part name and remove any delimiters. Return the remainder by reference.
        /// </summary>
        /// <param name="text">The multi-part name to parse.</param>
        /// <param name="remainder">The remainder of the text.</param>
        /// <param name="syntaxError">The syntax error message if the name cannot be parsed, or <see langword="null"/> if correct.</param>
        /// <returns>The first part of the name without delimiters. This may require delimiting before it can be used as a a legal identifier.</returns>
        /// <exception cref="ArgumentNullException">The text is null.</exception>
        public static string ParseNamePart(string text, out string remainder, out string syntaxError) {

            // Safety check.
            if(text == null) {
                throw new ArgumentNullException("text");
            }

            text = text.TrimStart();
            remainder = null;
            syntaxError = null;

            if(text.StartsWith("[")) {
                // [] delimiters. Parse specially.
                return ParseNamePartDelimited(text, out remainder, ref syntaxError, "[", "]");
            } else if(text.StartsWith(@"""")) {
                // "" delimiters. Parse specially.
                return ParseNamePartDelimited(text, out remainder, ref syntaxError, @"""", @"""");
            } else {
                // No delimiters. Split head and tail at the first dot, trimming appropriately.
                var name = StringSplit.HeadTail(text, out remainder, _namePartSeparator, StringComparison.Ordinal);
                remainder = remainder.TrimStart();
                return name.Trim();
            }
        }

        #endregion

        #region Private Static Methods - Parse

        /// <summary>
        /// Parse out the delimited first part of a SQL Server multi-part name and remove the delimiters.
        /// This function is a refactoring of the public one because the code is called twice with different delimiters.
        /// </summary>
        /// <param name="text">The multi-part name to parse.</param>
        /// <param name="remainder">The remainder of the text.</param>
        /// <param name="syntaxError">The syntax error message if the name cannot be parsed, or not set if correct.</param>
        /// <param name="delimiterStart"></param>
        /// <param name="delimiterEnd"></param>
        /// <returns>The first part of the name without delimiters.</returns>
        private static string ParseNamePartDelimited(string text,
            out string remainder,
            ref string syntaxError,
            string delimiterStart,
            string delimiterEnd) {

            // Search for the end delimiter, skipping any escaped delimiters.
            var delimiterEndPos = text.IndexOf(delimiterEnd, 1);
            while(delimiterEndPos > -1 && text.Length > delimiterEndPos + 1 && text[delimiterEndPos + 1].ToString() == delimiterEnd) {
                delimiterEndPos = text.IndexOf(delimiterEnd, delimiterEndPos + 1);
            }

            // End delimiter must exist.
            if(delimiterEndPos == -1) {
                syntaxError = string.Format(Properties.Resources.ErrorTransactSqlNameDelimiterMismatch, delimiterEnd, delimiterStart);
                remainder = null;
                return null;
            }

            // Split out the head from the tail and decode any escaped delimiters.
            var name = text.Substring(1, delimiterEndPos - 1).Replace(delimiterEnd + delimiterEnd, delimiterEnd);
            remainder = text.Substring(delimiterEndPos + 1).TrimStart();

            // Tail must start with the separator or be empty.
            if(remainder.Length > 0 && !remainder.StartsWith(_namePartSeparator)) {
                syntaxError = string.Format(Properties.Resources.ErrorTransactSqlNameOnlyWhitespaceAllowed, delimiterEnd, _namePartSeparator);
                remainder = null;
                return null;
            }

            // Remove the separator from the tail and return the head.
            if(remainder.Length > 0) {
                remainder = remainder.Substring(1).TrimStart();
            }
            return name;
        }

        /// <summary>
        /// Determines whether the specified name is a reserved word.
        /// </summary>
        /// <param name="name">Name to check</param>
        /// <returns>Result</returns>
        private static bool IsReservedWord(string name) {
            lock(_reservedWordsLock) {
                return _reservedWords.Contains(name);
            }
        }

        #endregion
    }
}
