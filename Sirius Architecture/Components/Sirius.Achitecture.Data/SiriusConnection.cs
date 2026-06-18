using Sirius.Architecture.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.XPath;

namespace Sirius.Architecture.Data {

    /// <summary>
    /// Represents one database connection to SQL Server, for use in all Sirius code.
    /// </summary>
    [Serializable]
    public abstract class SiriusConnection : ICloneable, IDisposable, ISerializable {

        #region Protected Constants

        // Our standard default timeout.
        protected const int CommandTimeoutDefault = 60;

        #endregion

        #region Private Constants

        private const string PmdaoWarning = "Use your own private copy of the Sirius.Architecture.Data.BackOffice.SiriusConnectionPMDAO class instead. " +
            "Alternatively, use the FromSirius method and bypass PMDAO altogether.";
        private const string UntrustedWarning = "Use an overload that takes a trusted parameter.";

        #endregion

        #region Private Static Fields

        // NOTE: Static data.
        // Cache the auto-generated application name in memory to speed up creating connections.
        // Reading from this string is thread-safe, but initialising it on demand is not,
        // so we protect it with a monitor lock.
        private static string _applicationName;

        // NOTE: Static data.
        // Lock control objects for the static variables to be initialised on demand.
        private static readonly object _applicationNameLock = new object();

        #endregion

        #region Private Fields

        // This data is exposed via public and/or protected properties.
        private int _commandTimeout;
        private bool _disposed = false;
        private SiriusConnectionSchema _schema = SiriusConnectionSchema.Unknown;
        private string _cacheKey;

        #endregion

        #region Constructors - Any

        /// <overloads>
        /// Create a new database connection from an arbitrary connection string.
        /// </overloads>
        /// <summary>
        /// Create a new database connection from an arbitrary connection string.
        /// The transaction behaviour is <see cref="Sirius.Architecture.Data.TransactionBehaviour.SqlServer"/>.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <returns>The connection.</returns>
        /// <remarks>
        /// This is intended for use by ad-hoc utilities, data conversions, and other code that
        /// requires non-standard data access. DO NOT USE for normal Sirius or Swift connections!
        /// </remarks>
        public static SiriusConnection FromAny(string connectionString) {

            return new SiriusConnectionSqlClient(connectionString,
                SiriusConnectionSchema.Unknown,
                CommandTimeoutDefault,
                TransactionBehaviour.SqlServer);

        }

        /// <summary>
        /// Create a new database connection from an arbitrary connection string.
        /// The transaction behaviour is <see cref="Sirius.Architecture.Data.TransactionBehaviour.SqlServer"/>.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <param name="schema">The database schema.</param>
        /// <returns>The connection.</returns>
        /// <remarks>
        /// This is intended for use by ad-hoc utilities, data conversions, and other code that
        /// requires non-standard data access. DO NOT USE for normal Sirius or Swift connections!
        /// </remarks>
        public static SiriusConnection FromAny(string connectionString,
            SiriusConnectionSchema schema) {

            return new SiriusConnectionSqlClient(connectionString,
                schema,
                CommandTimeoutDefault,
                TransactionBehaviour.SqlServer);

        }

        /// <summary>
        /// Create a new database connection from an arbitrary connection string.
        /// The transaction behaviour is <see cref="Sirius.Architecture.Data.TransactionBehaviour.SqlServer"/>.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <param name="commandTimeout">Default command timeout value.</param>
        /// <returns>The connection.</returns>
        /// <remarks>
        /// This is intended for use by ad-hoc utilities, data conversions, and other code that
        /// requires non-standard data access. DO NOT USE for normal Sirius or Swift connections!
        /// </remarks>
        public static SiriusConnection FromAny(string connectionString,
            int commandTimeout) {

            return new SiriusConnectionSqlClient(connectionString,
                SiriusConnectionSchema.Unknown,
                commandTimeout,
                TransactionBehaviour.SqlServer);

        }

        /// <summary>
        /// Create a new database connection from an arbitrary connection string.
        /// The transaction behaviour is <see cref="Sirius.Architecture.Data.TransactionBehaviour.SqlServer"/>.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <param name="schema">The database schema.</param>
        /// <param name="commandTimeout">Default command timeout value.</param>
        /// <returns>The connection.</returns>
        /// <remarks>
        /// This is intended for use by ad-hoc utilities, data conversions, and other code that
        /// requires non-standard data access. DO NOT USE for normal Sirius or Swift connections!
        /// </remarks>
        public static SiriusConnection FromAny(string connectionString,
            SiriusConnectionSchema schema,
            int commandTimeout) {

            return new SiriusConnectionSqlClient(connectionString,
                schema,
                commandTimeout,
                TransactionBehaviour.SqlServer);

        }

        /// <summary>
        /// Create a new database connection from an arbitrary connection string.
        /// The transaction behaviour is as specified.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <param name="schema">The database schema.</param>
        /// <param name="commandTimeout">Default command timeout value.</param>
        /// <param name="transactionBehaviour">Transaction behaviour.</param>
        /// <returns>The connection.</returns>
        /// <remarks>
        /// This is intended for use by ad-hoc utilities, data conversions, and other code that
        /// requires non-standard data access. DO NOT USE for normal Sirius or Swift connections!
        /// </remarks>
        public static SiriusConnection FromAny(string connectionString,
            SiriusConnectionSchema schema,
            int commandTimeout,
            TransactionBehaviour transactionBehaviour) {

            return new SiriusConnectionSqlClient(connectionString,
                schema,
                commandTimeout,
                transactionBehaviour);

        }

        #endregion

        #region Constructors - NamedSource

        /// <overloads>
        /// Create a new connection to a named data source, for use in internal development utilities.
        /// </overloads>
        /// <summary>
        /// Create a new connection to a named data source, for use in internal development utilities.
        /// The transaction behaviour is <see cref="Sirius.Architecture.Data.TransactionBehaviour.SqlServer"/>.
        /// </summary>
        /// <param name="dataSourceName">The data source name to use.</param>
        /// <returns>The connection.</returns>
        /// <remarks>
        /// This is intended for use by ad-hoc utilities, data conversions, and other code that
        /// requires non-standard data access. DO NOT USE for normal Sirius or Swift connections!
        /// </remarks>
        public static SiriusConnection FromNamedSource(string dataSourceName) {

            return FromNamedSource(dataSourceName, SiriusConnectionSchema.Unknown);

        }

