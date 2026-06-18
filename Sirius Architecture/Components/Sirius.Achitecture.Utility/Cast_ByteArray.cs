using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Sirius.Architecture.Utility {

    // All members of the class that deal with the ByteArray logical data type.
    partial class Cast {

        #region Public Shared Methods - Byte()

        /// <overloads>
        /// Convert a Byte array value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a Byte array value from one representation to another.
        /// </summary>
        public static Byte[] ToByteArray(Byte[] value, Byte[] defaultValue) {
            return DefaultIfNull(ToByteArray(value), defaultValue);
        }

        /// <summary>
        /// Convert a Byte array value from one representation to another.
        /// </summary>
        public static Byte[] ToByteArray(SqlBinary value, Byte[] defaultValue) {
            return DefaultIfNull(ToByteArray(value), defaultValue);
        }

        /// <summary>
        /// Convert a Byte array value from one representation to another.
        /// </summary>
        public static Byte[] ToByteArray(SqlParameter value, Byte[] defaultValue) {
            return DefaultIfNull(ToByteArray(value), defaultValue);
        }

        /// <summary>
        /// Convert a Byte array value from one representation to another.
        /// </summary>
        public static Byte[] ToByteArray(Object value, Byte[] defaultValue) {
            return DefaultIfNull(ToByteArray(value), defaultValue);
        }

        #endregion

        #region Public Shared Methods - Byte()

        /// <summary>
        /// Convert a Byte array value from one representation to another.
        /// </summary>
        public static Byte[] ToByteArray(Byte[] value) {
            return value;
        }

        /// <summary>
        /// Convert a Byte array value from one representation to another.
        /// </summary>
        public static Byte[] ToByteArray(SqlBinary value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a Byte array value from one representation to another.
        /// </summary>
        public static Byte[] ToByteArray(SqlParameter value) {
            return ToByteArray(value.Value);
        }

        /// <summary>
        /// Convert a Byte array value from one representation to another.
        /// </summary>
        public static Byte[] ToByteArray(Object value) {
            if(value == null || value is DBNull) {
                return null;
                // ---------- ByteArray ----------
            } else if(value is Byte[]) {
                return ToByteArray((Byte[]) value);
            } else if(value is SqlBinary) {
                return ToByteArray((SqlBinary) value);
                // ---------- Parameter ----------
            } else if(value is SqlParameter) {
                return ToByteArray((SqlParameter) value);
            } else {
                throw new InvalidCastException(String.Format(Properties.Resources.ErrorInvalidCast, RcwHelper.TypeName(value), typeof(Byte[])));
            }
        }

        #endregion

        #region Public Shared Methods - Object

        /// <overloads>
        /// Convert a Byte array value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a Byte array value from one representation to another.
        /// </summary>
        public static Object ToObjByteArray(Byte[] value) {
            if(value == null) {
                return DBNull.Value;
            } else {
                return value;
            }
        }

        /// <summary>
        /// Convert a Byte array value from one representation to another.
        /// </summary>
        public static Object ToObjByteArray(SqlBinary value) {
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
        public static Byte[] DefaultIfNull(Byte[] value) {
            return DefaultIfNull(value, new Byte[] { });
        }

        /// <summary>
        /// Convert a <c>NULL</c> value to the default value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return this value instead of null.</param>
        /// <returns>The new value.</returns>
        public static Byte[] DefaultIfNull(Byte[] value, Byte[] defaultValue) {
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
        public static Byte[] NullIfDefault(Byte[] value) {
            return NullIfDefault(value, new Byte[] { });
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static Byte[] NullIfDefault(Byte[] value, Byte[] defaultValue) {
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
        public static Boolean Equals(Byte[] value1, Byte[] value2) {

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
            for(Int32 i = 0; i <= value1.Length - 1; i++) {
                if(value1[i] != value2[i]) {
                    return false;
                }
            }
            return true;

        }

        #endregion

    }

}
