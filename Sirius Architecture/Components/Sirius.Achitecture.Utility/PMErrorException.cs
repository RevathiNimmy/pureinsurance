using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// The exception that is thrown when a Sirius COM method call returns a failure result.
    /// DO NOT USE THIS EXCEPTION FOR ANY OTHER PURPOSE!
    /// </summary>
    [Serializable]
    public class PMErrorException : Exception {

        #region Private Fields

        private string _className;
        private string _methodName;
        private int _returnValue;

        #endregion

        #region Constructors

        /// <overloads>
        /// Construct a new instance of this class.
        /// </overloads>
        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="className">The COM class programmatic ID.</param>
        /// <param name="methodName">The COM method name.</param>
        /// <param name="returnValue">The COM method return value.</param>
        public PMErrorException(string className, string methodName, int returnValue)
            : base(string.Format(Properties.Resources.PMErrorExceptionMessage, className, methodName, returnValue)) {

            _className = className;
            _methodName = methodName;
            _returnValue = returnValue;
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="className">The COM class programmatic ID.</param>
        /// <param name="methodName">The COM method name.</param>
        /// <param name="returnValue">The COM method return value.</param>
        /// <param name="innerException">The cause of the current exception.</param>
        public PMErrorException(string className, string methodName, int returnValue, Exception innerException)
            : base(string.Format(Properties.Resources.PMErrorExceptionMessage, className, methodName, returnValue), innerException) {

            _className = className;
            _methodName = methodName;
            _returnValue = returnValue;
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="comObject">The COM object being called.</param>
        /// <param name="methodName">The COM method name.</param>
        /// <param name="returnValue">The COM method return value.</param>
        public PMErrorException(object comObject, string methodName, int returnValue)
            : this(RcwHelper.TypeName(comObject), methodName, returnValue) {
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="comObject">The COM object being called.</param>
        /// <param name="methodName">The COM method name.</param>
        /// <param name="returnValue">The COM method return value.</param>
        /// <param name="innerException">The cause of the current exception.</param>
        public PMErrorException(object comObject, string methodName, int returnValue, Exception innerException)
            : this(RcwHelper.TypeName(comObject), methodName, returnValue, innerException) {
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="methodInfo">The COM method being called.</param>
        /// <param name="returnValue">The COM method return value.</param>
        public PMErrorException(MethodInfo methodInfo, int returnValue)
            : this(methodInfo.DeclaringType.GetType().ToString(), methodInfo.Name, returnValue) {
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="methodInfo">The COM method being called.</param>
        /// <param name="returnValue">The COM method return value.</param>
        /// <param name="innerException">The cause of the current exception.</param>
        public PMErrorException(MethodInfo methodInfo, int returnValue, Exception innerException)
            : this(methodInfo.DeclaringType.GetType().ToString(), methodInfo.Name, returnValue, innerException) {
        }

        /// <summary>
        /// Construct a new exception from old-style STS SiriusBackOffice error data.
        /// </summary>
        /// <param name="description">Unstructured description of the method call.</param>
        /// <param name="returnValue">The COM method return value.</param>
        /// <remarks>
        /// THIS CONSTRUCTOR IS RESERVED FOR INTERNAL USE, AND MUST NOT BE CALLED IN NORMAL CODE.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public PMErrorException(string description, int returnValue)
            : base(description) {

            _className = string.Empty;
            _methodName = string.Empty;
            _returnValue = returnValue;
        }

        /// <summary>
        /// Construct a new exception from old-style STS SiriusBackOffice error data.
        /// </summary>
        /// <param name="description">Unstructured description of the method call.</param>
        /// <param name="returnValue">The COM method return value.</param>
        /// <param name="innerException">The cause of the current exception.</param>
        /// <remarks>
        /// THIS CONSTRUCTOR IS RESERVED FOR INTERNAL USE, AND MUST NOT BE CALLED IN NORMAL CODE.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public PMErrorException(string description, int returnValue, Exception innerException)
            : base(description, innerException) {

            _className = string.Empty;
            _methodName = string.Empty;
            _returnValue = returnValue;
        }

        /// <summary>
        /// Construct a new exception from old-style STS SiriusBackOffice error data.
        /// </summary>
        /// <param name="description">Unstructured description of the method call.</param>
        /// <remarks>
        /// THIS CONSTRUCTOR IS RESERVED FOR INTERNAL USE, AND MUST NOT BE CALLED IN NORMAL CODE.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public PMErrorException(string description)
            : base(description) {

            _className = string.Empty;
            _methodName = string.Empty;
            _returnValue = 0;
        }

        /// <summary>
        /// Construct a new exception from old-style STS SiriusBackOffice error data.
        /// </summary>
        /// <param name="description">Unstructured description of the method call.</param>
        /// <param name="innerException">The cause of the current exception.</param>
        /// <remarks>
        /// THIS CONSTRUCTOR IS RESERVED FOR INTERNAL USE, AND MUST NOT BE CALLED IN NORMAL CODE.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public PMErrorException(string description, Exception innerException)
            : base(description, innerException) {

            _className = string.Empty;
            _methodName = string.Empty;
            _returnValue = 0;
        }

        /// <summary>
        /// This inherited constructor is not valid for this class.
        /// </summary>
        /// <remarks>
        /// THIS CONSTRUCTOR IS RESERVED FOR INTERNAL USE, AND MUST NOT BE CALLED IN NORMAL CODE.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public PMErrorException()
            : base() {

            _className = string.Empty;
            _methodName = string.Empty;
            _returnValue = 0;
        }
        
        /// <summary>
        /// Initialise a new instance of the class with serialized data.
        /// </summary>
        /// <param name="info">Serialized object data</param>
        /// <param name="context">Contextual information about the source or destination</param>
        protected PMErrorException(SerializationInfo info, StreamingContext context)
            : base(info, context) {

            if(info == null) {
                throw new ArgumentNullException("info");
            }
            _className = info.GetString("_className");
            _methodName = info.GetString("_methodName");
            _returnValue = info.GetInt32("_returnValue");
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
            info.AddValue("_className", _className, typeof(string));
            info.AddValue("_methodName", _methodName, typeof(string));
            info.AddValue("_returnValue", _returnValue, typeof(int));
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The COM class programmatic ID.
        /// </summary>
        public string ClassName {
            get {
                return _className;
            }
        }

        /// <summary>
        /// The COM method name.
        /// </summary>
        public string MethodName {
            get {
                return _methodName;
            }
        }

        /// <summary>
        /// The COM method return value.
        /// </summary>
        public int ReturnValue {
            get {
                return _returnValue;
            }
        }

        #endregion
    }
}
