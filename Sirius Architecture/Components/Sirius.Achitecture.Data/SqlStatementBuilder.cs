using Sirius.Architecture.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sirius.Architecture.Data {

    /// <summary>
    /// A class for building up a dynamic SQL statement. Set its properties and call its methods,
    /// then call <see cref="ToString"/>, and it will render an appropriate SQL statement.
    /// </summary>
    /// <remarks>
    /// This must only be used for statements that are truly dynamic, e.g. user searches. Statements
    /// that differ only in their parameter values must be done through stored procedures instead.
    /// </remarks>
    public class SqlStatementBuilder : ICloneable {

        #region Private Constants

        private const string ConstructorWarning = "Use another constructor overload, then set any extra properties separately.";

        #endregion

        #region Constructors

        /// <overloads>
        /// Create a new instance of this class.
        /// </overloads>
        /// <summary>
        /// Create a new instance of this class with unknown type, blank table name and the ANSI-92 dialect.
        /// </summary>
        public SqlStatementBuilder() {
            this.Dialect = SqlStatementDialect.Ansi92SQL;
            this.Indent = "    ";
            this.ReadColumns = new List<string>();
            this.WriteColumns = new SqlColumnCollection();
            this.OutputColumns = new List<string>();
            this.Options = new List<string>();
            this.Froms = new List<SqlJoin>();
            this.Wheres = new List<string>();
            this.GroupBys = new List<string>();
            this.Havings = new List<string>();
            this.OrderBys = new List<string>();
        }

        /// <summary>
        /// Create a new instance of this class with the specified type, blank table name and the ANSI-92 dialect.
        /// </summary>
        /// <param name="type">The type of statement (<c>SELECT</c>, <c>INSERT</c>, <c>UPDATE</c>, <c>DELETE</c>) to create.</param>
        public SqlStatementBuilder(SqlStatementType type)
            : this() {
            this.Type = type;
        }

        /// <summary>
        /// Create a new instance of this class with unknown type, the specified table name and the ANSI-92 dialect.
        /// </summary>
        /// <param name="tableName">The name of the table to modify in an <c>INSERT</c>, <c>UPDATE</c> or <c>DELETE</c> statement.</param>
        public SqlStatementBuilder(string tableName)
            : this() {
            this.TableName = tableName;
        }

        /// <summary>
        /// OBSOLETE
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(ConstructorWarning)]
        public SqlStatementBuilder(SqlStatementDialect dialect)
            : this() {
            this.Dialect = dialect;
        }

        /// <summary>
        /// Create a new instance of this class with the specified type, the specified table name and the ANSI-92 dialect.
        /// </summary>
        /// <param name="type">The type of statement (<c>SELECT</c>, <c>INSERT</c>, <c>UPDATE</c>, <c>DELETE</c>) to create.</param>
        /// <param name="tableName">The name of the table to modify in an <c>INSERT</c>, <c>UPDATE</c> or <c>DELETE</c> statement.</param>
        public SqlStatementBuilder(SqlStatementType type, string tableName)
            : this() {
            this.Type = type;
            this.TableName = tableName;
        }

        /// <summary>
        /// OBSOLETE
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(ConstructorWarning)]
        public SqlStatementBuilder(SqlStatementType type, SqlStatementDialect dialect)
            : this() {
            this.Type = type;
            this.Dialect = dialect;
        }

        /// <summary>
        /// OBSOLETE
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(ConstructorWarning)]
        public SqlStatementBuilder(string tableName, SqlStatementDialect dialect)
            : this() {
            this.TableName = tableName;
            this.Dialect = dialect;
        }

        /// <summary>
        /// OBSOLETE
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(ConstructorWarning)]
        public SqlStatementBuilder(string tableName, string indent)
            : this() {
            this.TableName = tableName;
            this.Indent = indent;
        }

        /// <summary>
        /// OBSOLETE
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(ConstructorWarning)]
        public SqlStatementBuilder(SqlStatementDialect dialect, string indent)
            : this() {
            this.Dialect = dialect;
            this.Indent = indent;
        }

        /// <summary>
        /// OBSOLETE
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(ConstructorWarning)]
        public SqlStatementBuilder(SqlStatementType type, string tableName, SqlStatementDialect dialect)
            : this() {
            this.Type = type;
            this.TableName = tableName;
            this.Dialect = dialect;
        }

        /// <summary>
        /// OBSOLETE
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(ConstructorWarning)]
        public SqlStatementBuilder(SqlStatementType type, string tableName, string indent)
            : this() {
            this.Type = type;
            this.TableName = tableName;
            this.Indent = indent;
        }

        /// <summary>
        /// OBSOLETE
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(ConstructorWarning)]
        public SqlStatementBuilder(SqlStatementType type, SqlStatementDialect dialect, string indent)
            : this() {
            this.Type = type;
            this.Dialect = dialect;
            this.Indent = indent;
        }

        /// <summary>
        /// OBSOLETE
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(ConstructorWarning)]
        public SqlStatementBuilder(string tableName, SqlStatementDialect dialect, string indent)
            : this() {
            this.TableName = tableName;
            this.Dialect = dialect;
            this.Indent = indent;
        }

        /// <summary>
        /// OBSOLETE
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(ConstructorWarning)]
        public SqlStatementBuilder(SqlStatementType type, string tableName, SqlStatementDialect dialect, string indent)
            : this() {
            this.Type = type;
            this.TableName = tableName;
            this.Dialect = dialect;
            this.Indent = indent;
        }

        #endregion

        #region Public Properties - Scalar Values

        /// <summary>
        /// SQL dialect to use when constructing the statement.
        /// </summary>
        public SqlStatementDialect Dialect { get; set; }

        /// <summary>
        /// Indent text to use when constructing the statement.
        /// </summary>
        public string Indent { get; set; }

        /// <summary>
        /// Type of statement (<c>SELECT</c>, <c>INSERT</c>, <c>UPDATE</c>, <c>DELETE</c>) to create.
        /// </summary>
        public SqlStatementType Type { get; set; }

        /// <summary>
        /// Name of the table to write to. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - ignored</item>
        /// <item><c>INSERT</c> - required</item>
        /// <item><c>UPDATE</c> - required</item>
        /// <item><c>DELETE</c> - required</item>
        /// </list>
        /// </para>
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Name of the table to create and write to. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - used for the <c>INTO</c> clause</item>
        /// <item><c>INSERT</c> - ignored</item>
        /// <item><c>UPDATE</c> - ignored</item>
        /// <item><c>DELETE</c> - ignored</item>
        /// </list>
        /// </para>
        /// </summary>
        public string IntoTableName { get; set; }

        /// <summary>
        /// Whether or not to return the last generated identity value. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - ignored</item>
        /// <item><c>INSERT</c> - used</item>
        /// <item><c>UPDATE</c> - ignored</item>
        /// <item><c>DELETE</c> - ignored</item>
        /// </list>
        /// </para>
        /// </summary>
        public bool ReturnIdentityValue { get; set; }

        #endregion

        #region Public Properties - Collections

        /// <summary>
        /// List of <c>SELECT</c> options (<c>DISTINCT</c>, <c>TOP</c>, etc). Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - used</item>
        /// <item><c>INSERT</c> - used if <see cref="AddFrom(string)"/> is called</item>
        /// <item><c>UPDATE</c> - ignored</item>
        /// <item><c>DELETE</c> - ignored</item>
        /// </list>
        /// </para>
        /// </summary>
        public List<string> Options { get; set; }

        /// <summary>
        /// List of column expressions to read. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - required</item>
        /// <item><c>INSERT</c> - ignored</item>
        /// <item><c>UPDATE</c> - ignored</item>
        /// <item><c>DELETE</c> - ignored</item>
        /// </list>
        /// </para>
        /// </summary>
        public List<string> ReadColumns { get; set; }

        /// <summary>
        /// List of column name/value pairs to write. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - ignored</item>
        /// <item><c>INSERT</c> - required</item>
        /// <item><c>UPDATE</c> - required</item>
        /// <item><c>DELETE</c> - ignored</item>
        /// </list>
        /// </para>
        /// </summary>
        public SqlColumnCollection WriteColumns { get; set; }

        /// <summary>
        /// List of column expressions to output. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - ignored</item>
        /// <item><c>INSERT</c> - used</item>
        /// <item><c>UPDATE</c> - used</item>
        /// <item><c>DELETE</c> - used</item>
        /// </list>
        /// </para>
        /// </summary>
        public List<string> OutputColumns { get; set; }

        /// <summary>
        /// List of <c>FROM</c> table sources. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - used</item>
        /// <item><c>INSERT</c> - used in <c>INSERT ... SELECT</c> statements</item>
        /// <item><c>UPDATE</c> - used</item>
        /// <item><c>DELETE</c> - used</item>
        /// </list>
        /// </para>
        /// </summary>
        public List<SqlJoin> Froms { get; set; }

        /// <summary>
        /// List of <c>WHERE</c> expressions. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - used</item>
        /// <item><c>INSERT</c> - used if <see cref="AddFrom(string)"/> is called</item>
        /// <item><c>UPDATE</c> - used</item>
        /// <item><c>DELETE</c> - used</item>
        /// </list>
        /// </para>
        /// </summary>
        public List<string> Wheres { get; set; }

        /// <summary>
        /// List of <c>GROUP BY</c> expressions. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - used</item>
        /// <item><c>INSERT</c> - used if <see cref="AddFrom(string)"/> is called</item>
        /// <item><c>UPDATE</c> - ignored</item>
        /// <item><c>DELETE</c> - ignored</item>
        /// </list>
        /// </para>
        /// </summary>
        public List<string> GroupBys { get; set; }

        /// <summary>
        /// List of <c>HAVING</c> expressions. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - used</item>
        /// <item><c>INSERT</c> - used if <see cref="AddFrom(string)"/> is called</item>
        /// <item><c>UPDATE</c> - ignored</item>
        /// <item><c>DELETE</c> - ignored</item>
        /// </list>
        /// </para>
        /// </summary>
        public List<string> Havings { get; set; }

        /// <summary>
        /// List of <c>ORDER BY</c> expressions. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - used</item>
        /// <item><c>INSERT</c> - used if <see cref="AddFrom(string)"/> is called</item>
        /// <item><c>UPDATE</c> - ignored</item>
        /// <item><c>DELETE</c> - ignored</item>
        /// </list>
        /// </para>
        /// </summary>
        public List<string> OrderBys { get; set; }

        /// <summary>
        /// List of column name/value pairs to write. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - ignored</item>
        /// <item><c>INSERT</c> - required</item>
        /// <item><c>UPDATE</c> - required</item>
        /// <item><c>DELETE</c> - ignored</item>
        /// </list>
        /// </para>
        /// </summary>
        public SqlColumnCollection Columns { get { return this.WriteColumns; } }

        #endregion

        #region Public Methods - Add

        /// <summary>
        /// Add a <c>SELECT</c> list option clause (<c>DISTINCT</c>, <c>TOP</c>, etc). Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - used</item>
        /// <item><c>INSERT</c> - used if <see cref="AddFrom(string)"/> is called</item>
        /// <item><c>UPDATE</c> - ignored</item>
        /// <item><c>DELETE</c> - ignored</item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="text">The option to add.</param>
        public void AddOption(string text) {
            this.Options.Add(text);
        }

        /// <summary>
        /// Add a <c>SELECT</c> list column. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - used</item>
        /// <item><c>INSERT</c> - ignored</item>
        /// <item><c>UPDATE</c> - ignored</item>
        /// <item><c>DELETE</c> - ignored</item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="expression">The column expression to add.</param>
        public void AddColumn(string expression) {
            this.ReadColumns.Add(expression);
        }

        /// <summary>
        /// Add an <c>INTO</c> table name. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - used</item>
        /// <item><c>INSERT</c> - ignored</item>
        /// <item><c>UPDATE</c> - ignored</item>
        /// <item><c>DELETE</c> - ignored</item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="tableName">The table name to add.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use the IntoTableName property instead.")]
        public void AddInto(string tableName) {
            this.IntoTableName = tableName;
        }

        /// <overloads>
        /// Add a <c>FROM</c> table source. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - used</item>
        /// <item><c>INSERT</c> - used in <c>INSERT ... SELECT</c> statements</item>
        /// <item><c>UPDATE</c> - used</item>
        /// <item><c>DELETE</c> - used</item>
        /// </list>
        /// </para>
        /// </overloads>
        /// <summary>
        /// Add a <c>FROM</c> table source. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - used</item>
        /// <item><c>INSERT</c> - used in <c>INSERT ... SELECT</c> statements</item>
        /// <item><c>UPDATE</c> - used</item>
        /// <item><c>DELETE</c> - used</item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="tableSource">The table source expression to add.</param>
        public void AddFrom(string tableSource) {
            this.Froms.Add(new SqlJoin { TableSource = tableSource });
        }

        /// <summary>
        /// Add a <c>FROM</c> table source, joined to the previous one. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - used</item>
        /// <item><c>INSERT</c> - used in <c>INSERT ... SELECT</c> statements</item>
        /// <item><c>UPDATE</c> - used</item>
        /// <item><c>DELETE</c> - used</item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="tableSource">The table source expression to add.</param>
        /// <param name="joinKeyword">The type of join.</param>
        /// <param name="joinPredicate">The join condition.</param>
        public void AddFrom(string tableSource, string joinKeyword, string joinPredicate) {
            this.Froms.Add(new SqlJoin { TableSource = tableSource, JoinKeyword = joinKeyword, JoinPredicate = joinPredicate });
        }

        /// <summary>
        /// Add a <c>WHERE</c> condition. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - used</item>
        /// <item><c>INSERT</c> - used if <see cref="AddFrom(string)"/> is called</item>
        /// <item><c>UPDATE</c> - used</item>
        /// <item><c>DELETE</c> - used</item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="expression">The filter expression to add.</param>
        public void AddWhere(string expression) {
            this.Wheres.Add(expression);
        }

        /// <summary>
        /// Add a <c>GROUP BY</c> expression. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - used</item>
        /// <item><c>INSERT</c> - used if <see cref="AddFrom(string)"/> is called</item>
        /// <item><c>UPDATE</c> - ignored</item>
        /// <item><c>DELETE</c> - ignored</item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="expression">The group expression to add.</param>
        public void AddGroupBy(string expression) {
            this.GroupBys.Add(expression);
        }

        /// <summary>
        /// Add a <c>HAVING</c> condition. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - used</item>
        /// <item><c>INSERT</c> - used if <see cref="AddFrom(string)"/> is called</item>
        /// <item><c>UPDATE</c> - ignored</item>
        /// <item><c>DELETE</c> - ignored</item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="expression">The group filter expression to add.</param>
        public void AddHaving(string expression) {
            this.Havings.Add(expression);
        }

        /// <summary>
        /// Add an <c>ORDER BY</c> expression. Relevant to:
        /// <para>
        /// <list type="bullet">
        /// <item><c>SELECT</c> - used</item>
        /// <item><c>INSERT</c> - used if <see cref="AddFrom(string)"/> is called</item>
        /// <item><c>UPDATE</c> - ignored</item>
        /// <item><c>DELETE</c> - ignored</item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="expression">The sort expression to add.</param>
        public void AddOrderBy(string expression) {
            this.OrderBys.Add(expression);
        }

        #endregion

        #region ICloneable Methods

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        object ICloneable.Clone() {
            return Clone();
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public SqlStatementBuilder Clone() {
            return new SqlStatementBuilder {
                Dialect = this.Dialect,
                Indent = this.Indent,
                Type = this.Type,
                TableName = this.TableName,
                ReturnIdentityValue = this.ReturnIdentityValue,
                ReadColumns = Clone(this.ReadColumns),
                WriteColumns = Clone(this.WriteColumns),
                Options = Clone(this.Options),
                Froms = Clone(this.Froms),
                Wheres = Clone(this.Wheres),
                GroupBys = Clone(this.GroupBys),
                Havings = Clone(this.Havings),
                OrderBys = Clone(this.OrderBys),
            };
        }

        private static List<string> Clone(List<string> values) {
            if(values == null) {
                return null;
            }
            return new List<string>(values);
        }

        private static List<SqlJoin> Clone(List<SqlJoin> values) {
            if(values == null) {
                return null;
            }
            return new List<SqlJoin>(values.Select(value => value.Clone()));
        }

        private static SqlColumnCollection Clone(SqlColumnCollection values) {
            if(values == null) {
                return null;
            }
            return values.Clone();
        }

        #endregion

        #region Object Methods

        /// <summary>
        /// Construct the complete SQL statement from all the information provided.
        /// </summary>
        /// <returns>A SQL statement.</returns>
        public override string ToString() {

            var text = new StringBuilder();
            switch(this.Type) {
            case SqlStatementType.Select:
                BuildSelect(text);
                break;
            case SqlStatementType.SelectRowCount:
                BuildSelectRowCount(text);
                break;
            case SqlStatementType.SelectExists:
                BuildSelectExists(text);
                break;
            case SqlStatementType.IfExists:
                BuildIfExists(text);
                break;
            case SqlStatementType.Insert:
                BuildInsert(text);
                break;
            case SqlStatementType.Update:
                BuildUpdate(text);
                break;
            case SqlStatementType.Delete:
                BuildDelete(text);
                break;
            default:
                throw new InvalidOperationException(string.Format(
                    Properties.Resources.ErrorSqlBuilderInvalidStatementType, this.Type));
            }
            return text.ToString();
        }

        #endregion

        #region Private Methods - Whole Statements

        private void BuildSelect(StringBuilder text) {

            var extraWheres = new List<string>();

            text.Append("select");
            BuildOptions(text);
            BuildReadColumns(text);
            BuildTableNameReadInto(text);
            BuildFroms(text, true, extraWheres);
            BuildWheres(text, extraWheres);
            BuildGroupBys(text);
            BuildHavings(text);
            BuildOrderBys(text);
        }

        private void BuildSelectRowCount(StringBuilder text) {

            var extraWheres = new List<string>();

            text.AppendLine("select count(*) from (");

            text.Append("select");
            BuildOptions(text);
            BuildReadColumns(text);
            BuildFroms(text, true, extraWheres);
            BuildWheres(text, extraWheres);
            BuildGroupBys(text);
            BuildHavings(text);

            text.AppendLine();
            text.Append(") as [!]");
        }

        private void BuildSelectExists(StringBuilder text) {

            var extraWheres = new List<string>();

            text.AppendLine("select (case when exists (");

            text.Append("select null");
            BuildFroms(text, true, extraWheres);
            BuildWheres(text, extraWheres);
            BuildGroupBys(text);
            BuildHavings(text);

            text.AppendLine();
            text.Append(") then 1 else 0 end) as [!]");
        }

        private void BuildIfExists(StringBuilder text) {

            var extraWheres = new List<string>();

            text.AppendLine("if exists (");

            text.Append("select null");
            BuildFroms(text, true, extraWheres);
            BuildWheres(text, extraWheres);
            BuildGroupBys(text);
            BuildHavings(text);

            text.AppendLine();
            text.Append(")");
        }

        private void BuildInsert(StringBuilder text) {

            var columnList1 = new StringBuilder();
            var columnList2 = new StringBuilder();
            var expressionFound = BuildWriteColumns(columnList1, columnList2);
            if(columnList1.Length == 0) {
                // No columns to insert, so don't generate the statement at all.
                return;
            }

            text.Append("insert into");
            BuildTableNameWrite(text);
            text.Append(" (");
            text.Append(columnList1);
            text.AppendLine();
            text.Append(")");
            BuildOutput(text);
            if(this.Froms != null && this.Froms.Any()) {
                text.AppendLine();
                var extraWheres = new List<string>();
                text.Append("select");
                BuildOptions(text);
                text.Append(columnList2);
                BuildFroms(text, true, extraWheres);
                BuildWheres(text, extraWheres);
                BuildGroupBys(text);
                BuildHavings(text);
                BuildOrderBys(text);
            } else if(expressionFound) {
                text.AppendLine();
                text.Append("select");
                text.Append(columnList2);
            } else {
                text.AppendLine();
                text.Append("values (");
                text.Append(columnList2);
                text.AppendLine();
                text.Append(")");
            }

            BuildReturnIdentityValue(text);
        }

        private void BuildUpdate(StringBuilder text) {

            var columnList = new StringBuilder();
            BuildWriteColumns(columnList);
            if(columnList.Length == 0) {
                // No columns to update, so don't generate the statement at all.
                return;
            }

            var extraWheres = new List<string>();

            text.Append("update");
            BuildTableNameWrite(text);
            text.Append(" set");
            text.Append(columnList);
            BuildOutput(text);
            BuildFroms(text, false, extraWheres);
            BuildWheres(text, extraWheres);
        }

        private void BuildDelete(StringBuilder text) {

            var extraWheres = new List<string>();

            text.Append("delete from");
            BuildTableNameWrite(text);
            BuildOutput(text);
            BuildFroms(text, false, extraWheres);
            BuildWheres(text, extraWheres);
        }

        #endregion

        #region Private Methods - Scalar Values

        private void BuildTableNameReadInto(StringBuilder text) {

            if(string.IsNullOrEmpty(this.IntoTableName)) {
                return;
            }

            text.AppendLine().Append("into ").Append(this.IntoTableName);
        }

        private void BuildTableNameWrite(StringBuilder text) {

            if(string.IsNullOrEmpty(this.TableName)) {
                throw new InvalidOperationException(Properties.Resources.ErrorSqlBuilderTableNameEmpty);
            }

            text.Append(" ").Append(this.TableName);
        }

        private void BuildReturnIdentityValue(StringBuilder text) {

            if(!this.ReturnIdentityValue) {
                return;
            }

            text.AppendLine().Append("select @@identity");
        }

        #endregion

        #region Private Methods - Collections

        private void BuildOptions(StringBuilder text) {

            if(this.Options == null || !this.Options.Any()) {
                return;
            }

            var space = " ";
            foreach(var expr in this.Options) {
                text.Append(space).Append(expr);
            }
        }

        private void BuildReadColumns(StringBuilder text) {

            if(this.ReadColumns == null || !this.ReadColumns.Any()) {
                throw new InvalidOperationException(string.Format(Properties.Resources.ErrorSqlBuilderCollectionEmpty, "ReadColumns"));
            }

            var comma = new Separator(",");
            foreach(var expr in this.ReadColumns) {
                text.Append(comma).AppendLine().Append(this.Indent).Append(expr);
            }
        }

        private void BuildWriteColumns(StringBuilder text) {

            if(this.WriteColumns == null || !this.WriteColumns.Any()) {
                throw new InvalidOperationException(string.Format(Properties.Resources.ErrorSqlBuilderCollectionEmpty, "WriteColumns"));
            }

            var comma = new Separator(",");
            foreach(var column in this.WriteColumns) {
                if(column.Value != Missing.Value) {
                    text.Append(comma).AppendLine().Append(this.Indent).Append(column.Key).Append(" = ").
                        Append(TransactSqlFormat.ToString(column.Value));
                }
            }
        }

        private bool BuildWriteColumns(StringBuilder text1, StringBuilder text2) {

            if(this.WriteColumns == null || !this.WriteColumns.Any()) {
                throw new InvalidOperationException(string.Format(Properties.Resources.ErrorSqlBuilderCollectionEmpty, "WriteColumns"));
            }

            var comma1 = new Separator(",");
            var comma2 = new Separator(",");
            var expressionFound = false;
            foreach(var column in this.WriteColumns) {
                if(column.Value != Missing.Value) {
                    text1.Append(comma1).AppendLine().Append(this.Indent).Append(column.Key);
                    text2.Append(comma2).AppendLine().Append(this.Indent).Append(TransactSqlFormat.ToString(column.Value));
                    if(column.Value is SqlExpression) {
                        expressionFound = true;
                    }
                }
            }
            return expressionFound;
        }

        private void BuildOutput(StringBuilder text) {

            if(this.OutputColumns == null || !this.OutputColumns.Any()) {
                return;
            }

            text.AppendLine().Append("output");
            var comma = new Separator(",");
            foreach(var expr in this.OutputColumns) {
                text.Append(comma).AppendLine().Append(this.Indent).Append(expr);
            }
        }

        private void BuildFroms(StringBuilder text, bool mandatory, List<string> extraWheres) {

            if(this.Froms == null || !this.Froms.Any()) {
                if(mandatory) {
                    throw new InvalidOperationException(string.Format(Properties.Resources.ErrorSqlBuilderCollectionEmpty, "Froms"));
                } else {
                    return;
                }
            }

            if(this.Froms.First().HasJoin) {
                throw new InvalidOperationException(Properties.Resources.ErrorSqlBuilderBadFrom1);
            }
            if(this.Froms.Skip(1).Any(f => !f.HasJoin)) {
                throw new InvalidOperationException(Properties.Resources.ErrorSqlBuilderBadFrom2);
            }

            int fromIndex = 0;
            switch(this.Dialect) {
            case SqlStatementDialect.Ansi92SQL:
                text.AppendLine().Append("from ");
                foreach(var from in this.Froms) {
                    if(fromIndex == 0) {
                        text.Append(from.TableSource);
                    } else {
                        text.AppendLine().Append(from.JoinKeyword).
                            Append(" join ").Append(from.TableSource).
                            Append(" on ").Append(from.JoinPredicate);
                    }
                    fromIndex++;
                }
                break;
            case SqlStatementDialect.Ansi92SQLForcedJoinOrder:
                text.AppendLine().Append("from ");
                var tables = new StringBuilder();
                foreach(var from in this.Froms) {
                    if(fromIndex == 0) {
                        tables.Append(from.TableSource);
                    } else {
                        if(fromIndex != 1) {
                            tables.Insert(0, "(").Append(")");
                        }
                        tables.AppendLine().Append(from.JoinKeyword).
                            Append(" join ").Append(from.TableSource).
                            Append(" on ").Append(from.JoinPredicate);
                    }
                    fromIndex++;
                }
                text.Append(tables);
                break;
            case SqlStatementDialect.TransactSQL:
                var comma1 = new Separator(Environment.NewLine + "from ", "," + Environment.NewLine + this.Indent);
                foreach(var from in this.Froms) {
                    text.Append(comma1).Append(from.TableSource);
                }
                foreach(var from in this.Froms.Skip(1)) {
                    string extraWhere = from.JoinPredicate;
                    switch(from.JoinKeyword.ToLowerInvariant()) {
                    case "left":
                    case "left outer":
                        extraWhere = extraWhere.Replace("=", "*=");
                        break;
                    case "right":
                    case "right outer":
                        extraWhere = extraWhere.Replace("=", "=*");
                        break;
                    }
                    extraWheres.Add(extraWhere);
                }
                break;
            }
        }

        private void BuildWheres(StringBuilder text, List<string> extraWheres) {

            var wheres = (extraWheres ?? new List<string>()).Concat(this.Wheres ?? new List<string>());

            if(wheres == null || !wheres.Any()) {
                return;
            }

            var op = new Separator("where", "and");
            foreach(var expr in wheres) {
                text.AppendLine().Append(op).Append(" ").Append(expr);
            }
        }

        private void BuildHavings(StringBuilder text) {

            if(this.Havings == null || !this.Havings.Any()) {
                return;
            }

            var op = new Separator("having", "and");
            foreach(var expr in this.Havings) {
                text.AppendLine().Append(op).Append(" ").Append(expr);
            }
        }

        private void BuildGroupBys(StringBuilder text) {

            if(this.GroupBys == null || !this.GroupBys.Any()) {
                return;
            }

            text.AppendLine().Append("group by");

            var comma = new Separator(",");
            foreach(var expr in this.GroupBys) {
                text.Append(comma).AppendLine().Append(this.Indent).Append(expr);
            }
        }

        private void BuildOrderBys(StringBuilder text) {

            if(this.OrderBys == null || !this.OrderBys.Any()) {
                return;
            }

            text.AppendLine().Append("order by");

            var comma = new Separator(",");
            foreach(var expr in this.OrderBys) {
                text.Append(comma).AppendLine().Append(this.Indent).Append(expr);
            }
        }

        #endregion
    }
}
