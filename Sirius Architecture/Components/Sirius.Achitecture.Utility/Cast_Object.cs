using System;

namespace Sirius.Architecture.Utility {

    // All members of the class that deal with the Object logical data type.
    partial class Cast {

        #region Public Shared Methods - DefaultIfNull

        /// <overloads>
        /// Convert a <c>NULL</c> value to the default value, leaving any other value untouched.
        /// </overloads>
        /// <summary>
        /// Convert a <c>NULL</c> value to the default value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return this value instead of null.</param>
        /// <returns>The new value.</returns>
        public static Object DefaultIfNull(Object value, Object defaultValue) {
            if(value == null || value is DBNull) {
                return defaultValue;
            } else {
                return value;
            }
        }

        #endregion

        #region Public Shared Methods - NullIfDefault

        /// <overloads>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </overloads>
        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        /// <remarks>
        /// NOTE - This overload will only work properly if the types you pass in override the <see cref="Object.Equals(object, object)"/> method correctly.
        /// </remarks>
        public static Object NullIfDefault(Object value, Object defaultValue) {
            if(Object.Equals(value, defaultValue)) {
                return null;
            } else {
                return value;
            }
        }

        #endregion

    }

}