        /// <summary>
        /// Create a new connection to a named data source, for use in internal development utilities.
        /// The transaction behaviour is <see cref="Sirius.Architecture.Data.TransactionBehaviour.SqlServer"/>.
        /// </summary>
        /// <param name="dataSourceName">The data source name to use.</param>
        /// <param name="schema">The database schema.</param>
        /// <returns>The connection.</returns>
        /// <remarks>
        /// This is intended for use by ad-hoc utilities, data conversions, and other code that
        /// requires non-standard data access. DO NOT USE for normal Sirius or Swift connections!
        /// </remarks>
        public static SiriusConnection FromNamedSource(string dataSourceName,
            SiriusConnectionSchema schema) {

            return FromNamedSource(dataSourceName, schema, TransactionBehaviour.SqlServer);

        }

        /// <summary>
        /// Create a new connection to a named data source, for use in internal development utilities.
        /// The transaction behaviour is as specified.
        /// </summary>
        /// <param name="dataSourceName">The data source name to use.</param>
        /// <param name="schema">The database schema.</param>
        /// <param name="transactionBehaviour">Transaction behaviour.</param>
        /// <returns>The connection.</returns>
        /// <remarks>
        /// This is intended for use by ad-hoc utilities, data conversions, and other code that
        /// requires non-standard data access. DO NOT USE for normal Sirius or Swift connections!
        /// </remarks>
        public static SiriusConnection FromNamedSource(string dataSourceName,
            SiriusConnectionSchema schema,
            TransactionBehaviour transactionBehaviour) {

            SiriusConnection connection = SiriusConnectionSqlClient.FromSiriusRegKey(dataSourceName,
                schema,
                GetApplicationName(),
                null,
                0,
                transactionBehaviour);

            connection.Schema = schema;
            return connection;

        }

        #endregion

        #region Constructors - Sirius

        /// <overloads>
        /// Create a new connection to the Sirius database, for use in Sirius code.
        /// </overloads>
        /// <summary>
        /// Create a new connection to the Sirius database, for use in Sirius code.
        /// The transaction behaviour is <see cref="Sirius.Architecture.Data.TransactionBehaviour.PMDAO"/>.
        /// </summary>
        /// <returns>The connection.</returns>
        public static SiriusConnection FromSirius() {

            return FromSirius(TransactionBehaviour.PMDAO);

        }

        /// <summary>
        /// Create a new connection to the Sirius database, for use in Sirius code.
        /// The transaction behaviour is as specified.
        /// </summary>
        /// <param name="transactionBehaviour">Transaction behaviour.</param>
        /// <returns>The connection.</returns>
        public static SiriusConnection FromSirius(TransactionBehaviour transactionBehaviour) {

           
                return SiriusConnectionSqlClient.FromPureRegKey("Pure",
                    SiriusConnectionSchema.Sirius,
                    GetApplicationName(),
                    null,
                    0,
                    transactionBehaviour);


        }

        /// <overloads>
        /// Create a new connection to a foreign Sirius 21 database, for use in a Sirius 21 Windows service.
        /// </overloads>
        /// <summary>
        /// Create a new connection to a foreign Sirius 21 database, for use in a Sirius 21 Windows service.
        /// The transaction behaviour is <see cref="Sirius.Architecture.Data.TransactionBehaviour.PMDAO"/>.
        /// </summary>
        /// <param name="serverName">The SQL Server instance name, obtained from the Sirius 21 config file.</param>
        /// <param name="databaseName">The database name, obtained from the Sirius 21 config file.</param>
        /// <returns>The connection.</returns>
        /// <remarks>
        /// This method is intended for use ONLY by Windows service code that has been designed to run
        /// in a Sirius 21 environment, working with multiple customer databases at once. Normal Sirius
        /// code must use the <see cref="FromSirius"/> method instead.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [Obsolete(UntrustedWarning)]
        public static SiriusConnection FromSirius21(string serverName, string databaseName) {

            return FromSirius21(serverName, databaseName, false, TransactionBehaviour.PMDAO);

        }

        /// <summary>
        /// Create a new connection to a foreign Sirius 21 database, for use in a Sirius 21 Windows service.
        /// The transaction behaviour is <see cref="Sirius.Architecture.Data.TransactionBehaviour.PMDAO"/>.
        /// </summary>
        /// <param name="serverName">The SQL Server instance name, obtained from the Sirius 21 config file.</param>
        /// <param name="databaseName">The database name, obtained from the Sirius 21 config file.</param>
        /// <param name="trusted">Trusted connection (use integrated login)?</param>
        /// <returns>The connection.</returns>
        /// <remarks>
        /// This method is intended for use ONLY by Windows service code that has been designed to run
        /// in a Sirius 21 environment, working with multiple customer databases at once. Normal Sirius
        /// code must use the <see cref="FromSirius"/> method instead.
        /// </remarks>
        public static SiriusConnection FromSirius21(string serverName, string databaseName, bool trusted) {

            return FromSirius21(serverName, databaseName, trusted, TransactionBehaviour.PMDAO);

        }

        /// <summary>
        /// Create a new connection to a foreign Sirius 21 database, for use in a Sirius 21 Windows service.
        /// The transaction behaviour is as specified.
        /// </summary>
        /// <param name="serverName">The SQL Server instance name, obtained from the Sirius 21 config file.</param>
        /// <param name="databaseName">The database name, obtained from the Sirius 21 config file.</param>
        /// <param name="transactionBehaviour">Transaction behaviour.</param>
        /// <returns>The connection.</returns>
        /// <remarks>
        /// This method is intended for use ONLY by Windows service code that has been designed to run
        /// in a Sirius 21 environment, working with multiple customer databases at once. Normal Sirius
        /// code must use the <see cref="FromSirius"/> method instead.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [Obsolete(UntrustedWarning)]
        public static SiriusConnection FromSirius21(string serverName, string databaseName,
            TransactionBehaviour transactionBehaviour) {

            return FromSirius21(serverName, databaseName, false, transactionBehaviour);

        }

