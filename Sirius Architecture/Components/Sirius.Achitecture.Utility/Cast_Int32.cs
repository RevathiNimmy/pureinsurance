using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Sirius.Architecture.Utility {

    // All members of the class that deal with the Int32 logical data type.
    partial class Cast {

        #region Public Shared Methods - Int32

        /// <overloads>
        /// Convert a 32-bit integer value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32 ToInt32(Boolean value, Int32 defaultValue) {
            return DefaultIfNull(ToInt32(value), defaultValue);
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32 ToInt32(Boolean? value, Int32 defaultValue) {
            return DefaultIfNull(ToInt32(value), defaultValue);
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32 ToInt32(SqlBoolean value, Int32 defaultValue) {
            return DefaultIfNull(ToInt32(value), defaultValue);
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32 ToInt32(Byte value, Int32 defaultValue) {
            return DefaultIfNull(ToInt32(value), defaultValue);
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32 ToInt32(Byte? value, Int32 defaultValue) {
            return DefaultIfNull(ToInt32(value), defaultValue);
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32 ToInt32(SqlByte value, Int32 defaultValue) {
            return DefaultIfNull(ToInt32(value), defaultValue);
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32 ToInt32(Int16 value, Int32 defaultValue) {
            return DefaultIfNull(ToInt32(value), defaultValue);
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32 ToInt32(Int16? value, Int32 defaultValue) {
            return DefaultIfNull(ToInt32(value), defaultValue);
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32 ToInt32(SqlInt16 value, Int32 defaultValue) {
            return DefaultIfNull(ToInt32(value), defaultValue);
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32 ToInt32(Int32 value, Int32 defaultValue) {
            return DefaultIfNull(ToInt32(value), defaultValue);
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32 ToInt32(Int32? value, Int32 defaultValue) {
            return DefaultIfNull(ToInt32(value), defaultValue);
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32 ToInt32(SqlInt32 value, Int32 defaultValue) {
            return DefaultIfNull(ToInt32(value), defaultValue);
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32 ToInt32(Int64 value, Int32 defaultValue) {
            return DefaultIfNull(ToInt32(value), defaultValue);
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32 ToInt32(Int64? value, Int32 defaultValue) {
            return DefaultIfNull(ToInt32(value), defaultValue);
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32 ToInt32(SqlInt64 value, Int32 defaultValue) {
            return DefaultIfNull(ToInt32(value), defaultValue);
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32 ToInt32(SqlParameter value, Int32 defaultValue) {
            return DefaultIfNull(ToInt32(value), defaultValue);
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32 ToInt32(Object value, Int32 defaultValue) {
            return DefaultIfNull(ToInt32(value), defaultValue);
        }

        #endregion

        #region Public Shared Methods - Nullable(Of Int32)

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32? ToInt32(Boolean value) {
            return ToInt32(BooleanDataConvert.ToInt32(value));
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32? ToInt32(Boolean? value) {
            return ToInt32(BooleanDataConvert.ToInt32(value));
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32? ToInt32(SqlBoolean value) {
            return ToInt32(BooleanDataConvert.ToInt32(value));
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32? ToInt32(Byte value) {
            return value;
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32? ToInt32(Byte? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32? ToInt32(SqlByte value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32? ToInt32(Int16 value) {
            return value;
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32? ToInt32(Int16? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32? ToInt32(SqlInt16 value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32? ToInt32(Int32 value) {
            return value;
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32? ToInt32(Int32? value) {
            return value;
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32? ToInt32(SqlInt32 value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32? ToInt32(Int64 value) {
            return (Int32) value;
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32? ToInt32(Int64? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return (Int32) value.Value;
            }
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32? ToInt32(SqlInt64 value) {
            if(value.IsNull) {
                return null;
            } else {
                return (Int32) value.Value;
            }
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32? ToInt32(SqlParameter value) {
            return ToInt32(value.Value);
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Int32? ToInt32(Object value) {
            if(value == null || value is DBNull) {
                return null;
                // ---------- Boolean ----------
            } else if(value is Boolean) {
                return ToInt32((Boolean) value);
            } else if(value is Boolean?) {
                return ToInt32((Boolean?) value);
            } else if(value is SqlBoolean) {
                return ToInt32((SqlBoolean) value);
                // ---------- Byte ----------
            } else if(value is Byte) {
                return ToInt32((Byte) value);
            } else if(value is Byte?) {
                return ToInt32((Byte?) value);
            } else if(value is SqlByte) {
                return ToInt32((SqlByte) value);
                // ---------- Int16 ----------
            } else if(value is Int16) {
                return ToInt32((Int16) value);
            } else if(value is Int16?) {
                return ToInt32((Int16?) value);
            } else if(value is SqlInt16) {
                return ToInt32((SqlInt16) value);
                // ---------- Int32 ----------
            } else if(value is Int32) {
                return ToInt32((Int32) value);
            } else if(value is Int32?) {
                return ToInt32((Int32?) value);
            } else if(value is SqlInt32) {
                return ToInt32((SqlInt32) value);
                // ---------- Int64 ----------
            } else if(value is Int64) {
                return ToInt32((Int64) value);
            } else if(value is Int64?) {
                return ToInt32((Int64?) value);
            } else if(value is SqlInt64) {
                return ToInt32((SqlInt64) value);
                // ---------- Enum ----------
            } else if(value is Enum) {
                return ToInt32(Convert.ToInt32(value));
                // ---------- Parameter ----------
            } else if(value is SqlParameter) {
                return ToInt32((SqlParameter) value);
            } else {
                throw new InvalidCastException(String.Format(Properties.Resources.ErrorInvalidCast, RcwHelper.TypeName(value), typeof(Int32?)));
            }
        }

        #endregion

        #region Public Shared Methods - Object

        /// <overloads>
        /// Convert a 32-bit integer value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Object ToObjInt32(Int32? value) {
            if(!value.HasValue) {
                return DBNull.Value;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a 32-bit integer value from one representation to another.
        /// </summary>
        public static Object ToObjInt32(SqlInt32 value) {
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
        public static Int32 DefaultIfNull(Int32? value) {
            return DefaultIfNull(value, (Int32) 0);
        }

        /// <summary>
        /// Convert a <c>NULL</c> value to the default value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return this value instead of null.</param>
        /// <returns>The new value.</returns>
        public static Int32 DefaultIfNull(Int32? value, Int32 defaultValue) {
            return value.GetValueOrDefault(defaultValue);
        }

        #endregion

        #region Public Shared Methods - NullIfDefault

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>The new value.</returns>
        public static Int32? NullIfDefault(Int32 value) {
            return NullIfDefault(value, (Int32) 0);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static Int32? NullIfDefault(Int32 value, Int32 defaultValue) {
            if(value == defaultValue) {
                return null;
            } else {
                return value;
            }
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>The new value.</returns>
        public static Int32? NullIfDefault(Int32? value) {
            return NullIfDefault(value, (Int32) 0);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static Int32? NullIfDefault(Int32? value, Int32 defaultValue) {
            if(value.GetValueOrDefault(defaultValue) == defaultValue) {
                return null;
            } else {
                return value;
            }
        }

        #endregion

    }

}
