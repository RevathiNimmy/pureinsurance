using System;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// This class returns different separator strings on the first and subsequent invocations.
    /// It is useful for building separated lists.
    /// </summary>
    [Serializable]
    public class Separator : ICloneable {

        #region Private Fields

        private string _text1;
        private string _text2;
        private bool _firstTime;

        #endregion

        #region Constructors

        /// <overloads>
        /// Construct a new instance of this class.
        /// </overloads>
        /// <summary>
        /// Construct a new instance of this class with a blank first separator (the most common case).
        /// </summary>
        /// <param name="text2">Text to return on all but the first invocation.</param>
        public Separator(string text2) {
            _text1 = string.Empty;
            _text2 = text2;
            _firstTime = true;
        }

        /// <summary>
        /// Construct a new instance of this class with both separators defined.
        /// </summary>
        /// <param name="text1">Text to return only on the first invocation.</param>
        /// <param name="text2">Text to return on all but the first invocation.</param>
        public Separator(string text1, string text2) {
            _text1 = text1;
            _text2 = text2;
            _firstTime = true;
        }

        // For private use only.
        private Separator() {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Return the first separator when first called, and the second separator on all subsequent calls.
        /// </summary>
        /// <returns>The appropriate separator.</returns>
        public override string ToString() {
            if(_firstTime) {
                _firstTime = false;
                return _text1;
            } else {
                return _text2;
            }
        }

        /// <summary>
        /// Return the first separator when first called, and the second separator on all subsequent calls.
        /// </summary>
        /// <param name="value">The object to convert to a string.</param>
        /// <returns>The appropriate separator.</returns>
        public static implicit operator string(Separator value) {
            return value.ToString();
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
        public Separator Clone() {
            Separator copy = new Separator();
            copy._text1 = this._text1;
            copy._text2 = this._text2;
            copy._firstTime = this._firstTime;
            return copy;
        }

        #endregion
    }
}