        /// <summary>
        /// Create a new connection to a foreign Sirius 21 database, for use in a Sirius 21 Windows service.
        /// The transaction behaviour is as specified.
        /// </summary>
        /// <param name="serverName">The SQL Server instance name, obtained from the Sirius 21 config file.</param>
        /// <param name="databaseName">The database name, obtained from the Sirius 21 config file.</param>
        /// <param name="trusted">Trusted connection (use integrated login)?</param>
        /// <param name="transactionBehaviour">Transaction behaviour.</param>
        /// <returns>The connection.</returns>
        /// <remarks>
        /// This method is intended for use ONLY by Windows service code that has been designed to run
        /// in a Sirius 21 environment, working with multiple customer databases at once. Normal Sirius
        /// code must use the <see cref="FromSirius"/> method instead.
        /// </remarks>
        public static SiriusConnection FromSirius21(string serverName, string databaseName, bool trusted,
            TransactionBehaviour transactionBehaviour) {

            return SiriusConnectionSqlClient.From21Info(SiriusConnectionSchema.Sirius,
                serverName,
                databaseName,
                trusted,
                GetApplicationName(),
                null,
                0,
                0,
                CommandTimeoutDefault,
                transactionBehaviour);

        }

        #endregion

        #region Constructors - SiriusViaPMDAO

        /// <overloads>
        /// Create a new connection to the Sirius database, for use in Sirius code that shares dPMDAO transactions with COM components.
        /// </overloads>
        /// <summary>
        /// Create a new connection to the Sirius database, for use in Sirius code that shares dPMDAO transactions with COM components.
        /// The transaction behaviour is <see cref="Sirius.Architecture.Data.TransactionBehaviour.PMDAO"/>.
        /// </summary>
        /// <param name="siriusUserName">Data normally used to initialise dPMDAO.</param>
        /// <param name="sourceID">Data normally used to initialise dPMDAO.</param>
        /// <param name="languageID">Data normally used to initialise dPMDAO.</param>
        /// <param name="callingAppName">Data normally used to initialise dPMDAO.</param>
        /// <returns>The connection.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(PmdaoWarning)]
        public static SiriusConnection FromSiriusViaPMDAO(string siriusUserName,
            short sourceID,
            short languageID,
            string callingAppName) {

            return FromSiriusViaPMDAO(siriusUserName, sourceID, languageID, callingAppName, null, null, null, null);

        }

        /// <summary>
        /// Create a new connection to the Sirius database, for use in Sirius code that shares dPMDAO transactions with COM components.
        /// The transaction behaviour is <see cref="Sirius.Architecture.Data.TransactionBehaviour.PMDAO"/>.
        /// </summary>
        /// <param name="siriusUserName">Data normally used to initialise dPMDAO.</param>
        /// <param name="sourceID">Data normally used to initialise dPMDAO.</param>
        /// <param name="languageID">Data normally used to initialise dPMDAO.</param>
        /// <param name="callingAppName">Data normally used to initialise dPMDAO.</param>
        /// <param name="userName">Data normally used to initialise dPMDAO.</param>
        /// <returns>The connection.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(PmdaoWarning)]
        public static SiriusConnection FromSiriusViaPMDAO(string siriusUserName,
            short sourceID,
            short languageID,
            string callingAppName,
            string userName) {

            return FromSiriusViaPMDAO(siriusUserName, sourceID, languageID, callingAppName, userName, null, null, null);

        }

        /// <summary>
        /// Create a new connection to the Sirius database, for use in Sirius code that shares dPMDAO transactions with COM components.
        /// The transaction behaviour is <see cref="Sirius.Architecture.Data.TransactionBehaviour.PMDAO"/>.
        /// </summary>
        /// <param name="siriusUserName">Data normally used to initialise dPMDAO.</param>
        /// <param name="sourceID">Data normally used to initialise dPMDAO.</param>
        /// <param name="languageID">Data normally used to initialise dPMDAO.</param>
        /// <param name="callingAppName">Data normally used to initialise dPMDAO.</param>
        /// <param name="userName">Data normally used to initialise dPMDAO.</param>
        /// <param name="password">Data normally used to initialise dPMDAO.</param>
        /// <returns>The connection.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(PmdaoWarning)]
        public static SiriusConnection FromSiriusViaPMDAO(string siriusUserName,
            short sourceID,
            short languageID,
            string callingAppName,
            string userName,
            string password) {

            return FromSiriusViaPMDAO(siriusUserName, sourceID, languageID, callingAppName, userName, password, null, null);

        }

        /// <summary>
        /// Create a new connection to the Sirius database, for use in Sirius code that shares dPMDAO transactions with COM components.
        /// The transaction behaviour is <see cref="Sirius.Architecture.Data.TransactionBehaviour.PMDAO"/>.
        /// </summary>
        /// <param name="siriusUserName">Data normally used to initialise dPMDAO.</param>
        /// <param name="sourceID">Data normally used to initialise dPMDAO.</param>
        /// <param name="languageID">Data normally used to initialise dPMDAO.</param>
        /// <param name="callingAppName">Data normally used to initialise dPMDAO.</param>
        /// <param name="userName">Data normally used to initialise dPMDAO.</param>
        /// <param name="password">Data normally used to initialise dPMDAO.</param>
        /// <param name="dataSourceName">Data normally used to initialise dPMDAO.</param>
        /// <returns>The connection.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(PmdaoWarning)]
        public static SiriusConnection FromSiriusViaPMDAO(string siriusUserName,
            short sourceID,
            short languageID,
            string callingAppName,
            string userName,
            string password,
            string dataSourceName) {

            return FromSiriusViaPMDAO(siriusUserName, sourceID, languageID, callingAppName, userName, password, dataSourceName, null);

        }

