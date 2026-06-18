namespace Sirius.Architecture.Data {

    /// <summary>
    /// A well-known structure that the database is known to conform to. This value determines whether certain
    /// tables and stored procedures are available.
    /// </summary>
    public enum SiriusConnectionSchema {
        /// <summary>
        /// The database schema is not known.
        /// </summary>
        Unknown,
        /// <summary>
        /// A Sirius database.
        /// </summary>
        Sirius,
        /// <summary>
        /// A Swift database.
        /// </summary>
        Swift,
        /// <summary>
        /// A Swift Suitability Letters database.
        /// </summary>
        /// <remarks>
        /// This is only a temporary situation. When time permits, this database will be merged back
        /// into the Swift database. Check with Swift Development for the current state of this process.
        /// </remarks>
        Letters,
    }
}
