namespace Sirius.Architecture.Utility {

    // TODO: Move the EnumFields.BindingInfo class out of its parent.

    partial class EnumFields {

        /// <summary>
        /// Data binding information for one possible value in a dropdown control.
        /// </summary>
        /// <remarks>
        /// This class has been decoupled from the enum utility methods, because it is
        /// a very useful generic model class to bind all sorts of data to the UI.
        /// </remarks>
        public sealed class BindingInfo {

            private readonly string _description;
            private readonly object _value;

            /// <summary>
            /// Human-readable description of the value.
            /// </summary>
            public string Description { get { return _description; } }
            /// <summary>
            /// The value.
            /// </summary>
            public object Value { get { return _value; } }

            /// <summary>
            /// Create a new instance of this class with the specified data.
            /// </summary>
            public BindingInfo(string description, object value) {
                _description = description;
                _value = value;
            }

            /// <summary>
            /// A text representation of this object.
            /// </summary>
            public override string ToString() {
                return string.Format("{0} = {1}", Description, Value);
            }
        }
    }
}
