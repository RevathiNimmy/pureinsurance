using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Sirius.Architecture.Utility {

    // All members of the class that deal with the String logical data type.
    partial class Cast {

        #region Public Shared Methods - String

        /// <overloads>
        /// Convert a String value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a String value from one representation to another.
        /// </summary>
        public static String ToString(String value, String defaultValue) {
            return DefaultIfNull(ToString(value), defaultValue);
        }

        /// <summary>
        /// Convert a String value from one representation to another.
        /// </summary>
        public static String ToString(SqlString value, String defaultValue) {
            return DefaultIfNull(ToString(value), defaultValue);
        }

        /// <summary>
        /// Convert a String value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support writing values to ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static String ToString(Guid value, String defaultValue) {
            return DefaultIfNull(ToString(value), defaultValue);
        }

        /// <summary>
        /// Convert a String value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support writing values to ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static String ToString(Guid? value, String defaultValue) {
            return DefaultIfNull(ToString(value), defaultValue);
        }

        /// <summary>
        /// Convert a String value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support writing values to ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static String ToString(SqlGuid value, String defaultValue) {
            return DefaultIfNull(ToString(value), defaultValue);
        }

        /// <summary>
        /// Convert a String value from one representation to another.
        /// </summary>
        public static String ToString(SqlParameter value, String defaultValue) {
            return DefaultIfNull(ToString(value), defaultValue);
        }

        /// <summary>
        /// Convert a String value from one representation to another.
        /// </summary>
        public static String ToString(Object value, String defaultValue) {
            return DefaultIfNull(ToString(value), defaultValue);
        }

        #endregion

        #region Public Shared Methods - String

        /// <summary>
        /// Convert a String value from one representation to another.
        /// </summary>
        public static String ToString(String value) {
            return value;
        }

        /// <summary>
        /// Convert a String value from one representation to another.
        /// </summary>
        public static String ToString(SqlString value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a String value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support writing values to ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static String ToString(Guid value) {
            return value.ToString("B").ToUpperInvariant();
        }

        /// <summary>
        /// Convert a String value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support writing values to ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static String ToString(Guid? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return value.Value.ToString("B").ToUpperInvariant();
            }
        }

        /// <summary>
        /// Convert a String value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support writing values to ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static String ToString(SqlGuid value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value.ToString("B").ToUpperInvariant();
            }
        }

        /// <summary>
        /// Convert a String value from one representation to another.
        /// </summary>
        public static String ToString(SqlParameter value) {
            return ToString(value.Value);
        }

        /// <summary>
        /// Convert a String value from one representation to another.
        /// </summary>
        public static String ToString(Object value) {
            if(value == null || value is DBNull) {
                return null;
                // ---------- String ----------
            } else if(value is String) {
                return ToString((String) value);
            } else if(value is SqlString) {
                return ToString((SqlString) value);
                // ---------- Guid ----------
            } else if(value is Guid) {
                return ToString((Guid) value);
            } else if(value is Guid?) {
                return ToString((Guid?) value);
            } else if(value is SqlGuid) {
                return ToString((SqlGuid) value);
                // ---------- Parameter ----------
            } else if(value is SqlParameter) {
                return ToString((SqlParameter) value);
            } else {
                throw new InvalidCastException(String.Format(Properties.Resources.ErrorInvalidCast, RcwHelper.TypeName(value), typeof(String)));
            }
        }

        #endregion

        #region Public Shared Methods - Object

        /// <overloads>
        /// Convert a String value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a String value from one representation to another.
        /// </summary>
        public static Object ToObjString(String value) {
            if(value == null) {
                return DBNull.Value;
            } else {
                return value;
            }
        }

        /// <summary>
        /// Convert a String value from one representation to another.
        /// </summary>
        public static Object ToObjString(SqlString value) {
            if(value.IsNull) {
                return DBNull.Value;
            } else {
                return value.Value;
            }
        }

        #endregion

        #region Public Shared Methods - DefaultIfNull

        /// <summary>
        /// Convert a <c>NULL</c> value to the default value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>The new value.</returns>
        public static String DefaultIfNull(String value) {
            return DefaultIfNull(value, String.Empty);
        }

        /// <summary>
        /// Convert a <c>NULL</c> value to the default value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return this value instead of null.</param>
        /// <returns>The new value.</returns>
        public static String DefaultIfNull(String value, String defaultValue) {
            if(value == null) {
                return defaultValue;
            } else {
                return value;
            }
        }

        #endregion

        #region Public Shared Methods - NullIfDefault

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>The new value.</returns>
        public static String NullIfDefault(String value) {
            return NullIfDefault(value, String.Empty);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static String NullIfDefault(String value, String defaultValue) {
            if(Equals(value, defaultValue)) {
                return null;
            } else {
                return value;
            }
        }

        #endregion

        #region Public Shared Methods - Equals

        /// <summary>
        /// Value equality function, with optimisation for the most common cases.
        /// </summary>
        /// <param name="value1">Value to compare.</param>
        /// <param name="value2">Value to compare.</param>
        /// <returns>True if the values are equal, otherwise false.</returns>
        public static Boolean Equals(String value1, String value2) {

            if(value1 == value2) {
                return true;
            } else if(value1 == null && value2 == null) {
                return true;
            } else if(value1 == null || value2 == null) {
                return false;
            } else if(value1.Length != value2.Length) {
                return false;
            } else if(value1.Length == 0) {
                return true;
            }
            return value1 == value2;

        }

        #endregion

    }

}