        /// <summary>
        /// Create a new connection to the Sirius database, for use in Sirius code that shares dPMDAO transactions with COM components.
        /// The transaction behaviour is <see cref="Sirius.Architecture.Data.TransactionBehaviour.PMDAO"/>.
        /// </summary>
        /// <param name="siriusUserName">Data normally used to initialise dPMDAO.</param>
        /// <param name="sourceID">Data normally used to initialise dPMDAO.</param>
        /// <param name="languageID">Data normally used to initialise dPMDAO.</param>
        /// <param name="callingAppName">Data normally used to initialise dPMDAO.</param>
        /// <param name="userName">Data normally used to initialise dPMDAO.</param>
        /// <param name="password">Data normally used to initialise dPMDAO.</param>
        /// <param name="dataSourceName">Data normally used to initialise dPMDAO.</param>
        /// <param name="databaseName">Data normally used to initialise dPMDAO.</param>
        /// <returns>The connection.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(PmdaoWarning)]
        public static SiriusConnection FromSiriusViaPMDAO(string siriusUserName,
            short sourceID,
            short languageID,
            string callingAppName,
            string userName,
            string password,
            string dataSourceName,
            string databaseName) {

            const string backOfficeAssemblyNameSimple = "Sirius.Architecture.Data.BackOffice";
            const string backOfficeConnectionTypeName = backOfficeAssemblyNameSimple + ".SiriusConnectionPMDAO";

            // Load the BackOffice assembly into the current app domain on first use.
            // For safety, the BackOffice assembly must have the same version and strong name as this assembly.
            var thisAssemblyName = Assembly.GetExecutingAssembly().GetName();
            var backOfficeAssemblyName = new AssemblyName {
                Name = backOfficeAssemblyNameSimple,
                KeyPair = thisAssemblyName.KeyPair,
                Version = thisAssemblyName.Version,
                VersionCompatibility = thisAssemblyName.VersionCompatibility
            };
            var backOfficeAssembly = Assembly.Load(backOfficeAssemblyName);

            // Create the connection object late-bound. Throw an exception if unable to create.
            object connection = backOfficeAssembly.CreateInstance(backOfficeConnectionTypeName,
                false,
                BindingFlags.Default,
                null,
                new object[] {
                    siriusUserName,
                    sourceID,
                    languageID,
                    callingAppName,
                    userName,
                    password,
                    dataSourceName,
                    databaseName
                },
                null,
                null);

            if(connection == null) {
                throw new TypeLoadException(string.Format(Properties.Resources.ErrorCannotLoadType, backOfficeConnectionTypeName, backOfficeAssemblyNameSimple));
            }

            return (SiriusConnection) connection;

        }

        #endregion

        #region Constructors - SiriusViaSwift

        /// <summary>
        /// Create a new connection to the Sirius database, for use in Swift code.
        /// The transaction behaviour is <see cref="Sirius.Architecture.Data.TransactionBehaviour.SqlServer"/>.
        /// </summary>
        /// <returns>The connection.</returns>
        public static SiriusConnection FromSiriusViaSwift() {

            return SiriusConnectionSqlClient.FromSwiftRegKey(SiriusConnectionSchema.Sirius,
                GetApplicationName(),
                null,
                0,
                TransactionBehaviour.SqlServer);

        }

        #endregion

        #region Constructors - Swift

        /// <summary>
        /// Create a new connection to the Swift database, for use in Swift code.
        /// The transaction behaviour is <see cref="Sirius.Architecture.Data.TransactionBehaviour.SqlServer"/>.
        /// </summary>
        /// <returns>The connection.</returns>
        public static SiriusConnection FromSwift() {

            return SiriusConnectionSqlClient.FromSwiftRegKey(SiriusConnectionSchema.Swift,
                GetApplicationName(),
                null,
                0,
                TransactionBehaviour.SqlServer);

        }

        /// <overloads>
        /// Create a new connection to a foreign Swift 21 database, for use in a Swift 21 Windows service.
        /// </overloads>
        /// <summary>
        /// Create a new connection to a foreign Swift 21 database, for use in a Swift 21 Windows service.
        /// The transaction behaviour is <see cref="Sirius.Architecture.Data.TransactionBehaviour.SqlServer"/>.
        /// </summary>
        /// <param name="serverName">The SQL Server instance name, obtained from the Swift 21 config file.</param>
        /// <param name="databaseName">The database name, obtained from the Swift 21 config file.</param>
        /// <returns>The connection.</returns>
        /// <remarks>
        /// This method is intended for use ONLY by Windows service code that has been designed to run
        /// in a Swift 21 environment, working with multiple customer databases at once. Normal Swift
        /// code must use the <see cref="FromSwift"/> method instead.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [Obsolete(UntrustedWarning)]
        public static SiriusConnection FromSwift21(string serverName, string databaseName) {

            return FromSwift21(serverName, databaseName, false);

        }

        /// <summary>
        /// Create a new connection to a foreign Swift 21 database, for use in a Swift 21 Windows service.
        /// The transaction behaviour is <see cref="Sirius.Architecture.Data.TransactionBehaviour.SqlServer"/>.
        /// </summary>
        /// <param name="serverName">The SQL Server instance name, obtained from the Swift 21 config file.</param>
        /// <param name="databaseName">The database name, obtained from the Swift 21 config file.</param>
        /// <param name="trusted">Trusted connection (use integrated login)?</param>
        /// <returns>The connection.</returns>
        /// <remarks>
        /// This method is intended for use ONLY by Windows service code that has been designed to run
        /// in a Swift 21 environment, working with multiple customer databases at once. Normal Swift
        /// code must use the <see cref="FromSwift"/> method instead.
        /// </remarks>
        public static SiriusConnection FromSwift21(string serverName, string databaseName, bool trusted) {

            return SiriusConnectionSqlClient.From21Info(SiriusConnectionSchema.Swift,
                serverName,
                databaseName,
                trusted,
                GetApplicationName(),
                null,
                0,
                0,
                CommandTimeoutDefault,
                TransactionBehaviour.SqlServer);

        }

