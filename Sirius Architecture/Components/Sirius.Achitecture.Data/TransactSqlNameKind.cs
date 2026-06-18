namespace Sirius.Architecture.Data {

    /// <summary>
    /// Describes the type of Transact-SQL name being processed.
    /// </summary>
    public enum TransactSqlNameKind {

        /// <summary>
        /// The type of name is unimportant in this context (default value).
        /// </summary>
        None,

        /// <summary>
        /// A table in a database.
        /// </summary>
        Table,

        /// <summary>
        /// A column in a table.
        /// </summary>
        Column,

        /// <summary>
        /// A variable name in a block of Transact-SQL code.
        /// </summary>
        Variable
    }
}
