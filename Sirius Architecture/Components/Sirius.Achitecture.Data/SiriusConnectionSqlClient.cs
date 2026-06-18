using Microsoft.SqlServer.Server;
using Microsoft.Win32;
using Sirius.Architecture.Configuration.Local;
using Sirius.Architecture.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.XPath;


namespace Sirius.Architecture.Data {

    /// <summary>
    /// This class supports <see cref="SiriusConnection"/>, and is not intended to be used directly in your code.
    /// </summary>
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal class SiriusConnectionSqlClient : SiriusConnection {

        #region Private Constants

        private const string PureRegKeyPath = @"SOFTWARE\Pure\Architecture\Server\Databases";
        private const string SiriusRegKeyPath = @"SOFTWARE\PM\SiriusArchitecture\Server\Databases";
        private const string SwiftRegKeyPath = @"SOFTWARE\PM\Swift\Database";

        private const string SiriusLoginName = "SIRIUS";
        private const string SiriusLoginPassword = "$1R1U5";
        private const string SwiftLoginName = "Swift";
        private const string SwiftLoginPassword = "hy4u8hv5495tyc92y637dx45t5c46y";
        private const string LettersLoginName = "SwiftWebCore";
        private const string LettersLoginPassword = "SwiftWebCore";

        private const string ErrorNotDefined = "NotDefined";

        #endregion

        #region Private Fields

        private string _connectionString;
        private SqlConnection _connection;
        private SqlTransaction _transaction;
        private TransactionBehaviour _transactionBehaviour;
        private int _transactionCount = 0;
        private bool _stateful = false;
        private static readonly DataProtectionScope kScope = DataProtectionScope.LocalMachine;
        #endregion

        #region Constructors - SSP Pure Config

        /// <summary>
        /// Test whether the specified SSP Pure database registry key exists.
        /// </summary>
        public static bool TestPureRegKey(string name) {

            string fullPath = PureRegKeyPath + "\\" + name;
            // Open the registry key for 64-bit view
            using (var databaseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                                                 .OpenSubKey(fullPath))
            {
                return databaseKey != null;
            }
        }

        /// <summary>
        /// Create a new database connection from one of the SSP Pure database registry keys.
        /// </summary>
        public static SiriusConnectionSqlClient FromPureRegKey(string name,
            SiriusConnectionSchema schema,
            string applicationName,
            string workstationName,
            int packetSize,
            TransactionBehaviour transactionBehaviour) {

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            int commandTimeout = 0;

                builder.DataSource = GetConnectionDetails("Server");
                builder.InitialCatalog = GetConnectionDetails("Database");
                if (builder.DataSource.Length == 0) {
                    throw new InvalidOperationException(string.Format(Properties.Resources.ErrorRegValueNotDefined,
                        "server", "server")) {
                            Data = { { ErrorNotDefined, true } }
                        };
                }
                if(builder.InitialCatalog.Length == 0) {
                    throw new InvalidOperationException(string.Format(Properties.Resources.ErrorRegValueNotDefined,
                        "database", "database")) {
                            Data = { { ErrorNotDefined, true } }
                        };
                }

            // Read the login credentials.
                string sTrusted = GetConnectionDetails("Trusted");
                    if (sTrusted == "1")
                {
                    builder.IntegratedSecurity = true;                
                } 

                if(!builder.IntegratedSecurity) {
                    builder.UserID = GetUserAndPassword("SQLLogin");
                    builder.Password = GetUserAndPassword("SecureKey");
                    
                }

                // Read the optional bits.
                if(!string.IsNullOrEmpty(applicationName)) {
                    builder.ApplicationName = applicationName;
                }
                if(!string.IsNullOrEmpty(workstationName)) {
                    builder.WorkstationID = workstationName;
                }
                if(packetSize != 0) {
                    builder.PacketSize = packetSize;
                }

                commandTimeout = CommandTimeoutDefault;


            return new SiriusConnectionSqlClient(builder.ConnectionString, schema, commandTimeout, transactionBehaviour);
               
        }


public static string GetConnectionDetails(string attributeName)
        {
            string assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string PMDIR = Path.GetPathRoot(assemblyDir);

            XmlDocument xmldoc = new XmlDocument();
            string xmlFileName = PMDIR + "\\Pure\\PureConfiguration.xml"; // ConfigurationManager.AppSettings["RegistryPath"];

            string xmlContent = File.ReadAllText(xmlFileName);

            if (xmldoc == null)
            {
                xmldoc = new XmlDocument();
            }

            xmldoc.LoadXml(xmlContent);
            XmlNode node = xmldoc.SelectSingleNode("Pure");

            if (node != null && node.Attributes[attributeName] != null)
            {
                try
                {
                    return node.Attributes[attributeName].Value;
                }
                catch
                {
                    return "";
                }
            }

            return "";
        }

        private static string GetUserAndPassword(string attributeName)
        {

            try
            {
                // use the variable, not a hardcoded string
                string attributeValue = GetConnectionDetails(attributeName);

                byte[] aKeys = Encoding.ASCII.GetBytes("SiriusArchitecture");

                return Decrypt(attributeValue, aKeys);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("GetUserAndPassword Failed", ex);
            }
        }

        private static string Decrypt(string sCipher, byte[] aKeys)
        {
            if (string.IsNullOrEmpty(sCipher))
            {
                return "";
            }

            if (sCipher == null)
            {
                throw new ArgumentNullException(nameof(sCipher));
            }

            // parse base64 string
            byte[] aData = Convert.FromBase64String(sCipher);

            // decrypt data
            byte[] aDecrypted = ProtectedData.Unprotect(aData, aKeys, kScope);
            return Encoding.Unicode.GetString(aDecrypted);
        }
        #endregion

        #region Constructors - Sirius Config

        /// <summary>
        /// Test whether the specified Sirius database registry key exists.
        /// </summary>
        public static bool TestSiriusRegKey(string name) {
            using(var databaseKey = Registry.LocalMachine.OpenSubKey32(SiriusRegKeyPath + "\\" + name)) {
                return databaseKey != null;
            }
        }

        /// <summary>
        /// Create a new database connection from one of the Sirius database registry keys.
        /// </summary>
        public static SiriusConnectionSqlClient FromSiriusRegKey(string name,
            SiriusConnectionSchema schema,
            string applicationName,
            string workstationName,
            int packetSize,
            TransactionBehaviour transactionBehaviour) {

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            int commandTimeout = 0;

            using(RegistryKey databaseKey = Registry.LocalMachine.OpenSubKey32(SiriusRegKeyPath + "\\" + name)) {

                // Read and check the mandatory values.
                if(databaseKey == null) {
                    throw new InvalidOperationException(string.Format(Properties.Resources.ErrorRegKeyNotDefined,
                        @"HKEY_LOCAL_MACHINE\" + SiriusRegKeyPath + "\\" + name)) {
                            Data = { { ErrorNotDefined, true } }
                        };
                }
                builder.DataSource = SiriusRegistryAccess.GetValueAsString(databaseKey, "Server", string.Empty);
                builder.InitialCatalog = SiriusRegistryAccess.GetValueAsString(databaseKey, "Database", string.Empty);
                if(builder.DataSource.Length == 0) {
                    throw new InvalidOperationException(string.Format(Properties.Resources.ErrorRegValueNotDefined,
                        databaseKey.Name, "server")) {
                            Data = { { ErrorNotDefined, true } }
                        };
                }
                if(builder.InitialCatalog.Length == 0) {
                    throw new InvalidOperationException(string.Format(Properties.Resources.ErrorRegValueNotDefined,
                        databaseKey.Name, "database")) {
                            Data = { { ErrorNotDefined, true } }
                        };
                }

                // NB: All code below this point uses SwiftRegistryAccess, because these values were all added by me
                // to support named data sources, and they use Swift storage formats.

                // Read the login credentials.
                builder.IntegratedSecurity = SwiftRegistryAccess.GetValueAsBoolean(databaseKey, "Trusted", false);
                if(!builder.IntegratedSecurity) {
                    builder.UserID = SwiftRegistryAccess.GetValueAsString(databaseKey, "UserName", string.Empty);
                    builder.Password = SwiftRegistryAccess.GetValueAsString(databaseKey, "Password", string.Empty);
                    // Recognise well-known logins automatically to avoid having to include secret passwords in the registry.
                    if(string.Equals(builder.UserID, SiriusLoginName, StringComparison.InvariantCultureIgnoreCase) && builder.Password.Length == 0) {
                        builder.Password = SiriusLoginPassword;
                    } else if(string.Equals(builder.UserID, SwiftLoginName, StringComparison.InvariantCultureIgnoreCase) && builder.Password.Length == 0) {
                        builder.Password = SwiftLoginPassword;
                    } else if(builder.UserID.Length == 0) {
                        // Default to the Sirius login if none specified. This is only for backward compatibility with
                        // the existing "SiriusSolutions" data source and should not be necessary for any others.
                        builder.UserID = SiriusLoginName;
                        builder.Password = SiriusLoginPassword;
                    }
                }

                // Read the optional bits.
                if(!string.IsNullOrEmpty(applicationName)) {
                    builder.ApplicationName = applicationName;
                }
                if(!string.IsNullOrEmpty(workstationName)) {
                    builder.WorkstationID = workstationName;
                }
                if(packetSize != 0) {
                    builder.PacketSize = packetSize;
                }

                // Read the timeout values.
                int connectTimeout = SwiftRegistryAccess.GetValueAsInt32(databaseKey, "ConnectTimeout", 0);
                if(connectTimeout != 0) {
                    builder.ConnectTimeout = connectTimeout;
                }
                commandTimeout = SwiftRegistryAccess.GetValueAsInt32(databaseKey, "CommandTimeout", CommandTimeoutDefault);

            }

            return new SiriusConnectionSqlClient(builder.ConnectionString, schema, commandTimeout, transactionBehaviour);

        }

        #endregion

        #region Constructors - Swift Config

        /// <summary>
        /// Create a new database connection from the Swift database registry key.
        /// </summary>
        public static SiriusConnectionSqlClient FromSwiftRegKey(SiriusConnectionSchema schema,
            string applicationName,
            string workstationName,
            int packetSize,
            TransactionBehaviour transactionBehaviour) {

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            int commandTimeout = 0;

            using(RegistryKey databaseKey = Registry.LocalMachine.OpenSubKey32(SwiftRegKeyPath)) {

                // Read and check the mandatory values.
                if(databaseKey == null) {
                    throw new InvalidOperationException(string.Format(Properties.Resources.ErrorRegKeyNotDefined,
                        @"HKEY_LOCAL_MACHINE\" + SwiftRegKeyPath)) {
                            Data = { { ErrorNotDefined, true } }
                        };
                }
                string schemaName;
                switch(schema) {
                case SiriusConnectionSchema.Letters:
                case SiriusConnectionSchema.Sirius:
                    schemaName = schema.ToString();
                    break;
                case SiriusConnectionSchema.Swift:
                    schemaName = string.Empty;
                    break;
                default:
                    throw new ArgumentException(Properties.Resources.ErrorInvalidSchemaValue, "schema");
                }
                builder.DataSource = SwiftRegistryAccess.GetValueAsString(databaseKey, schemaName + "Server", string.Empty);
                builder.InitialCatalog = SwiftRegistryAccess.GetValueAsString(databaseKey, schemaName + "Database", string.Empty);
                if(builder.DataSource.Length == 0) {
                    throw new InvalidOperationException(string.Format(Properties.Resources.ErrorRegValueNotDefined,
                        databaseKey.Name, "server")) {
                            Data = { { ErrorNotDefined, true } }
                        };
                }
                if(builder.InitialCatalog.Length == 0) {
                    throw new InvalidOperationException(string.Format(Properties.Resources.ErrorRegValueNotDefined,
                        databaseKey.Name, "database")) {
                            Data = { { ErrorNotDefined, true } }
                        };
                }

                // Read the login credentials.
                builder.IntegratedSecurity = SwiftRegistryAccess.GetValueAsBoolean(databaseKey, schemaName + "Trusted", false);
                if(!builder.IntegratedSecurity) {
                    switch(schema) {
                    case SiriusConnectionSchema.Letters:
                        builder.UserID = LettersLoginName;
                        builder.Password = LettersLoginPassword;
                        break;
                    case SiriusConnectionSchema.Sirius:
                        builder.UserID = SiriusLoginName;
                        builder.Password = SiriusLoginPassword;
                        break;
                    case SiriusConnectionSchema.Swift:
                        builder.UserID = SwiftLoginName;
                        builder.Password = SwiftLoginPassword;
                        break;
                    }
                }

                // Read the optional bits.
                if(!string.IsNullOrEmpty(applicationName)) {
                    builder.ApplicationName = applicationName;
                }
                if(!string.IsNullOrEmpty(workstationName)) {
                    builder.WorkstationID = workstationName;
                }
                if(packetSize != 0) {
                    builder.PacketSize = packetSize;
                }

                // Read the timeout values.
                int connectTimeout = SwiftRegistryAccess.GetValueAsInt32(databaseKey, "ConnectTimeout", 0);
                if(connectTimeout != 0) {
                    builder.ConnectTimeout = connectTimeout;
                }
                commandTimeout = SwiftRegistryAccess.GetValueAsInt32(databaseKey, "CommandTimeout", CommandTimeoutDefault);

            }

            return new SiriusConnectionSqlClient(builder.ConnectionString, schema, commandTimeout, transactionBehaviour);

        }

        #endregion

        #region Constructors - 21

        /// <summary>
        /// Create a new 21 database connection from 21 config data.
        /// </summary>
        public static SiriusConnectionSqlClient From21Info(SiriusConnectionSchema schema,
            string serverName,
            string databaseName,
            bool trusted,
            string applicationName,
            string workstationName,
            int packetSize,
            int connectTimeout,
            int commandTimeout,
            TransactionBehaviour transactionBehaviour) {

            if(string.IsNullOrEmpty(serverName)) {
                throw new ArgumentException(string.Format(Properties.Resources.ErrorRegValueNotDefined,
                    string.Empty, "server")) {
                        Data = { { ErrorNotDefined, true } }
                    };
            }
            if(string.IsNullOrEmpty(databaseName)) {
                throw new ArgumentException(string.Format(Properties.Resources.ErrorRegValueNotDefined,
                    string.Empty, "database")) {
                        Data = { { ErrorNotDefined, true } }
                    };
            }

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            // Set the location.
            builder.DataSource = serverName;
            builder.InitialCatalog = databaseName;

            // Set the login credentials.
            builder.IntegratedSecurity = trusted;
            if(!builder.IntegratedSecurity) {
                switch(schema) {
                case SiriusConnectionSchema.Letters:
                    builder.UserID = LettersLoginName;
                    builder.Password = LettersLoginPassword;
                    break;
                case SiriusConnectionSchema.Sirius:
                    builder.UserID = SiriusLoginName;
                    builder.Password = SiriusLoginPassword;
                    break;
                case SiriusConnectionSchema.Swift:
                    builder.UserID = SwiftLoginName;
                    builder.Password = SwiftLoginPassword;
                    break;
                }
            }

            // Read the optional bits.
            if(!string.IsNullOrEmpty(applicationName)) {
                builder.ApplicationName = applicationName;
            }
            if(!string.IsNullOrEmpty(workstationName)) {
                builder.WorkstationID = workstationName;
            }
            if(packetSize != 0) {
                builder.PacketSize = packetSize;
            }

            // Read the timeout values.
            if(connectTimeout != 0) {
                builder.ConnectTimeout = connectTimeout;
            }

            return new SiriusConnectionSqlClient(builder.ConnectionString, schema, commandTimeout, transactionBehaviour);

        }

        #endregion

        #region Constructors - Internal

        /// <summary>
        /// Create a new database connection from an arbitary connection string.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <param name="schema">The database schema.</param>
        /// <param name="commandTimeout">Default command timeout value.</param>
        /// <param name="transactionBehaviour">Transaction behaviour.</param>
        /// <remarks>
        /// It is NOT recommended to call this constructor directly; use <see cref="SiriusConnection.FromAny"/>,
        /// <see cref="SiriusConnection.FromNamedSource"/>, <see cref="SiriusConnection.FromSirius"/>,
        /// <see cref="SiriusConnection.FromSiriusViaSwift"/> or <see cref="SiriusConnection.FromSwift"/> instead.
        /// </remarks>
        public SiriusConnectionSqlClient(string connectionString,
            SiriusConnectionSchema schema,
            int commandTimeout,
            TransactionBehaviour transactionBehaviour) {

            _connectionString = connectionString;
            _connection = new SqlConnection(connectionString);
            _transaction = null;
            _transactionBehaviour = transactionBehaviour;
            this.CommandTimeout = commandTimeout;
            this.Schema = schema;

            // Re-parse the connection string to form our own, shorter, string that uniquely identifies
            // one physical database.
            SqlConnectionStringBuilder info = new SqlConnectionStringBuilder(connectionString);
            if(info.ConnectionString != "") {
                this.CacheKey = "SQLCONTEXT";
            } else {
                this.CacheKey = string.Format(CultureInfo.InvariantCulture, "SQL|{0}|{1}|{2}",
                    string.IsNullOrEmpty(info.AttachDBFilename) ? info.DataSource : info.AttachDBFilename,
                    info.IntegratedSecurity ? string.Empty : info.UserID,
                    info.InitialCatalog);
            }

        }

        /// <summary>
        /// Initialise a new instance of the class with serialized data.
        /// </summary>
        /// <param name="info">Serialized object data</param>
        /// <param name="context">Contextual information about the source or destination</param>
        protected SiriusConnectionSqlClient(SerializationInfo info, StreamingContext context)
            : base(info, context) {

            if(info == null) {
                throw new ArgumentNullException("info");
            }
            _connectionString = info.GetString("_connectionString");
            _connection = new SqlConnection(_connectionString);
            _transaction = null;
            _transactionBehaviour = (TransactionBehaviour) info.GetValue("_transactionBehaviour", typeof(TransactionBehaviour));
            _transactionCount = 0;
            _stateful = info.GetBoolean("_stateful");
        }

        #endregion

        #region ISerializable Members

        /// <summary>
        /// Serialise this instance of the class to serialised data.
        /// </summary>
        /// <param name="info">Serialized object data</param>
        /// <param name="context">Contextual information about the source or destination</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);

            if(info == null) {
                throw new ArgumentNullException("info");
            }
            info.AddValue("_connectionString", _connectionString);
            // _connection not serialized
            // _transaction not serialized
            info.AddValue("_transactionBehaviour", _transactionBehaviour, typeof(TransactionBehaviour));
            // _transactionCount not serialized
            info.AddValue("_stateful", _stateful);
        }

        #endregion

        #region Finalizers

        /// <summary>
        /// Release all unmanaged resources used by this object and optionally release managed resources as well.
        /// </summary>
        /// <param name="disposing">Release managed resources?</param>
        /// <remarks>
        /// If any transactions are still pending, they will all be rolled back before the connection is closed.
        /// </remarks>
        protected override void Dispose(bool disposing) {

            if(!Disposed) {
                if(disposing) {
                    // Dispose managed resources.
                    if(_transaction != null) {
                        _transaction.Dispose();
                    }
                    if(_connection != null) {
                        _connection.Dispose();
                    }
                }
                // Dispose unmanaged resources.
            }
            Disposed = true;

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The exact behaviour of the begin / commit / rollback transaction methods.
        /// </summary>
        public override TransactionBehaviour TransactionBehaviour {
            get {
                DisposedCheck();
                return _transactionBehaviour;
            }
        }

        /// <summary>
        /// The number of logical nested transactions currently open.
        /// </summary>
        public override int TransactionCount {
            get {
                DisposedCheck();
                return _transactionCount;
            }
        }

        /// <summary>
        /// The wrapped <see cref="SqlConnection"/> object.
        /// </summary>
        public override SqlConnection SqlConnection {
            get {
                DisposedCheck();
                return _connection;
            }
        }

        /// <summary>
        /// The wrapped <see cref="SqlTransaction"/> object.
        /// </summary>
        public override SqlTransaction SqlTransaction {
            get {
                DisposedCheck();
                return _transaction;
            }
        }

        /// <summary>
        /// This property is not applicable to this particular derived class.
        /// </summary>
        /// <exception cref="NotSupportedException">The property is not supported for this type of connection.</exception>
        public override object PMDAODatabase {
            get {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region Public Methods - Clone

        /// <summary>
        /// Creates a new independent connection with the same connection information as this connection.
        /// The transaction state is not copied; the new connection will have <see cref="TransactionCount"/> = 0.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public override SiriusConnection Clone() {

            DisposedCheck();

            // This is not normal cloning code because a SqlConnection cannot be cloned. Instead,
            // we create a new SqlConnection using the same connection string. This has the added advantage
            // of working even when a transaction is open; the clone merely starts with no transaction
            // which is what you would expect to happen.
            return new SiriusConnectionSqlClient(_connectionString,
                this.Schema,
                this.CommandTimeout,
                _transactionBehaviour);

        }

        #endregion

        #region Public Methods - Transactions

        /// <summary>
        /// Begin a nested transaction on the connection.
        /// </summary>
        public override void BeginTransaction() {

            DisposedCheck();

            // Roll our own nesting logic because:
            // (a) we want it to be compatible with either PMDAO or SQL Server's native implementation, and
            // (b) the SqlClient classes do not even support nesting.

            if(_transactionCount == 0) {
                BeginTransactionImpl();
            }
            _transactionCount += 1;

        }

        /// <summary>
        /// Commit a nested transaction on the connection.
        /// </summary>
        /// <exception cref="InvalidOperationException">There are no open transactions to commit.</exception>
        /// <remarks>
        /// If <see cref="TransactionCount"/> is greater than 1, this method will have the same effect as
        /// <see cref="RollbackTransaction"/>. It is the calling code's responsibility to throw an exception after
        /// every rollback, in order to ensure that the outermost transaction ends up being rolled back as well.
        /// </remarks>
        public override void CommitTransaction() {

            DisposedCheck();

            // Roll our own nesting logic because:
            // (a) we want it to be compatible with either PMDAO or SQL Server's native implementation, and
            // (b) the SqlClient classes do not even support nesting.

            // Safety check.
            if(_connection.State == ConnectionState.Closed) {
                throw new InvalidOperationException(Properties.Resources.ErrorConnectionClosedCommit);
            }

            // Safety check.
            if(_transactionBehaviour == TransactionBehaviour.PMDAO) {
                if(_transactionCount <= 0) {
                    _transactionCount = 0;
                    return;
                }
            } else {
                if(_transactionCount <= 0) {
                    throw new InvalidOperationException(Properties.Resources.ErrorTranCountZeroCommit);
                }
            }

            _transactionCount -= 1;
            if(_transactionCount == 0) {
                CommitTransactionImpl();
            }

        }

        /// <summary>
        /// Roll back a nested transaction on the connection.
        /// </summary>
        /// <exception cref="InvalidOperationException">There are no open transactions to roll back.</exception>
        /// <remarks>
        /// If <see cref="TransactionCount"/> is greater than 1, this method will have the same effect as
        /// <see cref="CommitTransaction"/>. It is the calling code's responsibility to throw an exception after
        /// every rollback, in order to ensure that the outermost transaction ends up being rolled back as well.
        /// </remarks>
        public override void RollbackTransaction() {

            DisposedCheck();

            // Roll our own nesting logic because:
            // (a) we want it to be compatible with either PMDAO or SQL Server's native implementation, and
            // (b) the SqlClient classes do not even support nesting.

            // NOTE: No exceptions are thrown in here because rollbacks are normally done inside a
            // catch block, and all we really want to do is restore the connection to a good state
            // and not care how it's done.

            // Safety check.
            if(_connection.State == ConnectionState.Closed) {
                return;
            }

            // Safety check.
            if(_transactionCount <= 0) {
                _transactionCount = 0;
                return;
            }

            if(_transactionBehaviour == TransactionBehaviour.PMDAO) {
                _transactionCount -= 1;
                if(_transactionCount == 0) {
                    RollbackTransactionImpl();
                }
            } else {
                _transactionCount = 0;
                RollbackTransactionImpl();
            }

        }

        #endregion

        #region Public Methods - Execute Command

        /// <summary>
        /// Execute the command without expecting a resultset or stream to be returned.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>The number of rows affected.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public override int ExecuteNonQuery(SiriusCommand command) {

            DisposedCheck();

            try {
                // Correct parameter values.
                ValidateParameterArray(command);
            } catch(Exception ex) {
                if(HandleExceptionFromParameters(ex, command)) {
                    throw;
                }
            }

            int rowsAffected = 0;
            try {
                // Connect the connection to the command.
                ExecuteBegin(command);

                // Execute the command.
                rowsAffected = command.SqlCommand.ExecuteNonQuery();
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
            } finally {
                // Disconnect the connection from the command.
                ExecuteEnd(command);
            }
            return rowsAffected;
        }

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
        public override object ExecuteScalar(SiriusCommand command) {

            DisposedCheck();

            try {
                // Correct parameter values.
                ValidateParameterArray(command);
            } catch(Exception ex) {
                if(HandleExceptionFromParameters(ex, command)) {
                    throw;
                }
            }

            object value = null;
            try {
                // Connect the connection to the command.
                ExecuteBegin(command);

                // Execute the command.
                value = command.SqlCommand.ExecuteScalar();
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
            } finally {
                // Disconnect the connection from the command.
                ExecuteEnd(command);
            }
            return value;
        }

        /// <summary>
        /// Execute the command and return a resultset in a new <see cref="DataTable"/>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>A new <see cref="DataTable"/> containing the resultset data.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public override DataTable ExecuteDataTable(SiriusCommand command) {

            DataTable results = new DataTable();
            results.Locale = CultureInfo.InvariantCulture;
            ExecuteDataTable(command, results);
            return results;
        }

        /// <summary>
        /// Execute the command and merge a resultset into an existing <see cref="DataTable"/>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="results">The <see cref="DataTable"/> to fill with data.</param>
        /// <returns>The number of rows successfully added or refreshed.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public override int ExecuteDataTable(SiriusCommand command, DataTable results) {

            using(SqlDataAdapter adapter = new SqlDataAdapter()) {
                return ExecuteDataTable(command, adapter, results);
            }
        }

        /// <summary>
        /// Execute the command and merge a resultset into an existing <see cref="DataTable"/>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="adapter">The <see cref="SqlDataAdapter"/> to use for merging the data.</param>
        /// <param name="results">The <see cref="DataTable"/> to fill with data.</param>
        /// <returns>The number of rows successfully added or refreshed.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public override int ExecuteDataTable(SiriusCommand command, DbDataAdapter adapter, DataTable results) {

            DisposedCheck();

            SqlDataAdapter adapterImpl = (SqlDataAdapter) adapter;

            try {
                // Correct parameter values.
                ValidateParameterArray(command);
            } catch(Exception ex) {
                if(HandleExceptionFromParameters(ex, command)) {
                    throw;
                }
            }

            int rowsAffected = 0;
            try {
                // Connect the connection to the command.
                ExecuteBegin(command);

                // Execute the command.
                adapter.SelectCommand = command.SqlCommand;
                rowsAffected = adapterImpl.Fill(results);
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
            } finally {
                // Disconnect the connection from the command.
                ExecuteEnd(command);
            }
            return rowsAffected;
        }