        #endregion

        #region Constructors - Letters

        /// <summary>
        /// Create a new connection to the Letters database, for use in Swift code.
        /// The transaction behaviour is <see cref="Sirius.Architecture.Data.TransactionBehaviour.SqlServer"/>.
        /// </summary>
        /// <returns>The connection.</returns>
        public static SiriusConnection FromLetters() {

            return SiriusConnectionSqlClient.FromSwiftRegKey(SiriusConnectionSchema.Letters,
                GetApplicationName(),
                null,
                0,
                TransactionBehaviour.SqlServer);

        }

        /// <summary>
        /// Create a new connection to a foreign Swift 21 Letters database, for use in a Swift 21 Windows service.
        /// The transaction behaviour is <see cref="Sirius.Architecture.Data.TransactionBehaviour.SqlServer"/>.
        /// </summary>
        /// <param name="serverName">The SQL Server instance name, obtained from the Swift 21 config file.</param>
        /// <param name="databaseName">The database name, obtained from the Swift 21 config file.</param>
        /// <param name="trusted">Trusted connection (use integrated login)?</param>
        /// <returns>The connection.</returns>
        /// <remarks>
        /// This method is intended for use ONLY by Windows service code that has been designed to run
        /// in a Swift 21 environment, working with multiple customer databases at once. Normal Swift
        /// code must use the <see cref="FromSwift"/> method instead.
        /// </remarks>
        public static SiriusConnection FromLetters21(string serverName, string databaseName, bool trusted) {

            return SiriusConnectionSqlClient.From21Info(SiriusConnectionSchema.Letters,
                serverName,
                databaseName,
                trusted,
                GetApplicationName(),
                null,
                0,
                0,
                CommandTimeoutDefault,
                TransactionBehaviour.SqlServer);

        }

        #endregion

        #region Constructors - Protected

        /// <summary>
        /// Allow only derived classes to instantiate this class directly.
        /// </summary>
        protected SiriusConnection() {
        }

        /// <summary>
        /// Initialise a new instance of the class with serialized data.
        /// </summary>
        /// <param name="info">Serialized object data</param>
        /// <param name="context">Contextual information about the source or destination</param>
        protected SiriusConnection(SerializationInfo info, StreamingContext context) {

            if(info == null) {
                throw new ArgumentNullException("info");
            }
            _commandTimeout = info.GetInt32("_commandTimeout");
            _disposed = info.GetBoolean("_disposed");
            _schema = (SiriusConnectionSchema) info.GetValue("_schema", typeof(SiriusConnectionSchema));
            _cacheKey = info.GetString("_cacheKey");
        }

        #endregion

        #region ISerializable Members

        /// <summary>
        /// Serialise this instance of the class to serialised data.
        /// </summary>
        /// <param name="info">Serialized object data</param>
        /// <param name="context">Contextual information about the source or destination</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) {

            if(info == null) {
                throw new ArgumentNullException("info");
            }
            info.AddValue("_commandTimeout", _commandTimeout);
            info.AddValue("_disposed", _disposed);
            info.AddValue("_schema", _schema, typeof(SiriusConnectionSchema));
            info.AddValue("_cacheKey", _cacheKey);
        }

        #endregion

        #region Finalizers

        /// <summary>
        /// Release unmanaged resources and performs other cleanup operations before the object is reclaimed by garbage collection.
        /// </summary>
        /// <remarks>
        /// If any transactions are still pending, they will all be rolled back before the connection is closed.
        /// </remarks>
        ~SiriusConnection() {
            Dispose(false);
        }

        /// <overloads>
        /// Release all resources used by this object.
        /// </overloads>
        /// <summary>
        /// Release all resources used by this object.
        /// </summary>
        /// <remarks>
        /// If any transactions are still pending, they will all be rolled back before the connection is closed.
        /// </remarks>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release all unmanaged resources used by this object and optionally release managed resources as well.
        /// </summary>
        /// <param name="disposing">Release managed resources?</param>
        /// <remarks>
        /// If any transactions are still pending, they will all be rolled back before the connection is closed.
        /// </remarks>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// Flag provided to derived classes to assist them in writing <c>Dispose</c> methods.
        /// </summary>
        protected bool Disposed {
            get {
                return _disposed;
            }
            set {
                _disposed = value;
            }
        }

