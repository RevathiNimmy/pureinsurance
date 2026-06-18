namespace Sirius.Architecture.Data {

    /// <summary>
    /// The type of dynamic SQL statement to build.
    /// </summary>
    public enum SqlStatementType {

        /// <summary>
        /// The statement type has not yet been set.
        /// </summary>
        Unknown,

        /// <summary>
        /// Generate a <c>SELECT</c> statement.
        /// </summary>
        Select,

        /// <summary>
        /// Generate an <c>INSERT</c> statement.
        /// </summary>
        Insert,

        /// <summary>
        /// Generate an <c>UPDATE</c> statement.
        /// </summary>
        Update,

        /// <summary>
        /// Generate a <c>DELETE</c> statement.
        /// </summary>
        Delete,

        /// <summary>
        /// Generate a <c>SELECT</c> statement that counts the number of rows that would be returned by the actual <c>SELECT</c> statement.
        /// </summary>
        SelectRowCount,

        /// <summary>
        /// Generate a <c>SELECT</c> statement that checks whether or not any rows would be returned by the actual <c>SELECT</c> statement.
        /// </summary>
        SelectExists,

        /// <summary>
        /// Generate an <c>IF</c> statement that checks whether or not any rows would be returned by the actual <c>SELECT</c> statement.
        /// </summary>
        IfExists,
    }
}
