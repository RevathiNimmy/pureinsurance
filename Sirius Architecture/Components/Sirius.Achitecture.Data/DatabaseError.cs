using System;
using System.ComponentModel;

namespace Sirius.Architecture.Data {

    /// <summary>
    /// Contains information about one specific database error.
    /// </summary>
    [Serializable]
    public sealed class DatabaseError {

        #region Private Fields

        private string _source;
        private string _message;
        private int _number;
        private byte _severity;
        private byte _internalState;
        private string _ansiSqlState;

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new instance of this class with all required information.
        /// This constructor supports <see cref="SiriusConnection"/>, and is not intended to be called directly in your code.
        /// </summary>
        /// <param name="source">Data provider that generated the error.</param>
        /// <param name="message">Error message.</param>
        /// <param name="number">Error number.</param>
        /// <param name="severity">Severity level (0 to 25).</param>
        /// <param name="internalState">Internal numeric state code.</param>
        /// <param name="ansiSqlState">ANSI SQLSTATE value.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public DatabaseError(string source, string message, int number, byte severity, byte internalState, string ansiSqlState) {

            _source = source;
            _message = message;
            _number = number;
            _severity = severity;
            _internalState = internalState;
            _ansiSqlState = ansiSqlState;

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Data provider that generated the error.
        /// </summary>
        public string Source {
            get {
                return _source;
            }
        }

        /// <summary>
        /// Error message.
        /// </summary>
        public string Message {
            get {
                return _message;
            }
        }

        /// <summary>
        /// Error number.
        /// </summary>
        public int Number {
            get {
                return _number;
            }
        }

        /// <summary>
        /// SqlClient connections: Severity level (0 to 25).
        /// PMDAO connections: Not available.
        /// </summary>
        public byte Severity {
            get {
                return _severity;
            }
        }

        /// <summary>
        /// SqlClient connections: Internal numeric state code, used by Microsoft engineers to diagnose issues.
        /// PMDAO connections: Not available.
        /// </summary>
        public byte InternalState {
            get {
                return _internalState;
            }
        }

        /// <summary>
        /// SqlClient connections: Not available.
        /// PMDAO connections: SQLSTATE value as defined by the X/Open and SQL Access Group SQL CAE specification (1992).
        /// </summary>
        public string AnsiSqlState {
            get {
                return _ansiSqlState;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Return all the data for this error in a format suitable for writing to an error log.
        /// </summary>
        public override string ToString() {

            // Hard-code the formatting because it contains only non-localisable bits.
            return string.Format(
                @"Source = ""{0}"", Message = ""{1}"", Number = {2}, Severity = {3}, InternalState = {4}, AnsiSqlState = ""{5}""",
                this.Source,
                this.Message,
                this.Number,
                this.Severity,
                this.InternalState,
                this.AnsiSqlState);

        }

        #endregion

    }

}