        /// <summary>
        /// Throw an exception if the calling code tries to use this object after it has been disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The disposed flag is true.</exception>
        protected void DisposedCheck() {

            if(_disposed) {
                throw new ObjectDisposedException(this.GetType().FullName, Properties.Resources.ObjectDisposedExceptionMessage);
            }

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The default <see cref="CommandTimeout"/> value applied to all new <see cref="SiriusCommand"/> objects created via this wrapper class.
        /// </summary>
        public int CommandTimeout {
            get {
                DisposedCheck();
                return _commandTimeout;
            }
            set {
                DisposedCheck();
                _commandTimeout = value;
            }
        }

        /// <summary>
        /// The database schema.
        /// </summary>
        public SiriusConnectionSchema Schema {
            get {
                DisposedCheck();
                return _schema;
            }
            protected set {
                DisposedCheck();
                _schema = value;
            }
        }

        /// <summary>
        /// A string suitable for using as a cache lookup key in situations where data must be stored on a per-database basis.
        /// </summary>
        public string CacheKey {
            get {
                return _cacheKey;
            }
            protected set {
                _cacheKey = value;
            }
        }

        #endregion

        #region Public Abstract Properties

        /// <summary>
        /// The exact behaviour of the begin / commit / rollback transaction methods.
        /// </summary>
        public abstract TransactionBehaviour TransactionBehaviour { get; }

        /// <summary>
        /// The number of logical nested transactions currently open.
        /// </summary>
        public abstract int TransactionCount { get; }

        /// <summary>
        /// The wrapped <see cref="SqlConnection"/> object, if applicable.
        /// </summary>
        /// <exception cref="NotSupportedException">The property is not supported for this type of connection.</exception>
        public abstract SqlConnection SqlConnection { get; }

        /// <summary>
        /// The wrapped <see cref="SqlTransaction"/> object, if applicable.
        /// </summary>
        /// <exception cref="NotSupportedException">The property is not supported for this type of connection.</exception>
        public abstract SqlTransaction SqlTransaction { get; }

        /// <summary>
        /// The wrapped <c>dPMDAO.Database</c> object, if applicable.
        /// </summary>
        /// <exception cref="NotSupportedException">The property is not supported for this type of connection.</exception>
        public abstract object PMDAODatabase { get; }

        #endregion

        #region ICloneable Methods

        /// <summary>
        /// Creates a new independent connection with the same connection information as this connection.
        /// The transaction state is not copied; the new connection will have <see cref="TransactionCount"/> = 0.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        /// <exception cref="NotSupportedException">The operation is not supported for this type of connection.</exception>
        object ICloneable.Clone() {

            return Clone();

        }

        #endregion

        #region Public Abstract Methods - Clone

        /// <summary>
        /// Creates a new independent connection with the same connection information as this connection.
        /// The transaction state is not copied; the new connection will have <see cref="TransactionCount"/> = 0.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        /// <exception cref="NotSupportedException">The operation is not supported for this type of connection.</exception>
        public abstract SiriusConnection Clone();

        #endregion

        #region Public Abstract Methods - Transactions

        /// <summary>
        /// Begin a nested transaction on the connection.
        /// </summary>
        public abstract void BeginTransaction();

        /// <summary>
        /// Commit a nested transaction on the connection. The exact behaviour is dependent on the <see cref="Sirius.Architecture.Data.TransactionBehaviour"/> setting.
        /// </summary>
        public abstract void CommitTransaction();

        /// <summary>
        /// Roll back a nested transaction on the connection. The exact behaviour is dependent on the <see cref="Sirius.Architecture.Data.TransactionBehaviour"/> setting.
        /// </summary>
        public abstract void RollbackTransaction();

        #endregion

        #region Public Abstract Methods - Execute Command Statelessly

        /// <summary>
        /// Execute the command without expecting a resultset or stream to be returned.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>The number of rows affected.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public abstract int ExecuteNonQuery(SiriusCommand command);

        /// <summary>
        /// Execute the command and return the first column of the first row in the resultset returned by the query.
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>The first column of the first row in the resultset, or a null reference if the resultset is empty.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        /// <remarks>
        /// This method is included ONLY to support some existing stored procedures. DO NOT use this for calling
        /// new stored procedures because single values should be returned in output parameters, not in a resultset.
        /// </remarks>
        public abstract object ExecuteScalar(SiriusCommand command);

        /// <overloads>
        /// Execute the command and return a resultset in a <see cref="DataTable"/>.
        /// </overloads>
        /// <summary>
        /// Execute the command and return a resultset in a new <see cref="DataTable"/>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>A new <see cref="DataTable"/> containing the resultset data.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public abstract DataTable ExecuteDataTable(SiriusCommand command);

        /// <summary>
        /// Execute the command and merge a resultset into an existing <see cref="DataTable"/>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="results">The <see cref="DataTable"/> to fill with data.</param>
        /// <returns>The number of rows successfully added or refreshed.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public abstract int ExecuteDataTable(SiriusCommand command, DataTable results);

        /// <summary>
        /// Execute the command and merge a resultset into an existing <see cref="DataTable"/>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="adapter">The data adapter to use for merging the data.</param>
        /// <param name="results">The <see cref="DataTable"/> to fill with data.</param>
        /// <returns>The number of rows successfully added or refreshed.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public abstract int ExecuteDataTable(SiriusCommand command, DbDataAdapter adapter, DataTable results);

        /// <overloads>
        /// Execute the command and return multiple resultsets in a new <see cref="DataSet"/>.
        /// </overloads>
        /// <summary>
        /// Execute the command and return multiple resultsets in a new <see cref="DataSet"/>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="tableName">The name to give the <see cref="DataTable"/> containing the first resultset. Any additional resultsets are named sequentially based on this name.</param>
        /// <returns>A new <see cref="DataSet"/> containing the resultset data.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public abstract DataSet ExecuteDataSet(SiriusCommand command, string tableName);

        /// <summary>
        /// Execute the command and merge multiple resultsets into an existing <see cref="DataSet"/>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="results">The <see cref="DataSet"/> to fill with data.</param>
        /// <param name="tableName">The name to give the <see cref="DataTable"/> containing the first resultset. Any additional resultsets are named sequentially based on this name.</param>
        /// <returns>The number of rows successfully added or refreshed.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public abstract int ExecuteDataSet(SiriusCommand command, DataSet results, string tableName);

        /// <summary>
        /// Execute the command and merge multiple resultsets into an existing <see cref="DataSet"/>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="adapter">The data adapter to use for merging the data.</param>
        /// <param name="results">The <see cref="DataSet"/> to fill with data.</param>
        /// <param name="tableName">The name to give the <see cref="DataTable"/> containing the first resultset. Any additional resultsets are named sequentially based on this name.</param>
        /// <returns>The number of rows successfully added or refreshed.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public abstract int ExecuteDataSet(SiriusCommand command, DbDataAdapter adapter, DataSet results, string tableName);

        /// <overloads>
        /// Execute the command and return a resultset as a list of business objects.
        /// </overloads>
        /// <summary>
        /// Execute the command and return a resultset as a list of business objects.
        /// </summary>
        /// <typeparam name="T">The business class to populate. This must implement the <see cref="IDataReadable"/> interface.</typeparam>
        /// <param name="command">The command to execute.</param>
        /// <returns>A new <see cref="List&lt;T&gt;"/> containing the resultset data.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public abstract List<T> ExecuteList<T>(SiriusCommand command) where T : IDataReadable, new();

        /// <summary>
        /// Execute the command and return a resultset as a list of business objects.
        /// </summary>
        /// <typeparam name="T">The business class to populate.</typeparam>
        /// <param name="command">The command to execute.</param>
        /// <param name="convert">A method to convert each data row into an instance of the business class.</param>
        /// <returns>A new <see cref="List&lt;T&gt;"/> containing the resultset data.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public abstract List<T> ExecuteList<T>(SiriusCommand command, Converter<IDataRecord, T> convert);

        /// <summary>
        /// Execute the command and return XML data as a string.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>A <see cref="String"/> containing the XML output of the command.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public abstract string ExecuteXmlText(SiriusCommand command);

        /// <summary>
        /// Execute the command and return XML data as a navigable document.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>An <see cref="XPathDocument"/> containing the XML output of the command.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public abstract XPathDocument ExecuteXPathDocument(SiriusCommand command);

        #endregion

        #region Public Abstract Methods - Connection State

        /// <summary>
        /// Open the connection (only if it is currently closed) and hold it open until <see cref="EndStateful"/> is called.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public abstract void BeginStateful();

        /// <summary>
        /// Close the connection (only if it is currently open and no transactions are active).
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public abstract void EndStateful();

        #endregion

        #region Protected Abstract Methods - Connection State

        /// <summary>
        /// Open the connection (only if it is currently closed).
        /// </summary>
        protected abstract void OpenConnection();

        /// <summary>
        /// Close the connection (only if it is currently open and no transactions are active).
        /// </summary>
        protected abstract void CloseConnection();

        #endregion

        #region Protected Shared Properties

        /// <summary>
        /// Provide access to a standard resource string for use by derived classes.
        /// </summary>
        protected static string BadParameterMessage {
            get {
                return Properties.Resources.BadParameterMessage;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Hard-coded exception handling rules for the statement that executes the command.
        /// </summary>
        /// <param name="ex">The exception that has been caught.</param>
        /// <param name="command">The command being executed.</param>
        /// <returns>True if the exception should be re-thrown, otherwise false.</returns>
        protected bool HandleExceptionFromParameters(Exception ex, SiriusCommand command) {

            // NB: This code must catch all exceptions thrown from the ValidateParameterValue method.
            if(ex is InvalidCastException) {
                // Wrap the error thrown by our type checking code.
                throw new DatabaseException(ex.Message, ex, null, this, command);
            } else if(ex is InvalidOperationException) {
                // Wrap the error thrown by our direction checking code.
                throw new DatabaseException(ex.Message, ex, null, this, command);
            } else if(ex is OverflowException) {
                // Wrap the error thrown by our size checking code.
                throw new DatabaseException(ex.Message, ex, null, this, command);
            } else {
                return true;
            }

        }

        #endregion

        #region Protected Shared Methods

        /// <summary>
        /// Validate a parameter value before the command is executed, and throw a helpful exception if it fails.
        /// This prevents the underlying data source throwing inconsistent and unhelpful exceptions.
        /// </summary>
        /// <param name="parameter">The parameter object to check.</param>
        /// <param name="behaviour">The checking rules to apply.</param>
        /// <exception cref="InvalidCastException">The value's type is inconsistent with the parameter definition.</exception>
        /// <exception cref="OverflowException">The value's size is inconsistent with the parameter definition.</exception>
        protected static void ValidateParameterValue(SqlParameter parameter, SiriusCommandBehaviour behaviour) {

            // If the value is NULL or missing, then it must be valid regardless of the type.
            // This assumes that missing values are dealt with before the command is executed,
            // otherwise nasty consequences might happen!
            object value = parameter.Value;
            if(value == null || value == DBNull.Value || value == Missing.Value || (value is INullable && ((INullable) value).IsNull)) {
                return;
            }

            bool checkRange;
            bool checkSize;
            ValidateParameterType(parameter, behaviour, out checkRange, out checkSize);
            ValidateParameterDirection(parameter, behaviour);
            if(checkRange) {
                ValidateParameterRange(parameter, behaviour);
            }
            if(checkSize) {
                ValidateParameterSize(parameter, behaviour);
            }

        }

        #endregion

        #region Private Static Methods

        private static string GetApplicationName() {

            // Get the application name only once on first use. It's safe to do this because assembly
            // version info will never change during the lifetime of an app domain in memory.
            lock(_applicationNameLock) {
                if(_applicationName == null) {
                    _applicationName = AssemblyVersionInfo.FromEntryAssembly().DisplayTitleAndVersion;
                }
            }
            return _applicationName;

        }

        private static void ValidateParameterType(SqlParameter parameter, SiriusCommandBehaviour behaviour,
            out bool checkRange, out bool checkSize) {

            object value = parameter.Value;

            // The type of the value must match the type of the parameter fairly closely,
            // allowing room for some common casts.
            bool allowStringValuesForNonStringTypes = (behaviour & SiriusCommandBehaviour.AllowStringValuesForNonStringTypes) != 0;
            bool goodType = false;
            checkRange = false;
            checkSize = false;

            switch(parameter.SqlDbType) {
            case SqlDbType.Bit:
            case SqlDbType.TinyInt:
            case SqlDbType.SmallInt:
            case SqlDbType.Int:
            case SqlDbType.BigInt:
                if(value is Boolean || value is Byte || value is Int16 || value is Int32 || value is Int64 || value is SqlBoolean || value is SqlByte || value is SqlInt16 || value is SqlInt32 || value is SqlInt64 || value is Enum) {
                    goodType = true;
                } else if(value is String) {
                    goodType = allowStringValuesForNonStringTypes;
                } else if(value is SqlString) {
                    goodType = allowStringValuesForNonStringTypes;
                    if(goodType) {
                        parameter.Value = value.ToString();
                    }
                }
                break;
            case SqlDbType.Real:
            case SqlDbType.Float:
            case SqlDbType.SmallMoney:
            case SqlDbType.Money:
            case SqlDbType.Decimal:
                if(value is Single || value is Double || value is Decimal || value is SqlSingle || value is SqlDouble || value is SqlMoney || value is SqlDecimal) {
                    goodType = true;
                    checkRange = true;
                } else if(value is String) {
                    goodType = allowStringValuesForNonStringTypes;
                } else if(value is SqlString) {
                    goodType = allowStringValuesForNonStringTypes;
                    if(goodType) {
                        parameter.Value = value.ToString();
                    }
                }
                break;
            case SqlDbType.Time:
            case SqlDbType.Date:
            case SqlDbType.SmallDateTime:
            case SqlDbType.DateTime:
            case SqlDbType.DateTime2:
            case SqlDbType.DateTimeOffset:
                if(value is DateTime || value is DateTimeOffset || value is TimeSpan || value is SqlDateTime) {
                    goodType = true;
                } else if(value is String) {
                    goodType = allowStringValuesForNonStringTypes;
                } else if(value is SqlString) {
                    goodType = allowStringValuesForNonStringTypes;
                    if(goodType) {
                        parameter.Value = value.ToString();
                    }
                }
                break;
            case SqlDbType.UniqueIdentifier:
                if(value is Guid || value is SqlGuid) {
                    goodType = true;
                }
                break;
            case SqlDbType.Timestamp:
            case SqlDbType.Binary:
            case SqlDbType.VarBinary:
                if(value is Byte[] || value is SqlBinary) {
                    goodType = true;
                    checkSize = true;
                }
                break;
            case SqlDbType.Image:
                if(value is Byte[] || value is SqlBinary) {
                    goodType = true;
                }
                break;
            case SqlDbType.Char:
            case SqlDbType.NChar:
            case SqlDbType.VarChar:
            case SqlDbType.NVarChar:
                if(value is String || value is SqlString) {
                    goodType = true;
                    checkSize = true;
                }
                break;
            case SqlDbType.Text:
            case SqlDbType.NText:
                if(value is String || value is SqlString) {
                    goodType = true;
                }
                break;
            default:
                // If we don't recognise the data type, don't bother checking it. This is because Microsoft
                // are always adding new types in each version of SQL Server, and we don't want to have to
                // rush out a new release of SA.NET every time this happens. This parameter validation is
                // only being done as a courtesy anyway, to improve compatibility between the SqlClient and
                // PMDAO connection classes.
                goodType = true;
                break;
            }

            if(!goodType) {
                // NB: This code must throw an exception caught by the HandleExceptionFromParameters method.
                throw new InvalidCastException(string.Format(Properties.Resources.BadParameterTypeMessage, parameter.ParameterName, value.GetType()));
            }

        }

        private static void ValidateParameterDirection(SqlParameter parameter, SiriusCommandBehaviour behaviour) {

            switch(parameter.SqlDbType) {
            case SqlDbType.Image:
            case SqlDbType.Text:
            case SqlDbType.NText:
                // The original BLOB types cannot be returned as output, even in SQL 2005.
                if(parameter.Direction != ParameterDirection.Input) {
                    // NB: This code must throw an exception caught by the HandleExceptionFromParameters method.
                    throw new InvalidOperationException(string.Format(Properties.Resources.BadParameterDirectionMessage, parameter.ParameterName));
                }
                break;
            }

        }

        private static void ValidateParameterRange(SqlParameter parameter, SiriusCommandBehaviour behaviour) {

            object value = parameter.Value;

            switch(parameter.SqlDbType) {
            case SqlDbType.Real:
                // Explicitly check for IEEE floating-point overflow, because SQL 2005 complains even though SQL 2000 doesn't.
                if(value is Double) {
                    if(Math.Abs((Double) value) > Single.MaxValue) {
                        // NB: This code must throw an exception caught by the HandleExceptionFromParameters method.
                        throw new OverflowException(string.Format(Properties.Resources.BadParameterSizeMessage, parameter.ParameterName, value.GetType()));
                    }
                } else if(value is SqlDouble) {
                    if(Math.Abs(((SqlDouble) value).Value) > Single.MaxValue) {
                        // NB: This code must throw an exception caught by the HandleExceptionFromParameters method.
                        throw new OverflowException(string.Format(Properties.Resources.BadParameterSizeMessage, parameter.ParameterName, value.GetType()));
                    }
                }
                break;
            }

        }

        private static void ValidateParameterSize(SqlParameter parameter, SiriusCommandBehaviour behaviour) {

            int parameterSize = parameter.Size;
            if(parameter.SqlDbType == SqlDbType.Timestamp) {
                // For timestamps, the size seems to automatically adjust to the value length
                // even though it was fixed beforehand, so compensate for that here.
                parameterSize = SiriusCommand.TimestampSize;
            }

            // Read the value length for checking.
            object value = parameter.Value;
            int valueLength = 0;
            if(value is Byte[]) {
                valueLength = ((Byte[]) value).Length;
            } else if(value is string) {
                valueLength = ((String) value).Length;
            } else if(value is SqlBinary) {
                valueLength = ((SqlBinary) value).Value.Length;
            } else if(value is SqlString) {
                valueLength = ((SqlString) value).Value.Length;
            }

            // Check the size.
            if(valueLength > parameterSize) {
                // TODO: P5 - Implement SiriusCommandBehaviour.SilentlyTruncateOverMaximumSize if there is any demand for it.
                // if((behaviour & SiriusCommandBehaviour.SilentlyTruncateOverMaximumSize) != 0) {
                //     // Truncate the value to the specified length.
                //     if(value is Byte[]) {
                //         Byte[] valueNew = new Byte[parameterSize];
                //         Array.Copy(((Byte[]) value), valueNew, parameterSize);
                //         parameter.Value = valueNew;
                //     } else if(value is String) {
                //         parameter.Value = ((String) value).Substring(0, parameterSize);
                //     } else if(value is SqlBinary) {
                //         Byte[] valueNew = new Byte[parameterSize];
                //         Array.Copy(((SqlBinary) value).Value, valueNew, parameterSize);
                //         parameter.Value = new SqlBinary(valueNew);
                //     } else if(value is SqlString) {
                //         parameter.Value = new SqlString(((SqlString) value).Value.Substring(0, parameterSize));
                //     }
                // } else {
                // NB: This code must throw an exception caught by the HandleExceptionFromParameters method.
                throw new OverflowException(string.Format(Properties.Resources.BadParameterSizeMessage, parameter.ParameterName, valueLength));
                // }
            }

        }

        #endregion

    }

}
