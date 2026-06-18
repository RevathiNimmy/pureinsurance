// ****************************************************************************************************
// ****************************************************************************************************
// ****************************************************************************************************
// ATTENTION!
// This class inherits from the SiriusConnection class in SA.NET, and as such, the code is
// very tightly coupled to the way that SA.NET works. If you make any unauthorised changes,
// you run the risk of breaking it. If you think you need to change anything, please check
// with Christian Hayter first.
// ****************************************************************************************************
// ****************************************************************************************************
// ****************************************************************************************************

using Sirius.Architecture.Data;
using Sirius.Architecture.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.XPath;

namespace dPMDAOBridge {

    /// <summary>
    /// Wraps a <see cref="dPMDAO.Database"/> object for use with code that expects a <see cref="SiriusConnection"/>.
    /// </summary>
    public class SiriusConnectionPMDAO : SiriusConnection {

        #region Private Variables

        private dPMDAO.Database _database;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new database connection from Sirius server-side global state information.
        /// </summary>
        /// <param name="siriusUserName">Data normally used to initialise dPMDAO.</param>
        /// <param name="sourceID">Data normally used to initialise dPMDAO.</param>
        /// <param name="languageID">Data normally used to initialise dPMDAO.</param>
        /// <param name="callingAppName">Data normally used to initialise dPMDAO.</param>
        public SiriusConnectionPMDAO(string siriusUserName,
            int sourceID,
            int languageID,
            string callingAppName)
            : this(siriusUserName, sourceID, languageID, callingAppName, "", "", "", "") {
        }

        /// <summary>
        /// Create a new database connection from Sirius server-side global state information.
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
            int sourceID,
            int languageID,
            string callingAppName,
            string userName,
            string password,
            string dataSourceName,
            string databaseName) {

            _database = new dPMDAO.Database();

            PMReturnCode returnValue = (PMReturnCode) _database.OpenDatabase(ref siriusUserName,
                ref sourceID,
                ref languageID,
                ref callingAppName,
                ref userName,
                ref password,
                ref dataSourceName,
                ref databaseName);

            if(returnValue != PMReturnCode.PMTrue) {
                throw new PMErrorException(_database, "OpenDatabase", (int) returnValue);
            }

            this.CommandTimeout = _database.QueryTimeout;
            this.Schema = SiriusConnectionSchema.Sirius;

            base.CacheKey = string.Format(CultureInfo.InvariantCulture, "PMDAO|{0}|{1}",
                dataSourceName,
                databaseName);

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
                }
                // Dispose unmanaged resources.
                if(_database != null) {
                    PMReturnCode returnValue = (PMReturnCode) _database.CloseDatabase();
                    if(disposing && (returnValue != PMReturnCode.PMTrue)) {
                        throw new PMErrorException(_database, "CloseDatabase", (int) returnValue);
                    }
                    _database = null;
                }
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
        public override object /*dPMDAO.Database*/ PMDAODatabase {
            get {
                DisposedCheck();
                return _database;
            }
        }

        #endregion

        #region Public Methods - Clone

        /// <summary>
        /// This method is not applicable to this particular derived class.
        /// </summary>
        /// <exception cref="NotSupportedException">A PMDAO connection cannot be cloned.</exception>
        public override SiriusConnection Clone() {

            throw new NotSupportedException();

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

            // Safety check.
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

            // Safety check.
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
        public override int ExecuteNonQuery(SiriusCommand command) {

            DisposedCheck();

            // Correct parameter values.
            ValidateParameterArray(command);

            // Execute the command.
            int rowsAffected = 0;
            try {
                rowsAffected = _database.ExecuteNonQuery(command.SqlCommand);
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
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

            // Correct parameter values.
            ValidateParameterArray(command);

            // Execute the command.
            object value = null;
            try {
                value = _database.ExecuteScalar(command.SqlCommand);
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
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

            // Correct parameter values.
            ValidateParameterArray(command);

            // Execute the command.
            int rowsAffected = 0;
            try {
                rowsAffected = _database.ExecuteDataTable(command.SqlCommand, adapterImpl, results);
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
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

            // Correct parameter values.
            ValidateParameterArray(command);

            // Execute the command.
            int rowsAffected = 0;
            try {
                rowsAffected = _database.ExecuteDataSet(command.SqlCommand, adapterImpl, results, tableName);
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
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

            // Correct parameter values.
            ValidateParameterArray(command);

            // Execute the command.
            List<T> results = null;
            try {
                results = _database.ExecuteList<T>(command.SqlCommand, reader => {
                    T row = new T();
                    ((IDataReadable) row).SetObjectData(reader);
                    return row;
                });
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
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

            // Correct parameter values.
            ValidateParameterArray(command);

            // Execute the command.
            string results = null;
            try {
                results = _database.ExecuteXmlText(command.SqlCommand);
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
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

            // Correct parameter values.
            ValidateParameterArray(command);

            // Execute the command.
            XPathDocument results = null;
            try {
                results = _database.ExecuteXPathDocument(command.SqlCommand);
            } catch(Exception ex) {
                if(HandleExceptionFromExecution(ex, command)) {
                    throw;
                }
            }

            return results;

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

        public override void EndStateful()
        {
            throw new NotSupportedException();
        }
        public override void BeginStateful()
        {
            throw new NotSupportedException();
        }
        public override List<T> ExecuteList<T>(SiriusCommand command, Converter<IDataRecord, T> convert)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region Private Methods

        private void BeginTransactionImpl() {

            PMReturnCode returnValue = (PMReturnCode) _database.SQLBeginTrans();
            if(returnValue != PMReturnCode.PMTrue) {
                throw new PMErrorException(_database, "SQLBeginTrans", (int) returnValue);
            }

        }

        private void CommitTransactionImpl() {

            PMReturnCode returnValue = (PMReturnCode) _database.SQLCommitTrans();
            if(returnValue != PMReturnCode.PMTrue) {
                throw new PMErrorException(_database, "SQLCommitTrans", (int) returnValue);
            }

        }

        private void RollbackTransactionImpl() {

            PMReturnCode returnValue = (PMReturnCode) _database.SQLRollbackTrans();
            if(returnValue != PMReturnCode.PMTrue) {
                throw new PMErrorException(_database, "SQLRollbackTrans", (int) returnValue);
            }

        }

        private void ValidateParameterArray(SiriusCommand command) {

            try {
                foreach(SqlParameter parameter in command.SqlCommand.Parameters) {
                    ValidateParameter(parameter, command.Behaviour);
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
        private bool HandleExceptionFromExecution(Exception ex, SiriusCommand command) {

            if(ex is ArgumentException || ex is FormatException || ex is InvalidCastException || ex is OverflowException) {
                if(ex.Source == "System.Data") {
                    // Wrap the case where a parameter value passed to SqlClient is the wrong type or size.
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
