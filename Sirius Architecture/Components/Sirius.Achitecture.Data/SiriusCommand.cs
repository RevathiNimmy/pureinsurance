using Sirius.Architecture.Utility;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Sirius.Architecture.Data {

    /// <summary>
    /// Represents one database command in SQL Server for use in Sirius code.
    /// This class is a thin wrapper around <see cref="SqlCommand"/>, and can be freely cast to or from an object of that type.
    /// </summary>
    [Serializable]
    public class SiriusCommand : ICloneable, IDisposable, ISerializable {

        #region Public Constants

        /// <summary>
        /// Defined size of a <c>timestamp</c> parameter in a command.
        /// </summary>
        public const int TimestampSize = 8;

        /// <summary>
        /// Defined size of an <c>image</c> parameter in a command (input only).
        /// </summary>
        public const int ImageSizeOnInput = 0x7FFFFFFF; // 2GB

        /// <summary>
        /// Defined size of a <c>text</c> parameter in a command (input only).
        /// </summary>
        public const int TextSizeOnInput = 0x7FFFFFFF; // 2GB

        /// <summary>
        /// Defined size of an <c>ntext</c> parameter in a command (input only).
        /// </summary>
        public const int NTextSizeOnInput = 0x3FFFFFFF; // 1GB

        /// <summary>
        /// Defined size of a <c>varbinary(max)</c> parameter in a command (input only).
        /// </summary>
        public const int VarBinaryMaxSizeOnInput = 0x7FFFFFFF; // same as image

        /// <summary>
        /// Defined size of a <c>varbinary(max)</c> parameter in a command (output only).
        /// </summary>
        public const int VarBinaryMaxSizeOnOutput = 0x1D2AFC80; // found by experimentation in ADO 2.7

        /// <summary>
        /// Defined size of a <c>varchar(max)</c> parameter in a command (input only).
        /// </summary>
        public const int VarCharMaxSizeOnInput = 0x7FFFFFFF; // same as text

        /// <summary>
        /// Defined size of a <c>varchar(max)</c> parameter in a command (output only).
        /// </summary>
        public const int VarCharMaxSizeOnOutput = 0x7FFFFFFE; // found by experimentation in ADO 2.7

        /// <summary>
        /// Defined size of a <c>nvarchar(max)</c> parameter in a command (input only).
        /// </summary>
        public const int NVarCharMaxSizeOnInput = 0x3FFFFFFF; // same as ntext

        /// <summary>
        /// Defined size of a <c>nvarchar(max)</c> parameter in a command (output only).
        /// </summary>
        public const int NVarCharMaxSizeOnOutput = 0x3FFFFFFF; // found by experimentation in ADO 2.7

        #endregion

        #region Private Fields

        private bool _disposed = false;
        private SqlCommand _command;
        private bool _commandOwned;
        private bool _commandTimeoutInherited;
        private SiriusCommandBehaviour _behaviour;

        #endregion

        #region Constructors - FromProcedure

        /// <overloads>
        /// Create a new database command from a stored procedure name.
        /// </overloads>
        /// <summary>
        /// Create a new database command from a stored procedure name.
        /// </summary>
        /// <param name="procedureName">The stored procedure name.</param>
        /// <returns>The command.</returns>
        public static SiriusCommand FromProcedure(string procedureName) {
            return new SiriusCommand(procedureName, 0, CommandType.StoredProcedure, true, SiriusCommandBehaviour.None);
        }

        /// <summary>
        /// Create a new database command from a stored procedure name.
        /// </summary>
        /// <param name="procedureName">The stored procedure name.</param>
        /// <param name="commandTimeout">The timeout value to use.</param>
        /// <returns>The command.</returns>
        public static SiriusCommand FromProcedure(string procedureName, int commandTimeout) {
            return new SiriusCommand(procedureName, commandTimeout, CommandType.StoredProcedure, false, SiriusCommandBehaviour.None);
        }

        /// <summary>
        /// Create a new database command from a stored procedure name.
        /// </summary>
        /// <param name="procedureName">The stored procedure name.</param>
        /// <param name="behaviour">Behaviour flags.</param>
        /// <returns>The command.</returns>
        public static SiriusCommand FromProcedure(string procedureName, SiriusCommandBehaviour behaviour) {
            return new SiriusCommand(procedureName, 0, CommandType.StoredProcedure, true, behaviour);
        }

        /// <summary>
        /// Create a new database command from a stored procedure name.
        /// </summary>
        /// <param name="procedureName">The stored procedure name.</param>
        /// <param name="commandTimeout">The timeout value to use.</param>
        /// <param name="behaviour">Behaviour flags.</param>
        /// <returns>The command.</returns>
        public static SiriusCommand FromProcedure(string procedureName, int commandTimeout, SiriusCommandBehaviour behaviour) {
            return new SiriusCommand(procedureName, commandTimeout, CommandType.StoredProcedure, false, behaviour);
        }

        #endregion

        #region Constructors - FromText

        /// <overloads>
        /// Create a new database command from a SQL statement batch.
        /// </overloads>
        /// <summary>
        /// Create a new database command from a SQL statement batch.
        /// </summary>
        /// <param name="text">The SQL statement batch.</param>
        /// <returns>The command.</returns>
        public static SiriusCommand FromText(string text) {
            return new SiriusCommand(text, 0, CommandType.Text, true, SiriusCommandBehaviour.None);
        }

        /// <summary>
        /// Create a new database command from a SQL statement batch.
        /// </summary>
        /// <param name="text">The SQL statement batch.</param>
        /// <param name="commandTimeout">The timeout value to use.</param>
        /// <returns>The command.</returns>
        public static SiriusCommand FromText(string text, int commandTimeout) {
            return new SiriusCommand(text, commandTimeout, CommandType.Text, false, SiriusCommandBehaviour.None);
        }

        /// <summary>
        /// Create a new database command from a SQL statement batch.
        /// </summary>
        /// <param name="text">The SQL statement batch.</param>
        /// <param name="behaviour">Behaviour flags.</param>
        /// <returns>The command.</returns>
        public static SiriusCommand FromText(string text, SiriusCommandBehaviour behaviour) {
            return new SiriusCommand(text, 0, CommandType.Text, true, behaviour);
        }

        /// <summary>
        /// Create a new database command from a SQL statement batch.
        /// </summary>
        /// <param name="text">The SQL statement batch.</param>
        /// <param name="commandTimeout">The timeout value to use.</param>
        /// <param name="behaviour">Behaviour flags.</param>
        /// <returns>The command.</returns>
        public static SiriusCommand FromText(string text, int commandTimeout, SiriusCommandBehaviour behaviour) {
            return new SiriusCommand(text, commandTimeout, CommandType.Text, false, behaviour);
        }

        /// <summary>
        /// Create a new database command from a SQL statement batch.
        /// </summary>
        /// <param name="text">The SQL statement batch.</param>
        /// <returns>The command.</returns>
        public static SiriusCommand FromText(StringBuilder text) {
            return FromText(text.ToString());
        }

        /// <summary>
        /// Create a new database command from a SQL statement batch.
        /// </summary>
        /// <param name="text">The SQL statement batch.</param>
        /// <param name="commandTimeout">The timeout value to use.</param>
        /// <returns>The command.</returns>
        public static SiriusCommand FromText(StringBuilder text, int commandTimeout) {
            return FromText(text.ToString(), commandTimeout);
        }

        /// <summary>
        /// Create a new database command from a SQL statement batch.
        /// </summary>
        /// <param name="text">The SQL statement batch.</param>
        /// <param name="behaviour">Behaviour flags.</param>
        /// <returns>The command.</returns>
        public static SiriusCommand FromText(StringBuilder text, SiriusCommandBehaviour behaviour) {
            return FromText(text.ToString(), behaviour);
        }

        /// <summary>
        /// Create a new database command from a SQL statement batch.
        /// </summary>
        /// <param name="text">The SQL statement batch.</param>
        /// <param name="commandTimeout">The timeout value to use.</param>
        /// <param name="behaviour">Behaviour flags.</param>
        /// <returns>The command.</returns>
        public static SiriusCommand FromText(StringBuilder text, int commandTimeout, SiriusCommandBehaviour behaviour) {
            return FromText(text.ToString(), commandTimeout, behaviour);
        }

        /// <summary>
        /// Create a new database command from a SQL statement batch.
        /// </summary>
        /// <param name="text">The SQL statement batch.</param>
        /// <returns>The command.</returns>
        public static SiriusCommand FromText(SqlStatementBuilder text) {
            return FromText(text.ToString());
        }

        /// <summary>
        /// Create a new database command from a SQL statement batch.
        /// </summary>
        /// <param name="text">The SQL statement batch.</param>
        /// <param name="commandTimeout">The timeout value to use.</param>
        /// <returns>The command.</returns>
        public static SiriusCommand FromText(SqlStatementBuilder text, int commandTimeout) {
            return FromText(text.ToString(), commandTimeout);
        }

        /// <summary>
        /// Create a new database command from a SQL statement batch.
        /// </summary>
        /// <param name="text">The SQL statement batch.</param>
        /// <param name="behaviour">Behaviour flags.</param>
        /// <returns>The command.</returns>
        public static SiriusCommand FromText(SqlStatementBuilder text, SiriusCommandBehaviour behaviour) {
            return FromText(text.ToString(), behaviour);
        }

        /// <summary>
        /// Create a new database command from a SQL statement batch.
        /// </summary>
        /// <param name="text">The SQL statement batch.</param>
        /// <param name="commandTimeout">The timeout value to use.</param>
        /// <param name="behaviour">Behaviour flags.</param>
        /// <returns>The command.</returns>
        public static SiriusCommand FromText(SqlStatementBuilder text, int commandTimeout, SiriusCommandBehaviour behaviour) {
            return FromText(text.ToString(), commandTimeout, behaviour);
        }

        #endregion

        #region Constructors - Wrap

        /// <overloads>
        /// Wrap an existing <see cref="SqlCommand"/> object and take over control of its lifetime.
        /// </overloads>
        /// <summary>
        /// Wrap an existing <see cref="SqlCommand"/> object and take over control of its lifetime.
        /// By default, the <see cref="DbCommand.CommandTimeout"/> property will be inherited from the connection
        /// and the command will not be prepared.
        /// </summary>
        /// <param name="command">The <see cref="SqlCommand"/> object to wrap.</param>
        /// <returns>The command.</returns>
        public static SiriusCommand Wrap(SqlCommand command) {
            return new SiriusCommand(command, true, SiriusCommandBehaviour.None);
        }

        /// <summary>
        /// Wrap an existing <see cref="SqlCommand"/> object and take over control of its lifetime.
        /// By default, the command will not be prepared.
        /// </summary>
        /// <param name="command">The <see cref="SqlCommand"/> object to wrap.</param>
        /// <param name="commandTimeoutInherited">If true, the <see cref="DbCommand.CommandTimeout"/> property will be inherited from the connection.</param>
        /// <returns>The command.</returns>
        public static SiriusCommand Wrap(SqlCommand command, bool commandTimeoutInherited) {
            return new SiriusCommand(command, commandTimeoutInherited, SiriusCommandBehaviour.None);
        }

        /// <summary>
        /// Wrap an existing <see cref="SqlCommand"/> object and take over control of its lifetime.
        /// By default, the <see cref="DbCommand.CommandTimeout"/> property will be inherited from the connection
        /// and the command will not be prepared.
        /// </summary>
        /// <param name="command">The <see cref="SqlCommand"/> object to wrap.</param>
        /// <param name="behaviour">Behaviour flags.</param>
        /// <returns>The command.</returns>
        public static SiriusCommand Wrap(SqlCommand command, SiriusCommandBehaviour behaviour) {
            return new SiriusCommand(command, true, behaviour);
        }

        /// <summary>
        /// Wrap an existing <see cref="SqlCommand"/> object and take over control of its lifetime.
        /// By default, the command will not be prepared.
        /// </summary>
        /// <param name="command">The <see cref="SqlCommand"/> object to wrap.</param>
        /// <param name="commandTimeoutInherited">If true, the <see cref="DbCommand.CommandTimeout"/> property will be inherited from the connection.</param>
        /// <param name="behaviour">Behaviour flags.</param>
        /// <returns>The command.</returns>
        public static SiriusCommand Wrap(SqlCommand command, bool commandTimeoutInherited, SiriusCommandBehaviour behaviour) {
            return new SiriusCommand(command, commandTimeoutInherited, behaviour);
        }

        #endregion

        #region Constructors - Internal

        /// <summary>
        /// Construct a new object from original source data.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <param name="commandTimeoutInherited"></param>
        /// <param name="behaviour"></param>
        private SiriusCommand(string commandText,
            int commandTimeout,
            CommandType commandType,
            bool commandTimeoutInherited,
            SiriusCommandBehaviour behaviour) {
            _command = new SqlCommand(commandText);
            _command.CommandTimeout = commandTimeout;
            _command.CommandType = commandType;
            _commandOwned = true;
            _commandTimeoutInherited = commandTimeoutInherited;
            _behaviour = behaviour;
        }

        /// <summary>
        /// Construct a new object by wrapping an existing <see cref="SqlCommand"/>.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandTimeoutInherited"></param>
        /// <param name="behaviour"></param>
        private SiriusCommand(SqlCommand command,
            bool commandTimeoutInherited,
            SiriusCommandBehaviour behaviour) {
            _command = command;
            _commandOwned = false;
            _commandTimeoutInherited = commandTimeoutInherited;
            _behaviour = behaviour;
        }

        /// <summary>
        /// Initialise a new instance of the class with serialized data.
        /// </summary>
        /// <param name="info">Serialized object data</param>
        /// <param name="context">Contextual information about the source or destination</param>
        protected SiriusCommand(SerializationInfo info, StreamingContext context) {
            if(info == null) {
                throw new ArgumentNullException("info");
            }
            _command = new SqlCommand();
            SetObjectData(info, context, "_command", _command);
            _commandTimeoutInherited = info.GetBoolean("_commandTimeoutInherited");
            _behaviour = (SiriusCommandBehaviour) info.GetValue("_behaviour", typeof(SiriusCommandBehaviour));
            _commandOwned = true;
        }

        private static void SetObjectData(SerializationInfo info, StreamingContext context, string name, SqlCommand value) {
            // Deserialise just enough data to be useful in exception propogation.
            value.CommandText = info.GetString(name + ".CommandText");
            value.CommandTimeout = info.GetInt32(name + ".CommandTimeout");
            value.CommandType = (CommandType) info.GetValue(name + ".CommandType", typeof(CommandType));
            value.DesignTimeVisible = info.GetBoolean(name + ".DesignTimeVisible");
            for(int i = 0; i < info.GetInt32(name + ".Parameters.Count"); i++) {
                SqlParameter parameter = new SqlParameter();
                SetObjectData(info, context, name + ".Parameters[" + i.ToString(CultureInfo.InvariantCulture) + "]", parameter);
                value.Parameters.Add(parameter);
            }
        }

        private static void SetObjectData(SerializationInfo info, StreamingContext context, string name, SqlParameter value) {
            // Deserialise just enough data to be useful in exception propogation.
            value.ParameterName = info.GetString(name + ".ParameterName");
            value.Direction = (ParameterDirection) info.GetValue(name + ".Direction", typeof(ParameterDirection));
            value.SqlDbType = (SqlDbType) info.GetValue(name + ".SqlDbType", typeof(SqlDbType));
            value.Size = info.GetInt32(name + ".Size");
            value.Precision = info.GetByte(name + ".Precision");
            value.Scale = info.GetByte(name + ".Scale");
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
            GetObjectData(info, context, "_command", _command);
            info.AddValue("_commandTimeoutInherited", _commandTimeoutInherited);
            info.AddValue("_behaviour", _behaviour, typeof(SiriusCommandBehaviour));
        }

        private static void GetObjectData(SerializationInfo info, StreamingContext context, string name, SqlCommand value) {
            // Serialise just enough data to be useful in exception propogation.
            info.AddValue(name + ".CommandText", value.CommandText);
            info.AddValue(name + ".CommandTimeout", value.CommandTimeout);
            info.AddValue(name + ".CommandType", value.CommandType, typeof(CommandType));
            info.AddValue(name + ".DesignTimeVisible", value.DesignTimeVisible);
            info.AddValue(name + ".Parameters.Count", value.Parameters.Count);
            for(int i = 0; i < value.Parameters.Count; i++) {
                GetObjectData(info, context, name + ".Parameters[" + i.ToString(CultureInfo.InvariantCulture) + "]", value.Parameters[i]);
            }
        }

        private static void GetObjectData(SerializationInfo info, StreamingContext context, string name, SqlParameter value) {
            // Serialise just enough data to be useful in exception propogation.
            info.AddValue(name + ".ParameterName", value.ParameterName);
            info.AddValue(name + ".Direction", value.Direction, typeof(ParameterDirection));
            info.AddValue(name + ".SqlDbType", value.SqlDbType, typeof(SqlDbType));
            info.AddValue(name + ".Size", value.Size);
            info.AddValue(name + ".Precision", value.Precision);
            info.AddValue(name + ".Scale", value.Scale);
        }

        #endregion

        #region Finalizers

        /// <summary>
        /// Release unmanaged resources and performs other cleanup operations before the object is reclaimed by garbage collection.
        /// </summary>
        ~SiriusCommand() {
            Dispose(false);
        }

        /// <overloads>
        /// Release all resources used by this object.
        /// </overloads>
        /// <summary>
        /// Release all resources used by this object.
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release all unmanaged resources used by this object and optionally release managed resources as well.
        /// </summary>
        /// <param name="disposing">Release managed resources?</param>
        protected virtual void Dispose(bool disposing) {
            if(!_disposed) {
                if(disposing) {
                    // Dispose managed resources.
                    if(_command != null && _commandOwned) {
                        _command.Dispose();
                    }
                }
                // Dispose unmanaged resources.
            }
            _disposed = true;
        }

        /// <summary>
        /// Throw an exception if the calling code tries to use this object after it has been disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The disposed flag is true.</exception>
        private void DisposedCheck() {
            if(_disposed) {
                throw new ObjectDisposedException(this.GetType().FullName, Properties.Resources.ObjectDisposedExceptionMessage);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The wrapped <see cref="SqlCommand"/> object.
        /// </summary>
        public SqlCommand SqlCommand {
            get {
                DisposedCheck();
                return _command;
            }
        }

        /// <summary>
        /// The wrapped <see cref="DbCommand.CommandText"/> value, for convenience.
        /// </summary>
        public string CommandText {
            get {
                return _command.CommandText;
            }
        }

        /// <summary>
        /// The wrapped <see cref="DbCommand.Parameters"/> collection, for convenience.
        /// </summary>
        public SqlParameterCollection Parameters {
            get {
                DisposedCheck();
                return _command.Parameters;
            }
        }

        /// <summary>
        /// The wrapped return value parameter.
        /// </summary>
        /// <remarks>
        /// The return value is assumed to be the parameter with a name of <c>"RETURN_VALUE"</c> and a data type of <see cref="SqlDbType.Int"/>.
        /// </remarks>
        public SqlParameter ReturnValue {
            get {
                DisposedCheck();
                return _command.Parameters["RETURN_VALUE"];
            }
        }

        /// <summary>
        /// Inherit the connection's default command timeout value when the command is executed?
        /// </summary>
        public bool CommandTimeoutInherited {
            get {
                DisposedCheck();
                return _commandTimeoutInherited;
            }
            set {
                DisposedCheck();
                _commandTimeoutInherited = value;
            }
        }

        /// <summary>
        /// Flags that alter the default behaviour before execution.
        /// </summary>
        public SiriusCommandBehaviour Behaviour {
            get {
                DisposedCheck();
                return _behaviour;
            }
            set {
                DisposedCheck();
                _behaviour = value;
            }
        }

        #endregion

        #region Public Methods - AddParameter

        /// <overloads>
        /// Add a parameter to the command.
        /// </overloads>
        /// <summary>
        /// Add a parameter to the command.
        /// Use this overload for data types that do not have a size parameter, e.g. <c>int</c>.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="dataType">Parameter data type.</param>
        /// <param name="direction">Parameter direction.</param>
        /// <returns>The parameter.</returns>
        public SqlParameter AddParameter(string name, SqlDbType dataType, ParameterDirection direction) {
            DisposedCheck();
            var p = new SqlParameter(name, dataType) { Direction = direction };
            return _command.Parameters.Add(p);
        }

        /// <summary>
        /// Add a parameter to the command.
        /// Use this overload for data types that have one size parameter, e.g. <c>nvarchar(&lt;length&gt;)</c>.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="dataType">Parameter data type.</param>
        /// <param name="size">Parameter size.</param>
        /// <param name="direction">Parameter direction.</param>
        /// <returns>The parameter.</returns>
        /// <remarks>
        /// This class defines size constants for use with <c>image</c>, <c>text</c>, <c>ntext</c>,
        /// <c>varbinary(max)</c>, <c>varchar(max)</c>, <c>nvarchar(max)</c> and <c>timestamp</c> data types.
        /// Please use these constants instead of guessing an appropriate value.
        /// </remarks>
        public SqlParameter AddParameter(string name, SqlDbType dataType, int size, ParameterDirection direction) {
            DisposedCheck();
            var p = new SqlParameter(name, dataType, size) { Direction = direction };
            return _command.Parameters.Add(p);
        }

        /// <summary>
        /// Add a parameter to the command.
        /// Use this overload for data types that have two size parameters, e.g. <c>decimal(&lt;precision&gt;, &lt;scale&gt;)</c>.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="dataType">Parameter data type.</param>
        /// <param name="precision">Parameter precision.</param>
        /// <param name="scale">Parameter scale.</param>
        /// <param name="direction">Parameter direction.</param>
        /// <returns>The parameter.</returns>
        public SqlParameter AddParameter(string name, SqlDbType dataType, byte precision, byte scale, ParameterDirection direction) {
            DisposedCheck();
            var p = new SqlParameter(name, dataType) { Direction = direction, Precision = precision, Scale = scale };
            return _command.Parameters.Add(p);
        }

        /// <summary>
        /// Add a parameter to the command.
        /// Use this overload for data types that have a type name, e.g. table-valued and user-defined.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="dataType">Parameter data type.</param>
        /// <param name="typeName">Parameter data type name.</param>
        /// <param name="direction">Parameter direction.</param>
        /// <returns>The parameter.</returns>
        public SqlParameter AddParameter(string name, SqlDbType dataType, string typeName, ParameterDirection direction) {
            DisposedCheck();
            var p = new SqlParameter(name, dataType) { Direction = direction };
            if(dataType == SqlDbType.Structured) {
                p.TypeName = typeName;
            } else if(dataType == SqlDbType.Udt) {
                p.UdtTypeName = typeName;
            }
            return _command.Parameters.Add(p);
        }

        /// <summary>
        /// Add a return value parameter to the command.
        /// </summary>
        /// <returns>The parameter.</returns>
        /// <remarks>
        /// The parameter is created with a name of <c>"RETURN_VALUE"</c> and a data type of <c>int</c>.
        /// </remarks>
        public SqlParameter AddReturnValue() {
            return AddParameter("RETURN_VALUE", SqlDbType.Int, ParameterDirection.ReturnValue);
        }

        /// <overloads>
        /// Add an input parameter to the command.
        /// </overloads>
        /// <summary>
        /// Add an input parameter to the command.
        /// Use this overload for data types that do not have a size parameter, e.g. <c>int</c>.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="dataType">Parameter data type.</param>
        /// <returns>The parameter.</returns>
        public SqlParameter AddInParameter(string name, SqlDbType dataType) {
            return AddParameter(name, dataType, ParameterDirection.Input);
        }

        /// <summary>
        /// Add an input parameter to the command.
        /// Use this overload for data types that have one size parameter, e.g. <c>nvarchar(&lt;length&gt;)</c>.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="dataType">Parameter data type.</param>
        /// <param name="size">Parameter size.</param>
        /// <returns>The parameter.</returns>
        /// <remarks>
        /// This class defines size constants for use with <c>image</c>, <c>text</c>, <c>ntext</c>,
        /// <c>varbinary(max)</c>, <c>varchar(max)</c>, <c>nvarchar(max)</c> and <c>timestamp</c> data types.
        /// Please use these constants instead of guessing an appropriate value.
        /// </remarks>
        public SqlParameter AddInParameter(string name, SqlDbType dataType, int size) {
            return AddParameter(name, dataType, size, ParameterDirection.Input);
        }

        /// <summary>
        /// Add an input parameter to the command.
        /// Use this overload for data types that have two size parameters, e.g. <c>decimal(&lt;precision&gt;, &lt;scale&gt;)</c>.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="dataType">Parameter data type.</param>
        /// <param name="precision">Parameter precision.</param>
        /// <param name="scale">Parameter scale.</param>
        /// <returns>The parameter.</returns>
        public SqlParameter AddInParameter(string name, SqlDbType dataType, byte precision, byte scale) {
            return AddParameter(name, dataType, precision, scale, ParameterDirection.Input);
        }

        /// <summary>
        /// Add an input parameter to the command.
        /// Use this overload for data types that have a type name, e.g. table-valued and user-defined.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="dataType">Parameter data type.</param>
        /// <param name="typeName">Parameter data type name.</param>
        /// <returns>The parameter.</returns>
        public SqlParameter AddInParameter(string name, SqlDbType dataType, string typeName) {
            return AddParameter(name, dataType, typeName, ParameterDirection.Input);
        }

        /// <overloads>
        /// Add an output parameter to the command.
        /// </overloads>
        /// <summary>
        /// Add an output parameter to the command.
        /// Use this overload for data types that do not have a size parameter, e.g. <c>int</c>.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="dataType">Parameter data type.</param>
        /// <returns>The parameter.</returns>
        public SqlParameter AddOutParameter(string name, SqlDbType dataType) {
            return AddParameter(name, dataType, ParameterDirection.Output);
        }

        /// <summary>
        /// Add an output parameter to the command.
        /// Use this overload for data types that have one size parameter, e.g. <c>nvarchar(&lt;length&gt;)</c>.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="dataType">Parameter data type.</param>
        /// <param name="size">Parameter size.</param>
        /// <returns>The parameter.</returns>
        /// <remarks>
        /// This class defines size constants for use with <c>image</c>, <c>text</c>, <c>ntext</c>,
        /// <c>varbinary(max)</c>, <c>varchar(max)</c>, <c>nvarchar(max)</c> and <c>timestamp</c> data types.
        /// Please use these constants instead of guessing an appropriate value.
        /// </remarks>
        public SqlParameter AddOutParameter(string name, SqlDbType dataType, int size) {
            return AddParameter(name, dataType, size, ParameterDirection.Output);
        }

        /// <summary>
        /// Add an output parameter to the command.
        /// Use this overload for data types that have two size parameters, e.g. <c>decimal(&lt;precision&gt;, &lt;scale&gt;)</c>.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="dataType">Parameter data type.</param>
        /// <param name="precision">Parameter precision.</param>
        /// <param name="scale">Parameter scale.</param>
        /// <returns>The parameter.</returns>
        public SqlParameter AddOutParameter(string name, SqlDbType dataType, byte precision, byte scale) {
            return AddParameter(name, dataType, precision, scale, ParameterDirection.Output);
        }

        /// <summary>
        /// Add an output parameter to the command.
        /// Use this overload for data types that have a type name, e.g. table-valued and user-defined.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="dataType">Parameter data type.</param>
        /// <param name="typeName">Parameter data type name.</param>
        /// <returns>The parameter.</returns>
        public SqlParameter AddOutParameter(string name, SqlDbType dataType, string typeName) {
            return AddParameter(name, dataType, typeName, ParameterDirection.Output);
        }

        /// <overloads>
        /// Add an input/output parameter to the command.
        /// </overloads>
        /// <summary>
        /// Add an input/output parameter to the command.
        /// Use this overload for data types that do not have a size parameter, e.g. <c>int</c>.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="dataType">Parameter data type.</param>
        /// <returns>The parameter.</returns>
        public SqlParameter AddInOutParameter(string name, SqlDbType dataType) {
            return AddParameter(name, dataType, ParameterDirection.InputOutput);
        }

        /// <summary>
        /// Add an input/output parameter to the command.
        /// Use this overload for data types that have one size parameter, e.g. <c>nvarchar(&lt;length&gt;)</c>.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="dataType">Parameter data type.</param>
        /// <param name="size">Parameter size.</param>
        /// <returns>The parameter.</returns>
        /// <remarks>
        /// This class defines size constants for use with <c>image</c>, <c>text</c>, <c>ntext</c>,
        /// <c>varbinary(max)</c>, <c>varchar(max)</c>, <c>nvarchar(max)</c> and <c>timestamp</c> data types.
        /// Please use these constants instead of guessing an appropriate value.
        /// </remarks>
        public SqlParameter AddInOutParameter(string name, SqlDbType dataType, int size) {
            return AddParameter(name, dataType, size, ParameterDirection.InputOutput);
        }

        /// <summary>
        /// Add an input/output parameter to the command.
        /// Use this overload for data types that have two size parameters, e.g. <c>decimal(&lt;precision&gt;, &lt;scale&gt;)</c>.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="dataType">Parameter data type.</param>
        /// <param name="precision">Parameter precision.</param>
        /// <param name="scale">Parameter scale.</param>
        /// <returns>The parameter.</returns>
        public SqlParameter AddInOutParameter(string name, SqlDbType dataType, byte precision, byte scale) {
            return AddParameter(name, dataType, precision, scale, ParameterDirection.InputOutput);
        }

        /// <summary>
        /// Add an input/output parameter to the command.
        /// Use this overload for data types that have a type name, e.g. table-valued and user-defined.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="dataType">Parameter data type.</param>
        /// <param name="typeName">Parameter data type name.</param>
        /// <returns>The parameter.</returns>
        public SqlParameter AddInOutParameter(string name, SqlDbType dataType, string typeName) {
            return AddParameter(name, dataType, typeName, ParameterDirection.InputOutput);
        }

        #endregion

        #region Public Methods - ResetAllParameterValues

        /// <summary>
        /// Reset all parameter values to <see cref="DBNull.Value"/>. Most useful inside a loop.
        /// </summary>
        public void ResetAllParameterValues() {
            DisposedCheck();
            foreach(SqlParameter parameter in _command.Parameters) {
                parameter.Value = DBNull.Value;
            }
        }

        #endregion

        #region Public Methods - ToString

        /// <summary>
        /// Return a textual representation of the current state of the command, in a format suitable for error logging.
        /// </summary>
        public override string ToString() {
            // Hard-code the formatting because it contains only non-localisable bits.
            return string.Format(
                @"CommandType = {0}, CommandText = ""{1}"", CommandTimeout = {2}, Behaviour = {3}, Parameters = {4}",
                _command.CommandType,
                _command.CommandText,
                CommandTimeoutToString(),
                _behaviour,
                ParametersToString());
        }

        #endregion

        #region Public Shared Operators

        /// <summary>
        /// Convert a <see cref="SqlCommand"/> to a <see cref="SiriusCommand"/>, using default settings.
        /// </summary>
        /// <param name="command">The command to wrap.</param>
        /// <returns>The wrapped command.</returns>
        public static implicit operator SiriusCommand(SqlCommand command) {
            return Wrap(command);
        }

        /// <summary>
        /// Convert a <see cref="SiriusCommand"/> to a <see cref="SqlCommand"/>, losing the extra state provided by this class.
        /// </summary>
        /// <param name="command">The command to unwrap.</param>
        /// <returns>The unwrapped command.</returns>
        public static explicit operator SqlCommand(SiriusCommand command) {
            return command.SqlCommand;
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
        public SiriusCommand Clone() {
            DisposedCheck();
            return new SiriusCommand(_command.Clone(), _commandTimeoutInherited, _behaviour);
        }

        #endregion

        #region Private Methods

        private string CommandTimeoutToString() {
            // Hard-code the formatting because it contains only non-localisable bits.
            if(_commandTimeoutInherited) {
                return "Inherited";
            } else {
                return _command.CommandTimeout.ToString();
            }
        }

        private string ParametersToString() {
            return VBCodeFormat.ToString<SqlParameter>(_command.Parameters, ParameterToString, false);
        }

        #endregion

        #region Private Shared Methods

        private static string ParameterToString(SqlParameter parameter) {
            // Hard-code the formatting because it contains only non-localisable bits.
            return string.Format(
                @"Name = ""{0}"", Type = {1}, Direction = {2}, Value = {3}",
                parameter.ParameterName,
                TransactSqlFormat.ParameterTypeToString(parameter),
                parameter.Direction,
                ParameterValueToString(parameter));
        }

        private static string ParameterValueToString(SqlParameter parameter) {
            try {
                return TransactSqlFormat.ToString(parameter);
            } catch(InvalidCastException ex) {
                return "<" + ex.Message + ">";
            }
        }

        #endregion
    }
}