        /// <summary>
        /// Execute the command and return all resultsets in a new <see cref="DataSet"/>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="tableName">The name to give the <see cref="DataTable"/> containing the first resultset. Any additional resultsets are named sequentially based on this name.</param>
        /// <returns>A new <see cref="DataSet"/> containing the resultset data.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public override DataSet ExecuteDataSet(SiriusCommand command, string tableName) {

            DataSet results = new DataSet();
            results.Locale = CultureInfo.InvariantCulture;
            ExecuteDataSet(command, results, tableName);
            return results;
        }

        /// <summary>
        /// Execute the command and merge all resultsets into an existing <see cref="DataSet"/>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="results">The <see cref="DataSet"/> to fill with data.</param>
        /// <param name="tableName">The name to give the <see cref="DataTable"/> containing the first resultset. Any additional resultsets are named sequentially based on this name.</param>
        /// <returns>The number of rows successfully added or refreshed.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public override int ExecuteDataSet(SiriusCommand command, DataSet results, string tableName) {

            using(SqlDataAdapter adapter = new SqlDataAdapter()) {
                return ExecuteDataSet(command, adapter, results, tableName);
            }
        }

        /// <summary>
        /// Execute the command and merge all resultsets into an existing <see cref="DataSet"/>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="adapter">The <see cref="SqlDataAdapter"/> to use for merging the data.</param>
        /// <param name="results">The <see cref="DataSet"/> to fill with data.</param>
        /// <param name="tableName">The name to give the <see cref="DataTable"/> containing the first resultset. Any additional resultsets are named sequentially based on this name.</param>
        /// <returns>The number of rows successfully added or refreshed.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public override int ExecuteDataSet(SiriusCommand command, DbDataAdapter adapter, DataSet results, string tableName) {

            DisposedCheck();

            SqlDataAdapter adapterImpl = (SqlDataAdapter) adapter;

            try {
                // Correct parameter values.
                ValidateParameterArray(command);
            } catch(Exception ex) {
                if(HandleExceptionFromParameters(ex, command)) {
                    throw;
                }
            }

            int rowsAffected = 0;
            try {
                // Connect the connection to the command.
                ExecuteBegin(command);

                // Execute the command.
                adapter.SelectCommand = command.SqlCommand;
                rowsAffected = adapterImpl.Fill(results, tableName);
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
            } finally {
                // Disconnect the connection from the command.
                ExecuteEnd(command);
            }
            return rowsAffected;
        }

        /// <summary>
        /// Execute the command and return a resultset as a list of business objects.
        /// </summary>
        /// <typeparam name="T">The business class to populate. This must implement the <see cref="IDataReadable"/> interface.</typeparam>
        /// <param name="command">The command to execute.</param>
        /// <returns>A new <see cref="List&lt;T&gt;"/> containing the resultset data.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public override List<T> ExecuteList<T>(SiriusCommand command) {

            DisposedCheck();

            try {
                // Correct parameter values.
                ValidateParameterArray(command);
            } catch(Exception ex) {
                if(HandleExceptionFromParameters(ex, command)) {
                    throw;
                }
            }

            List<T> results = new List<T>();
            try {
                // Connect the connection to the command.
                ExecuteBegin(command);

                // Execute the command.
                using(SqlDataReader reader = command.SqlCommand.ExecuteReader(CommandBehavior.SingleResult)) {
                    while(reader.Read()) {
                        T row = new T();
                        ((IDataReadable) row).SetObjectData(reader);
                        results.Add(row);
                    }
                }
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
            } finally {
                // Disconnect the connection from the command.
                ExecuteEnd(command);
            }
            return results;
        }

        /// <summary>
        /// Execute the command and return a resultset as a list of business objects.
        /// </summary>
        /// <typeparam name="T">The business class to populate.</typeparam>
        /// <param name="command">The command to execute.</param>
        /// <param name="convert">A method to convert each data row into an instance of the business class.</param>
        /// <returns>A new <see cref="List&lt;T&gt;"/> containing the resultset data.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public override List<T> ExecuteList<T>(SiriusCommand command, Converter<IDataRecord, T> convert) {

            DisposedCheck();

            try {
                // Correct parameter values.
                ValidateParameterArray(command);
            } catch(Exception ex) {
                if(HandleExceptionFromParameters(ex, command)) {
                    throw;
                }
            }

            List<T> results = new List<T>();
            try {
                // Connect the connection to the command.
                ExecuteBegin(command);

                // Execute the command.
                using(SqlDataReader reader = command.SqlCommand.ExecuteReader(CommandBehavior.SingleResult)) {
                    while(reader.Read()) {
                        results.Add(convert(reader));
                    }
                }
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
            } finally {
                // Disconnect the connection from the command.
                ExecuteEnd(command);
            }
            return results;
        }

        /// <summary>
        /// Execute the command and return XML data as a string.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>A <see cref="String"/> containing the XML output of the command.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public override string ExecuteXmlText(SiriusCommand command) {

            DisposedCheck();

            try {
                // Correct parameter values.
                ValidateParameterArray(command);
            } catch(Exception ex) {
                if(HandleExceptionFromParameters(ex, command)) {
                    throw;
                }
            }

            string results = null;
            try {
                // Connect the connection to the command.
                ExecuteBegin(command);

                // Execute the command.
                using(XmlReader reader = command.SqlCommand.ExecuteXmlReader()) {
                    if(reader.Read()) {
                        results = reader.ReadOuterXml();
                    }
                }
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
            } finally {
                // Disconnect the connection from the command.
                ExecuteEnd(command);
            }
            return results;
        }

        /// <summary>
        /// Execute the command and return XML data as a navigable document.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>An <see cref="XPathDocument"/> containing the XML output of the command.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public override XPathDocument ExecuteXPathDocument(SiriusCommand command) {

            DisposedCheck();

            try {
                // Correct parameter values.
                ValidateParameterArray(command);
            } catch(Exception ex) {
                if(HandleExceptionFromParameters(ex, command)) {
                    throw;
                }
            }

            XPathDocument results = null;
            try {
                // Connect the connection to the command.
                ExecuteBegin(command);

                // Execute the command.
                using(XmlReader reader = command.SqlCommand.ExecuteXmlReader()) {
                    results = new XPathDocument(reader);
                }
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
            } finally {
                // Disconnect the connection from the command.
                ExecuteEnd(command);
            }
            return results;
        }

        #endregion

        #region Public Methods - Connection State

        /// <summary>
        /// Open the connection (only if it is currently closed) and hold it open until <see cref="EndStateful"/> is called.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void BeginStateful() {

            // Open the connection if currently closed.
            OpenConnectionImpl();
            _stateful = true;
        }

        /// <summary>
        /// Close the connection (only if it is currently open and no transactions are active).
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void EndStateful() {

            // Close the connection if currently open and there are no active transactions.
            if(this.TransactionCount == 0) {
                CloseConnectionImpl();
                _stateful = false;
            }
        }

        #endregion

        #region Protected Methods - Connection State

        /// <summary>
        /// Open the connection (only if it is currently closed).
        /// </summary>
        protected override void OpenConnection() {

            // Open the connection if currently closed.
            OpenConnectionImpl();

        }

        /// <summary>
        /// Close the connection (only if it is currently open and no transactions are active).
        /// </summary>
        protected override void CloseConnection() {

            // Close the connection if currently open and there are no active transactions and it is not wedged open.
            if(this.TransactionCount == 0 && !_stateful) {
                CloseConnectionImpl();
            }

        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Expose this for unit-testing cloning and serialization. It is accessed from
        /// the unit test assembly using reflection.
        /// </summary>
        internal string ConnectionInfo {
            get {
                return _connectionString;
            }
        }

        #endregion

        #region Private Methods

        private void OpenConnectionImpl() {

            // There is a known multithreading bug in ADO.NET; see the article by Dan Archer at
            // <a href="http://www.simple-talk.com/community/blogs/dana/archive/2007/03/15/20819.aspx"/>.
            // I have decided not to implement the suggested workaround because it would incur an unacceptable
            // concurrency and performance hit to use a monitor lock every time we open a connection. In addition,
            // it is unlikely that a production server would ever be shut down while code is trying to run.
            // If the error occurs in practice, we just need to be aware that the error message is not accurate.
            if(_connection.State == ConnectionState.Closed) {
                _connection.Open();
            }

        }

        private void CloseConnectionImpl() {

            if(_connection.State != ConnectionState.Closed) {
                _connection.Close();
            }

        }

        private void BeginTransactionImpl() {

            OpenConnectionImpl();

            _transaction = _connection.BeginTransaction();

        }

        private void CommitTransactionImpl() {

            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;

            if(!_stateful) {
                CloseConnectionImpl();
            }

        }

        private void RollbackTransactionImpl() {

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;

            if(!_stateful) {
                CloseConnectionImpl();
            }

        }

        private void ExecuteBegin(SiriusCommand command) {

            // Open the connection if not already open.
            OpenConnection();

            // Attach the connection and transaction to the command.
            command.SqlCommand.Connection = _connection;
            command.SqlCommand.Transaction = _transaction;
            if(command.CommandTimeoutInherited) {
                command.SqlCommand.CommandTimeout = this.CommandTimeout;
            }

            // Prepare the command if requested, then set it to "already prepared".
            if((command.Behaviour & SiriusCommandBehaviour.Prepare) != 0) {
                command.SqlCommand.Prepare();
                command.Behaviour &= ~SiriusCommandBehaviour.Prepare;
            }

        }

        private void ExecuteEnd(SiriusCommand command) {

            // Detach the connection and transaction from the command.
            command.SqlCommand.Transaction = null;
            command.SqlCommand.Connection = null;

            // Close the connection if possible.
            CloseConnection();

        }

        private void ValidateParameterArray(SiriusCommand command) {

            foreach(SqlParameter parameter in command.SqlCommand.Parameters) {
                ValidateParameter(parameter, command.Behaviour);
            }

        }

        /// <summary>
        /// Hard-coded exception handling rules for the statement that executes the command.
        /// </summary>
        /// <param name="ex">The exception that has been caught.</param>
        /// <param name="command">The command being executed.</param>
        /// <returns>True if the exception should be re-thrown, otherwise false.</returns>
        private bool HandleExceptionFromExecution(Exception ex, SiriusCommand command) {

            if(ex is ArgumentException || ex is FormatException || ex is InvalidCastException || ex is OverflowException || ex is SqlTypeException) {
                if(ex.Source == "System.Data" || ex.Source == "mscorlib") {
                    // Wrap the case where a parameter value passed to SqlClient is the wrong type or size or an illegal value.
                    throw new DatabaseException(BadParameterMessage, ex, null, this, command);
                } else {
                    return true;
                }
            } else if(ex is SqlException) {
                // Wrap all exceptions from the server.
                throw new DatabaseException(ex.Message, ex, TranslateErrors(((SqlException) ex).Errors), this, command);
            } else {
                return true;
            }

        }

        #endregion

        #region Private Shared Methods

        private static void ValidateParameter(SqlParameter parameter, SiriusCommandBehaviour behaviour) {

            // Validate the data value manually before it gets passed to ADO.NET. If illegal values
            // get that far, then ADO.NET will throw inconsistent and unhelpful exceptions.
            ValidateParameterValue(parameter, behaviour);

            // Convert value. Different types are handled as follows:
            // * Null - Interpreted as "missing parameter". Convert to DBNull.Value.
            // * DBNull.Value - Pass through unchanged.
            // * Missing.Value - Convert to null to indicate "missing parameter".
            // * Intrinsic type - Pass through unchanged.
            // * Nullable Of(Intrinsic type) - Pass through unchanged.
            // * SQL type - Pass through unchanged.
            if(parameter.Value == null) {
                parameter.Value = DBNull.Value;
            } else if(parameter.Value == Missing.Value) {
                parameter.Value = null;
            }

        }

        private static DatabaseErrorCollection TranslateErrors(SqlErrorCollection errors) {

            List<DatabaseError> errorList = new List<DatabaseError>();
            foreach(SqlError item in errors) {
                errorList.Add(new DatabaseError(item.Source, item.Message, item.Number, item.Class, item.State, string.Empty));
            }
            return new DatabaseErrorCollection(errorList);

        }

        #endregion

    }

}
