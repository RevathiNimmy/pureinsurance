using Sirius.Architecture.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;
using System.Xml.XPath;

namespace Sirius.Architecture.Data.BackOffice {

    /// <summary>
    /// This class supports <see cref="SiriusConnection"/>, and is not intended to be used directly in your code.
    /// </summary>
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class SiriusConnectionPMDAO : SiriusConnection {

        #region Private Fields

        private PmdaoConnectionInfo _connectionInfo;
        private dPMDAO.Database _database;

        #endregion

        #region Constructors - Internal

        /// <summary>
        /// Create a new database connection from Sirius server-side global state information.
        /// DO NOT call this constructor directly; use <see cref="SiriusConnection.FromSiriusViaPMDAO"/> instead.
        /// </summary>
        /// <param name="siriusUserName">Data normally used to initialise dPMDAO.</param>
        /// <param name="sourceID">Data normally used to initialise dPMDAO.</param>
        /// <param name="languageID">Data normally used to initialise dPMDAO.</param>
        /// <param name="callingAppName">Data normally used to initialise dPMDAO.</param>
        /// <param name="userName">Data normally used to initialise dPMDAO.</param>
        /// <param name="password">Data normally used to initialise dPMDAO.</param>
        /// <param name="dataSourceName">Data normally used to initialise dPMDAO.</param>
        /// <param name="databaseName">Data normally used to initialise dPMDAO.</param>
        public SiriusConnectionPMDAO(string siriusUserName,
            short sourceID,
            short languageID,
            string callingAppName,
            object userName,
            object password,
            object dataSourceName,
            object databaseName)
            : this(new PmdaoConnectionInfo {
                SiriusUserName = siriusUserName,
                SourceID = sourceID,
                LanguageID = languageID,
                CallingAppName = callingAppName,
                UserName = userName,
                Password = password,
                DataSourceName = dataSourceName,
                DatabaseName = databaseName
            }) {
        }

        /// <summary>
        /// Create a new database connection using another object's connection information.
        /// The connection information object is copied, not shared.
        /// </summary>
        /// <param name="info">Connection information</param>
        [OleDbPermission(SecurityAction.Demand)]
        private SiriusConnectionPMDAO(PmdaoConnectionInfo info) {

            // Always clone this first because we mustn't share a reference with any other PMDAO instance.
            _connectionInfo = info.Clone();

            InitialiseDatabase();

            this.CommandTimeout = _database.get_QueryTimeout();
            this.Schema = SiriusConnectionSchema.Sirius;

            base.CacheKey = string.Format(CultureInfo.InvariantCulture, "PMDAO|{0}|{1}",
                info.DataSourceName,
                info.DatabaseName);
        }

        /// <summary>
        /// Initialise the PMDAO instance from the stored connection information.
        /// </summary>
        [OleDbPermission(SecurityAction.Demand)]
        private void InitialiseDatabase() {

            _database = new dPMDAO.Database();

            var returnValue = (PMReturnCode) _database.OpenDatabase(
                ref _connectionInfo.SiriusUserName,
                ref _connectionInfo.SourceID,
                ref _connectionInfo.LanguageID,
                ref _connectionInfo.CallingAppName,
                ref _connectionInfo.UserName,
                ref _connectionInfo.Password,
                ref _connectionInfo.DataSourceName,
                ref _connectionInfo.DatabaseName);

            if(returnValue != PMReturnCode.PMTrue) {
                throw new PMErrorException(_database, "OpenDatabase", (int) returnValue);
            }
        }

        /// <summary>
        /// Initialise a new instance of the class with serialized data.
        /// </summary>
        /// <param name="info">Serialized object data</param>
        /// <param name="context">Contextual information about the source or destination</param>
        protected SiriusConnectionPMDAO(SerializationInfo info, StreamingContext context)
            : base(info, context) {

            if(info == null) {
                throw new ArgumentNullException("info");
            }
            _connectionInfo = (PmdaoConnectionInfo) info.GetValue("_connectionInfo", typeof(PmdaoConnectionInfo));
            InitialiseDatabase();
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
            info.AddValue("_connectionInfo", _connectionInfo);
            // _database not serialized
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
        [OleDbPermission(SecurityAction.Demand)]
        protected override void Dispose(bool disposing) {
            if(!Disposed) {
                if(disposing) {
                    // Dispose managed resources.
                    try {
                        if(_database != null) {
                            _database.CloseDatabase();
                        }
                        RcwHelper.Release(_database);
                    } catch {
                        // Absorb all exceptions.
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
                return TransactionBehaviour.PMDAO;    
            }
        }

        /// <summary>
        /// The number of logical nested transactions currently open.
        /// </summary>
        public override int TransactionCount {
            [OleDbPermission(SecurityAction.Demand)]
            get {
                DisposedCheck();
                return _database.TransactionNestLevel;
            }
        }

        /// <summary>
        /// This property is not applicable to this particular derived class.
        /// </summary>
        /// <exception cref="NotSupportedException">The property is not supported for this type of connection.</exception>
        public override SqlConnection SqlConnection {
            get {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// This property is not applicable to this particular derived class.
        /// </summary>
        /// <exception cref="NotSupportedException">The property is not supported for this type of connection.</exception>
        public override SqlTransaction SqlTransaction {
            get {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// The wrapped <c>dPMDAO.Database</c> object.
        /// </summary>
        public override object PMDAODatabase {
            [OleDbPermission(SecurityAction.Demand)]
            get {
                DisposedCheck();
                return _database;
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
            // This is not normal cloning code because a PMDAO Database cannot be cloned. Instead,
            // we create a new PMDAO Database using the same connection info. This has the added advantage
            // of working even when a transaction is open; the clone merely starts with no transaction
            // which is what you would expect to happen.
            return new SiriusConnectionPMDAO(_connectionInfo);
        }

        #endregion

        #region Public Methods - Transactions

        /// <summary>
        /// Begin a nested transaction on the connection.
        /// </summary>
        public override void BeginTransaction() {
            DisposedCheck();
            BeginTransactionImpl();
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
            if(this.TransactionCount <= 0) {
                throw new InvalidOperationException(Properties.Resources.ErrorTranCountZeroCommit);
            }
            CommitTransactionImpl();
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
            if(this.TransactionCount <= 0) {
                throw new InvalidOperationException(Properties.Resources.ErrorTranCountZeroRollback);
            }
            RollbackTransactionImpl();
        }

        #endregion

        #region Public Methods - Execute Command

        /// <summary>
        /// Execute the command without expecting a resultset or stream to be returned.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>The number of rows affected.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        [OleDbPermission(SecurityAction.Demand)]
        public override int ExecuteNonQuery(SiriusCommand command) {

            DisposedCheck();

            int rowsAffected = 0;

            // Convert all command data into COM interop format.
            dPMDAO.CommandInfo commandInfo = GetCommandInfo(command);
            Array parameterInfoArray = GetParameterInfoArray(command);

            // Execute the command.
            try {
                rowsAffected = _database.ExecuteNonQuery(ref commandInfo, ref parameterInfoArray);
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
            }

            // Retrieve output parameter values.
            PutParameterInfoArray(command, (dPMDAO.ParameterInfo[]) parameterInfoArray);

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
        [OleDbPermission(SecurityAction.Demand)]
        public override object ExecuteScalar(SiriusCommand command) {

            DisposedCheck();

            object value = null;

            // Convert all command data into COM interop format.
            dPMDAO.CommandInfo commandInfo = GetCommandInfo(command);
            Array parameterInfoArray = GetParameterInfoArray(command);

            // Execute the command.
            try {
                value = _database.ExecuteScalar(ref commandInfo, ref parameterInfoArray);
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
            }

            // Retrieve output parameter values.
            PutParameterInfoArray(command, (dPMDAO.ParameterInfo[]) parameterInfoArray);

            return value;
        }

        /// <overloads>
        /// Execute the command and return a resultset in a new <see cref="DataTable"/>.
        /// </overloads>
        /// <summary>
        /// Execute the command and return a resultset in a new <see cref="DataTable"/>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>A new <see cref="DataTable"/> containing the resultset data.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public override DataTable ExecuteDataTable(SiriusCommand command) {
            var results = new DataTable();
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
            using(var adapter = new OleDbDataAdapter()) {
                return ExecuteDataTable(command, adapter, results);
            }
        }

        /// <summary>
        /// Execute the command and merge a resultset into an existing <see cref="DataTable"/>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="adapter">The <see cref="OleDbDataAdapter"/> to use for merging the data.</param>
        /// <param name="results">The <see cref="DataTable"/> to fill with data.</param>
        /// <returns>The number of rows successfully added or refreshed.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        [OleDbPermission(SecurityAction.Demand)]
        public override int ExecuteDataTable(SiriusCommand command, DbDataAdapter adapter, DataTable results) {

            DisposedCheck();

            var adapterImpl = (OleDbDataAdapter) adapter;

            int rowsAffected = 0;

            // Convert all command data into COM interop format.
            dPMDAO.CommandInfo commandInfo = GetCommandInfo(command);
            Array parameterInfoArray = GetParameterInfoArray(command);

            // Execute the command.
            try {
                using(var reader = new AdoDataReader(_database.ExecuteRecordset(ref commandInfo, ref parameterInfoArray))) {
                    if(!reader.IsClosed) {
                        rowsAffected = adapterImpl.Fill(results, reader.Recordset);
                    }
                }
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
            }

            // Retrieve output parameter values.
            PutParameterInfoArray(command, (dPMDAO.ParameterInfo[]) parameterInfoArray);

            return rowsAffected;
        }

        /// <overloads>
        /// Execute the command and return all resultsets in a new <see cref="DataSet"/>.
        /// </overloads>
        /// <summary>
        /// Execute the command and return all resultsets in a new <see cref="DataSet"/>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="tableName">The name to give the <see cref="DataTable"/> containing the first resultset. Any additional resultsets are named sequentially based on this name.</param>
        /// <returns>A new <see cref="DataSet"/> containing the resultset data.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        public override DataSet ExecuteDataSet(SiriusCommand command, string tableName) {
            var results = new DataSet();
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
            using(var adapter = new OleDbDataAdapter()) {
                return ExecuteDataSet(command, adapter, results, tableName);
            }
        }

        /// <summary>
        /// Execute the command and merge all resultsets into an existing <see cref="DataSet"/>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="adapter">The <see cref="OleDbDataAdapter"/> to use for merging the data.</param>
        /// <param name="results">The <see cref="DataSet"/> to fill with data.</param>
        /// <param name="tableName">The name to give the <see cref="DataTable"/> containing the first resultset. Any additional resultsets are named sequentially based on this name.</param>
        /// <returns>The number of rows successfully added or refreshed.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        [OleDbPermission(SecurityAction.Demand)]
        public override int ExecuteDataSet(SiriusCommand command, DbDataAdapter adapter, DataSet results, string tableName) {

            DisposedCheck();

            var adapterImpl = (OleDbDataAdapter) adapter;

            int rowsAffected = 0;

            // Convert all command data into COM interop format.
            dPMDAO.CommandInfo commandInfo = GetCommandInfo(command);
            Array parameterInfoArray = GetParameterInfoArray(command);

            // Execute the command.
            try {
                using(var reader = new AdoDataReader(_database.ExecuteRecordset(ref commandInfo, ref parameterInfoArray))) {
                    if(!reader.IsClosed) {
                        rowsAffected = adapterImpl.Fill(results, reader.Recordset, tableName);
                    }
                }
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
            }

            // Retrieve output parameter values.
            PutParameterInfoArray(command, (dPMDAO.ParameterInfo[]) parameterInfoArray);

            return rowsAffected;
        }

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
        [OleDbPermission(SecurityAction.Demand)]
        public override List<T> ExecuteList<T>(SiriusCommand command) {

            DisposedCheck();

            var results = new List<T>();

            // Convert all command data into COM interop format.
            dPMDAO.CommandInfo commandInfo = GetCommandInfo(command);
            Array parameterInfoArray = GetParameterInfoArray(command);

            // Execute the command.
            try {
                using(var reader = new AdoDataReader(_database.ExecuteRecordset(ref commandInfo, ref parameterInfoArray))) {
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
            }

            // Retrieve output parameter values.
            PutParameterInfoArray(command, (dPMDAO.ParameterInfo[]) parameterInfoArray);

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
        [OleDbPermission(SecurityAction.Demand)]
        public override List<T> ExecuteList<T>(SiriusCommand command, Converter<IDataRecord, T> convert) {

            DisposedCheck();

            var results = new List<T>();

            // Convert all command data into COM interop format.
            dPMDAO.CommandInfo commandInfo = GetCommandInfo(command);
            Array parameterInfoArray = GetParameterInfoArray(command);

            // Execute the command.
            try {
                using(var reader = new AdoDataReader(_database.ExecuteRecordset(ref commandInfo, ref parameterInfoArray))) {
                    while(reader.Read()) {
                        results.Add(convert(reader));
                    }
                }
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
            }

            // Retrieve output parameter values.
            PutParameterInfoArray(command, (dPMDAO.ParameterInfo[]) parameterInfoArray);

            return results;
        }

        /// <summary>
        /// Execute the command and return XML data as a string.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>A <see cref="String"/> containing the XML output of the command.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        [OleDbPermission(SecurityAction.Demand)]
        public override string ExecuteXmlText(SiriusCommand command) {

            DisposedCheck();

            string results = null;

            // Convert all command data into COM interop format.
            dPMDAO.CommandInfo commandInfo = GetCommandInfo(command);
            Array parameterInfoArray = GetParameterInfoArray(command);

            // Execute the command.
            try {
                results = _database.ExecuteStreamText(ref commandInfo, ref parameterInfoArray);
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
            }

            // Retrieve output parameter values.
            PutParameterInfoArray(command, (dPMDAO.ParameterInfo[]) parameterInfoArray);

            return results;
        }

        /// <summary>
        /// Execute the command and return XML data as a navigable document.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>An <see cref="XPathDocument"/> containing the XML output of the command.</returns>
        /// <exception cref="DatabaseException">A database server error occurred.</exception>
        [OleDbPermission(SecurityAction.Demand)]
        public override XPathDocument ExecuteXPathDocument(SiriusCommand command) {

            DisposedCheck();

            XPathDocument results = null;

            // Convert all command data into COM interop format.
            dPMDAO.CommandInfo commandInfo = GetCommandInfo(command);
            Array parameterInfoArray = GetParameterInfoArray(command);

            // Execute the command.
            try {
                var resultsText = _database.ExecuteStreamText(ref commandInfo, ref parameterInfoArray);
                using(var reader = XmlReader.Create(new StringReader(resultsText))) {
                    results = new XPathDocument(reader);
                }
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
            }

            // Retrieve output parameter values.
            PutParameterInfoArray(command, (dPMDAO.ParameterInfo[]) parameterInfoArray);

            return results;
        }

        #endregion

        #region Public Methods - Connection State

        /// <summary>
        /// Open the connection (only if it is currently closed) and hold it open until <see cref="EndStateful"/> is called.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void BeginStateful() {
            // Silently do nothing. PMDAO cannot do this yet, and it's not really worth the hassle adding support for it.
        }

        /// <summary>
        /// Close the connection (only if it is currently open and no transactions are active).
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void EndStateful() {
            // Silently do nothing. PMDAO cannot do this yet, and it's not really worth the hassle adding support for it.
        }

        #endregion

        #region Protected Methods - Connection State

        /// <summary>
        /// Open the connection (only if it is currently closed).
        /// </summary>
        protected override void OpenConnection() {
            // Do nothing because PMDAO handles it itself.
        }

        /// <summary>
        /// Close the connection (only if it is currently open and no transactions are active).
        /// </summary>
        protected override void CloseConnection() {
            // Do nothing because PMDAO handles it itself.
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Expose this for unit-testing cloning and serialization. It is accessed from
        /// the unit test assembly using reflection.
        /// </summary>
        internal PmdaoConnectionInfo ConnectionInfo {
            get {
                return _connectionInfo;
            }
        }

        #endregion

        #region Private Methods

        [OleDbPermission(SecurityAction.Demand)]
        private void BeginTransactionImpl() {
            var returnValue = (PMReturnCode) _database.SQLBeginTrans();
            if(returnValue != PMReturnCode.PMTrue) {
                throw new PMErrorException(_database, "SQLBeginTrans", (int) returnValue);
            }

        }

        [OleDbPermission(SecurityAction.Demand)]
        private void CommitTransactionImpl() {
            var returnValue = (PMReturnCode) _database.SQLCommitTrans();
            if(returnValue != PMReturnCode.PMTrue) {
                throw new PMErrorException(_database, "SQLCommitTrans", (int) returnValue);
            }

        }

        [OleDbPermission(SecurityAction.Demand)]
        private void RollbackTransactionImpl() {
            var returnValue = (PMReturnCode) _database.SQLRollbackTrans();
            if(returnValue != PMReturnCode.PMTrue) {
                throw new PMErrorException(_database, "SQLRollbackTrans", (int) returnValue);
            }

        }

        private dPMDAO.CommandInfo GetCommandInfo(SiriusCommand command) {

            var commandInfo = new dPMDAO.CommandInfo();

            // Copy all properties that have the same representation in both interfaces.
            commandInfo.CommandText = command.SqlCommand.CommandText;
            if(command.CommandTimeoutInherited) {
                commandInfo.CommandTimeout = this.CommandTimeout;
            } else {
                commandInfo.CommandTimeout = command.SqlCommand.CommandTimeout;
            }

            // Convert command type values.
            switch(command.SqlCommand.CommandType) {
            case CommandType.StoredProcedure:
                commandInfo.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc;
                break;
            case CommandType.Text:
                commandInfo.CommandType = ADODB.CommandTypeEnum.adCmdText;
                break;
            default:
                throw new NotSupportedException(Properties.Resources.ErrorCommandTypeInvalid);
            }

            // Convert prepare values.
            commandInfo.Prepare = BooleanDataConvert.ToVBInt16((command.Behaviour & SiriusCommandBehaviour.Prepare) != 0);

            return commandInfo;
        }

        private dPMDAO.ParameterInfo[] GetParameterInfoArray(SiriusCommand command) {
            try {
                var parameterInfoList = new List<dPMDAO.ParameterInfo>();
                foreach(var parameter in command.Parameters.Cast<SqlParameter>()) {
                    parameterInfoList.Add(GetParameterInfo(parameter, command.Behaviour));
                }
                return parameterInfoList.ToArray();
            } catch(Exception ex) {
                if(HandleExceptionFromParameters(ex, command)) {
                    throw;
                }
                return null; // just to remove useless compiler warning
            }
        }

        private void PutParameterInfoArray(SiriusCommand command, dPMDAO.ParameterInfo[] parameterInfoArray) {
            try {
                var parameterInfoList = new List<dPMDAO.ParameterInfo>(parameterInfoArray);
                foreach(var parameterInfo in parameterInfoList) {
                    PutParameterInfo(command.Parameters[parameterInfo.Name], parameterInfo);
                }
            } catch(Exception ex) {
                if(HandleExceptionFromParameters(ex, command)) {
                    throw;
                }
            }
        }

        /// <summary>
        /// Hard-coded exception handling rules for the statement that executes the command.
        /// </summary>
        /// <param name="ex">The exception that has been caught.</param>
        /// <param name="command">The command being executed.</param>
        /// <returns>True if the exception should be re-thrown, otherwise false.</returns>
        [OleDbPermission(SecurityAction.Demand)]
        private bool HandleExceptionFromExecution(Exception ex, SiriusCommand command) {

            // Standard COM HRESULTs.
            const int DISP_E_OVERFLOW = unchecked((int) 0x8002000A);

            if(ex is OverflowException) {
                // Wrap the case where a parameter value marshalled through COM Interop is too big.
                throw new DatabaseException(BadParameterMessage, ex, TranslateErrors(_database.Errors), this, command);
            } else if(ex is COMException) {
                if(ex.Source == "Microsoft OLE DB Provider for SQL Server") {
                    // Wrap all exceptions from the server.
                    throw new DatabaseException(ex.Message, ex, TranslateErrors(_database.Errors), this, command);
                } else if(ex.Source == "ADODB.Command") {
                    // Wrap the case where a parameter value marshalled through COM Interop is the wrong type.
                    throw new DatabaseException(BadParameterMessage, ex, TranslateErrors(_database.Errors), this, command);
                } else if(ex.Source == "Provider" && ((COMException) ex).ErrorCode == DISP_E_OVERFLOW) {
                    // Wrap the case where a parameter value marshalled through COM Interop is too big.
                    throw new DatabaseException(BadParameterMessage, ex, TranslateErrors(_database.Errors), this, command);
                } else {
                    return true;
                }
            } else {
                return true;
            }
        }

        #endregion

        #region Private Shared Methods

        private static dPMDAO.ParameterInfo GetParameterInfo(SqlParameter parameter, SiriusCommandBehaviour behaviour) {

            // Validate the data value manually before it gets passed to COM Interop. If illegal values
            // get that far, then COM Interop will throw inconsistent and unhelpful exceptions.
            ValidateParameterValue(parameter, behaviour);

            var parameterInfo = new dPMDAO.ParameterInfo();

            // Copy all properties that have the same representation in both interfaces.
            parameterInfo.Name = parameter.ParameterName;
            parameterInfo.Size = parameter.Size;
            parameterInfo.Precision = parameter.Precision;
            parameterInfo.NumericScale = parameter.Scale;

            // Convert direction values.
            switch(parameter.Direction) {
            case ParameterDirection.Input:
                parameterInfo.Direction = ADODB.ParameterDirectionEnum.adParamInput;
                break;
            case ParameterDirection.Output:
                parameterInfo.Direction = ADODB.ParameterDirectionEnum.adParamOutput;
                break;
            case ParameterDirection.InputOutput:
                parameterInfo.Direction = ADODB.ParameterDirectionEnum.adParamInputOutput;
                break;
            case ParameterDirection.ReturnValue:
                parameterInfo.Direction = ADODB.ParameterDirectionEnum.adParamReturnValue;
                break;
            default:
                throw new InvalidOperationException(string.Format(Properties.Resources.BadParameterDirectionMessage, parameter.ParameterName, parameter.Direction));
            }

            // Convert type values.
            switch(parameter.SqlDbType) {
            case SqlDbType.BigInt:
                parameterInfo.Type = ADODB.DataTypeEnum.adBigInt;
                break;
            case SqlDbType.Binary:
                parameterInfo.Type = ADODB.DataTypeEnum.adBinary;
                break;
            case SqlDbType.Bit:
                parameterInfo.Type = ADODB.DataTypeEnum.adBoolean;
                break;
            case SqlDbType.Char:
                parameterInfo.Type = ADODB.DataTypeEnum.adChar;
                break;
            //case SqlDbType.Time: // found by experiment that ADO does not like this
            //    parameterInfo.Type = ADODB.DataTypeEnum.adDBTime;
            //    break;
            case SqlDbType.Date:
                parameterInfo.Type = ADODB.DataTypeEnum.adDBDate;
                break;
            case SqlDbType.DateTime:
            case SqlDbType.SmallDateTime:
                //case SqlDbType.DateTime2: // found by experiment that ADO does not like this
                //case SqlDbType.DateTimeOffset: // found by experiment that ADO does not like this
                parameterInfo.Type = ADODB.DataTypeEnum.adDBTimeStamp;
                break;
            case SqlDbType.Decimal:
                parameterInfo.Type = ADODB.DataTypeEnum.adDecimal;
                break;
            case SqlDbType.Float:
                parameterInfo.Type = ADODB.DataTypeEnum.adDouble;
                break;
            case SqlDbType.Image:
                parameterInfo.Type = ADODB.DataTypeEnum.adLongVarBinary;
                parameterInfo.Size = SiriusCommand.ImageSizeOnInput;
                break;
            case SqlDbType.Int:
                parameterInfo.Type = ADODB.DataTypeEnum.adInteger;
                break;
            case SqlDbType.Money:
            case SqlDbType.SmallMoney:
                parameterInfo.Type = ADODB.DataTypeEnum.adCurrency;
                break;
            case SqlDbType.NChar:
                parameterInfo.Type = ADODB.DataTypeEnum.adWChar;
                break;
            case SqlDbType.NText:
                parameterInfo.Type = ADODB.DataTypeEnum.adLongVarWChar;
                parameterInfo.Size = SiriusCommand.NTextSizeOnInput;
                break;
            case SqlDbType.NVarChar:
                parameterInfo.Type = ADODB.DataTypeEnum.adVarWChar;
                break;
            case SqlDbType.Real:
                parameterInfo.Type = ADODB.DataTypeEnum.adSingle;
                break;
            case SqlDbType.SmallInt:
                parameterInfo.Type = ADODB.DataTypeEnum.adSmallInt;
                break;
            case SqlDbType.Text:
                parameterInfo.Type = ADODB.DataTypeEnum.adLongVarChar;
                parameterInfo.Size = SiriusCommand.TextSizeOnInput;
                break;
            case SqlDbType.Timestamp:
                parameterInfo.Type = ADODB.DataTypeEnum.adBinary;
                parameterInfo.Size = SiriusCommand.TimestampSize;
                break;
            case SqlDbType.TinyInt:
                parameterInfo.Type = ADODB.DataTypeEnum.adUnsignedTinyInt;
                break;
            case SqlDbType.Udt:
                parameterInfo.Type = ADODB.DataTypeEnum.adUserDefined;
                break;
            case SqlDbType.UniqueIdentifier:
                parameterInfo.Type = ADODB.DataTypeEnum.adGUID;
                break;
            case SqlDbType.VarBinary:
                parameterInfo.Type = ADODB.DataTypeEnum.adVarBinary;
                break;
            case SqlDbType.VarChar:
                parameterInfo.Type = ADODB.DataTypeEnum.adVarChar;
                break;
            case SqlDbType.Variant:
                parameterInfo.Type = ADODB.DataTypeEnum.adVariant;
                break;
            case SqlDbType.Xml:
                parameterInfo.Type = ADODB.DataTypeEnum.adLongVarWChar;
                parameterInfo.Size = SiriusCommand.NTextSizeOnInput; // TODO: P2 - Test how valid this is for all directions.
                break;
            default:
                throw new InvalidCastException(string.Format(Properties.Resources.BadParameterTypeMessage, parameter.ParameterName, parameter.SqlDbType));
            }

            // Convert attribute values.
            parameterInfo.Attributes = 0;
            if(parameter.IsNullable) {
                parameterInfo.Attributes |= (int) ADODB.ParameterAttributesEnum.adParamNullable;
            }

            // Convert value. Different types are handled as follows:
            // * Null - Interpreted as "missing parameter". Convert to DBNull.Value.
            // * DBNull.Value - Pass through unchanged. The COM interop marshaller and ADO correctly interpret this.
            // * Missing.Value - Convert to null to indicate "missing parameter".
            // * Intrinsic type - Pass through unchanged. The COM interop marshaller and ADO correctly interpret these.
            // * Nullable Of(Intrinsic type) - Pass through unchanged. The COM interop marshaller and ADO correctly interpret these.
            // * SQL type - Not supported directly. Convert to equivalent intrinsic type.
            // * GUID - ADO cannot handle these directly, they must be converted to String, e.g. "{F9B5AEE8-C567-46EA-BBEC-8CD20CC20879}".
            var value = parameter.Value;
            if(value == null) {
                value = DBNull.Value;
            } else if(value == Missing.Value) {
                value = null;
            } else if(value is SqlBoolean) {
                value = Cast.ToObjBoolean((SqlBoolean) value);
            } else if(value is SqlByte) {
                value = Cast.ToObjByte((SqlByte) value);
            } else if(value is SqlInt16) {
                value = Cast.ToObjInt16((SqlInt16) value);
            } else if(value is SqlInt32) {
                value = Cast.ToObjInt32((SqlInt32) value);
            } else if(value is SqlInt64) {
                value = Cast.ToObjInt64((SqlInt64) value);
            } else if(value is SqlSingle) {
                value = Cast.ToObjSingle((SqlSingle) value);
            } else if(value is SqlDouble) {
                value = Cast.ToObjDouble((SqlDouble) value);
            } else if(value is SqlMoney) {
                value = Cast.ToObjDecimal((SqlMoney) value);
            } else if(value is SqlDecimal) {
                value = Cast.ToObjDecimal((SqlDecimal) value);
            } else if(value is SqlDateTime) {
                value = Cast.ToObjDateTime((SqlDateTime) value);
            } else if(value is SqlGuid) {
                value = Cast.ToObjGuid((SqlGuid) value);
            } else if(value is SqlBinary) {
                value = Cast.ToObjByteArray((SqlBinary) value);
            } else if(value is SqlString) {
                value = Cast.ToObjString((SqlString) value);
            }
            // Test separately to catch all GUID types.
            if(value is Guid || value is Guid?) {
                value = Cast.ToString(value);
            }
            parameterInfo.Value = value;

            return parameterInfo;
        }

        private static void PutParameterInfo(SqlParameter parameter, dPMDAO.ParameterInfo parameterInfo) {

            // If direction is input only, there is no need to retrieve the value.
            if(parameterInfo.Direction == ADODB.ParameterDirectionEnum.adParamInput) {
                return;
            }

            // Convert value. Different types are handled as follows:
            // * GUID - ADO passes these back as String, so they must be parsed explicitly.
            // * Int64 - ADO passes these back as Decimal, so they must be cast explicitly.
            // * Others - ADO passes these back correctly.
            switch(parameterInfo.Type) {
            case ADODB.DataTypeEnum.adGUID:
                // Belt-and-braces check; only parse value if it is actually a String,
                // otherwise assume it does not need conversion.
                if(parameterInfo.Value != null && parameterInfo.Value is String) {
                    parameter.Value = Cast.ToGuid(parameterInfo.Value).Value;
                } else {
                    parameter.Value = parameterInfo.Value;
                }
                break;
            case ADODB.DataTypeEnum.adBigInt:
                // Belt-and-braces check; only parse value if it is actually a Decimal,
                // otherwise assume it does not need conversion.
                if(parameterInfo.Value != null && parameterInfo.Value is Decimal) {
                    parameter.Value = Cast.ToInt64(parameterInfo.Value).Value;
                } else {
                    parameter.Value = parameterInfo.Value;
                }
                break;
            default:
                // Let the COM interop marshaller do it.
                parameter.Value = parameterInfo.Value;
                break;
            }
        }

        private static DatabaseErrorCollection TranslateErrors(ADODB.Errors errors) {
            var errorList = new List<DatabaseError>();
            foreach(ADODB.Error item in errors) {
                errorList.Add(new DatabaseError(item.Source, item.Description, item.NativeError, 0, 0, item.SQLState));
            }
            return new DatabaseErrorCollection(errorList);
        }

        #endregion
    }
}
