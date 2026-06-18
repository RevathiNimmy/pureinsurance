using System;
using System.Runtime.Serialization;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// This exception indicates that the code wants to abort processing with no further user interaction.
    /// Error dialogs should do nothing when this exception type is encountered.
    /// </summary>
    /// <remarks>
    /// This exception must ONLY be used in interface code.
    /// </remarks>
    [Serializable]
    public class AbortException : Exception {

        #region Private Fields

        private bool _cancel;

        #endregion

        #region Constructors

        /// <overloads>
        /// Construct a new instance of this class.
        /// </overloads>
        /// <summary>
        /// Construct a new instance of this class with default properties.
        /// This indicates that the user has chosen to cancel processing.
        /// </summary>
        public AbortException()
            : base(Properties.Resources.AbortExceptionMessageCancelled) {

            _cancel = true;
        }

        /// <summary>
        /// Construct a new instance of this class with a reference to the inner exception that is the cause of this exception.
        /// This indicates that processing has aborted with a fatal error.
        /// </summary>
        /// <param name="innerException">The cause of the current exception.</param>
        public AbortException(Exception innerException)
            : base(Properties.Resources.AbortExceptionMessageNotCancelled, innerException) {

            _cancel = false;
        }

        /// <summary>
        /// Construct a new instance of this class with default properties.
        /// This indicates that the user has chosen to cancel processing.
        /// </summary>
        /// <param name="message">Description of the exception.</param>
        public AbortException(string message)
            : base(message) {

            _cancel = true;
        }

        /// <summary>
        /// Construct a new instance of this class with a reference to the inner exception that is the cause of this exception.
        /// This indicates that processing has aborted with a fatal error.
        /// </summary>
        /// <param name="message">Description of the exception.</param>
        /// <param name="innerException">The cause of the current exception.</param>
        public AbortException(string message, Exception innerException)
            : base(message, innerException) {

            _cancel = false;
        }
        
        /// <summary>
        /// Initialise a new instance of the class with serialized data.
        /// </summary>
        /// <param name="info">Serialized object data</param>
        /// <param name="context">Contextual information about the source or destination</param>
        protected AbortException(SerializationInfo info, StreamingContext context)
            : base(info, context) {

            if(info == null) {
                throw new ArgumentNullException("info");
            }
            _cancel = info.GetBoolean("_cancel");
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
            info.AddValue("_cancel", _cancel, typeof(bool));
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// If true, the user has chosen to cancel processing. If false, there was a fatal error.
        /// </summary>
        public bool Cancel {
            get {
                return _cancel;
            }
        }

        #endregion
    }
}
