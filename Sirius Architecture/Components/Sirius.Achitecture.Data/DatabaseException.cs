using System;
using System.ComponentModel;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Sirius.Architecture.Data {

    /// <summary>
    /// This exception indicates that a database server error occurred when executing a command.
    /// </summary>
    [Serializable]
    public class DatabaseException : DbException {

        #region Private Fields

        private DatabaseErrorCollection _errors;
        private SiriusCommand _command;
        private int _commandTimeout;
        private SiriusConnectionSchema _schema;
        private int _transactionCount;

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new instance of this class with all required information.
        /// This constructor supports <see cref="SiriusConnection"/>, and is not intended to be called directly in your code.
        /// </summary>
        /// <param name="message">The error message string.</param>
        /// <param name="innerException">The inner exception reference.</param>
        /// <param name="errors">The database errors from the connection.</param>
        /// <param name="connection">The connection in use at the time.</param>
        /// <param name="command">The command being executed at the time.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public DatabaseException(string message, Exception innerException, DatabaseErrorCollection errors, SiriusConnection connection, SiriusCommand command)
            : base(message, innerException) {

            if(innerException is ExternalException) {
                HResult = ((ExternalException) innerException).ErrorCode;
            }

            _errors = errors;
            _command = command.Clone();
            if(command.CommandTimeoutInherited) {
                _commandTimeout = connection.CommandTimeout;
            } else {
                _commandTimeout = command.SqlCommand.CommandTimeout;
            }
            _schema = connection.Schema;
            _transactionCount = connection.TransactionCount;

        }

        /// <summary>
        /// Initialise a new instance of the class with serialized data.
        /// </summary>
        /// <param name="info">Serialized object data</param>
        /// <param name="context">Contextual information about the source or destination</param>
        protected DatabaseException(SerializationInfo info, StreamingContext context)
            : base(info, context) {

            if(info == null) {
                throw new ArgumentNullException("info");
            }
            _errors = (DatabaseErrorCollection) info.GetValue("_errors", typeof(DatabaseErrorCollection));
            _command = (SiriusCommand) info.GetValue("_command", typeof(SiriusCommand));
            _commandTimeout = info.GetInt32("_commandTimeout");
            _schema = (SiriusConnectionSchema) info.GetValue("_schema", typeof(SiriusConnectionSchema));
            _transactionCount = info.GetInt32("_transactionCount");
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
            info.AddValue("_errors", _errors, typeof(DatabaseErrorCollection));
            info.AddValue("_command", _command, typeof(SiriusCommand));
            info.AddValue("_commandTimeout", _commandTimeout, typeof(int));
            info.AddValue("_schema", _schema, typeof(SiriusConnectionSchema));
            info.AddValue("_transactionCount", _transactionCount, typeof(int));
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// A list of all the errors from the underlying data provider.
        /// </summary>
        public DatabaseErrorCollection Errors {
            get {
                return _errors;
            }
        }

        /// <summary>
        /// A copy of the command as it was when the exception occurred.
        /// </summary>
        public SiriusCommand Command {
            get {
                return _command;
            }
        }

        /// <summary>
        /// The command timeout that was in effect when the exception occurred.
        /// </summary>
        public int CommandTimeout {
            get {
                return _commandTimeout;
            }
        }

        /// <summary>
        /// The schema of the connection when the exception occurred.
        /// </summary>
        public SiriusConnectionSchema Schema {
            get {
                return _schema;
            }
        }

        /// <summary>
        /// The number of logical nested transactions open when the exception occurred.
        /// </summary>
        public int TransactionCount {
            get {
                return _transactionCount;
            }
        }

        #endregion

    }

}
