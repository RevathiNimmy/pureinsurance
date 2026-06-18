namespace Sirius.Architecture.Data {

    /// <summary>
    /// The dialect of SQL to use for building a dynamic SQL statement.
    /// </summary>
    public enum SqlStatementDialect {

        /// <summary>
        /// The ANSI-92 dialect (<c>INNER JOIN</c>, <c>LEFT OUTER JOIN</c>, <c>RIGHT OUTER JOIN</c> operators in the <c>FROM</c> clause).
        /// This is the default, and should be used on all code that targets SQL Server 7 or above.
        /// </summary>
        Ansi92SQL,

        /// <summary>
        /// The ANSI-92 dialect, but with brackets round every join expression to force them to be
        /// evaluated in order. This is useful for compatibility with the JET dialect in Access.
        /// </summary>
        Ansi92SQLForcedJoinOrder,

        /// <summary>
        /// The Transact-SQL dialect (<c>*=</c> and <c>=*</c> operators in the <c>WHERE</c> clause). This is useful for
        /// SQL Server 6.5 or below and Sybase Adaptive Server Enterprise.
        /// </summary>
        TransactSQL
    }
}
