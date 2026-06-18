using Sirius.Architecture.Utility;
using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlTypes;
using System.Security.Permissions;

namespace Sirius.Architecture.Data.BackOffice {

    /// <summary>
    /// Quick and dirty wrapper class to make an ADO Recordset look like a .NET data reader that can be disposed properly.
    /// It implements enough of the IDataRecord interface to be useable in the ExecuteList methods.
    /// </summary>
    internal class AdoDataReader : IDataReader {

        #region Private Fields

        private bool _disposed = false;
        private ADODB.Recordset _recordset;
        private bool _firstRow;

        #endregion

        #region Constructors

        /// <summary>
        /// Take ownership of the lifetime of the specified ADO recordset.
        /// </summary>
        /// <param name="recordset">Recordset to wrap</param>
        /// <exception cref="ArgumentNullException">Recordset is null</exception>
        public AdoDataReader(ADODB.Recordset recordset) {

            if(recordset == null) {
                throw new ArgumentNullException("recordset");
            }
            _recordset = recordset;
            _firstRow = true;
        }

        #endregion

        #region Finalizers

        /// <summary>
        /// Release unmanaged resources and performs other cleanup operations before the object is reclaimed by garbage collection.
        /// </summary>
        ~AdoDataReader() {
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
                    try {
                        if(_recordset != null) {
                            if(_recordset.State != (int) ADODB.ObjectStateEnum.adStateClosed) {
                                _recordset.Close();
                            }
                        }
                        RcwHelper.Release(_recordset);
                    } catch {
                        // Absorb all exceptions.
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
        /// The ADO recordset wrapped by this object, for passing to OleDbDataAdapter objects.
        /// </summary>
        public ADODB.Recordset Recordset {
            [OleDbPermission(SecurityAction.Demand)]
            get {
                return _recordset;
            }
        }

        #endregion

        #region IDataReader Members

        public void Close() {
            Dispose();
        }

        public int Depth {
            get {
                throw new NotSupportedException();
            }
        }

        public DataTable GetSchemaTable() {
            throw new NotSupportedException();
        }

        public bool IsClosed {
            [OleDbPermission(SecurityAction.Demand)]
            get {
                return _disposed || _recordset == null || _recordset.State != (int) ADODB.ObjectStateEnum.adStateOpen;
            }
        }

        public bool NextResult() {
            throw new NotSupportedException();
        }

        [OleDbPermission(SecurityAction.Demand)]
        public bool Read() {

            if(IsClosed || _recordset.EOF) {
                return false;
            } else if(_firstRow) {
                _firstRow = false;
                return true;
            } else {
                _recordset.MoveNext();
                return !_recordset.EOF;
            }
        }

        public int RecordsAffected {
            get {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region IDataRecord Members - Get<T>

        public Boolean GetBoolean(int i) {
            return Cast.ToBoolean(this[i]).Value;
        }

        public Byte GetByte(int i) {
            return Cast.ToByte(this[i]).Value;
        }

        public Int16 GetInt16(int i) {
            return Cast.ToInt16(this[i]).Value;
        }

        public Int32 GetInt32(int i) {
            return Cast.ToInt32(this[i]).Value;
        }

        public Int64 GetInt64(int i) {
            return Cast.ToInt64(this[i]).Value;
        }

        public Single GetFloat(int i) {
            return Cast.ToSingle(this[i]).Value;
        }

        public Double GetDouble(int i) {
            return Cast.ToDouble(this[i]).Value;
        }

        public Decimal GetDecimal(int i) {
            return Cast.ToDecimal(this[i]).Value;
        }

        public DateTime GetDateTime(int i) {
            return Cast.ToDateTime(this[i]).Value;
        }

        public Guid GetGuid(int i) {
            return Cast.ToGuid(this[i]).Value;
        }

        public String GetString(int i) {
            return Cast.ToString(this[i]);
        }

        #endregion

        #region IDataRecord Members - Misc

        public int FieldCount {
            [OleDbPermission(SecurityAction.Demand)]
            get {
                DisposedCheck();
                return _recordset.Fields.Count;
            }
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) {
            throw new NotSupportedException();
        }

        public char GetChar(int i) {
            throw new NotSupportedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) {
            throw new NotSupportedException();
        }

        public IDataReader GetData(int i) {
            throw new NotSupportedException();
        }

        public string GetDataTypeName(int i) {
            throw new NotSupportedException();
        }

        [OleDbPermission(SecurityAction.Demand)]
        public Type GetFieldType(int i) {
            DisposedCheck();
            return _recordset.Fields[i].Value.GetType();
        }

        [OleDbPermission(SecurityAction.Demand)]
        public string GetName(int i) {
            DisposedCheck();
            return _recordset.Fields[i].Name;
        }

        public int GetOrdinal(string name) {
            throw new NotSupportedException();
        }

        public object GetValue(int i) {
            return this[i];
        }

        public int GetValues(object[] values) {

            DisposedCheck();
            values = new object[FieldCount];
            for(int i = 0; i < values.Length; i++) {
                values[i] = this[i];
            }
            return values.Length;
        }

        public bool IsDBNull(int i) {
            object value = this[i];
            return value == null || value is DBNull || (value is INullable && ((INullable) value).IsNull);
        }

        public object this[string name] {
            [OleDbPermission(SecurityAction.Demand)]
            get {
                DisposedCheck();
                return _recordset.Fields[name].Value;
            }
        }

        public object this[int i] {
            [OleDbPermission(SecurityAction.Demand)]
            get {
                DisposedCheck();
                return _recordset.Fields[i].Value;
            }
        }

        #endregion
    }
}
