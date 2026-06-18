using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Sirius.Architecture.Utility {

    // All members of the class that deal with the Int16 logical data type.
    partial class Cast {

        #region Public Shared Methods - Int16

        /// <overloads>
        /// Convert a 16-bit integer value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16 ToInt16(Boolean value, Int16 defaultValue) {
            return DefaultIfNull(ToInt16(value), defaultValue);
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16 ToInt16(Boolean? value, Int16 defaultValue) {
            return DefaultIfNull(ToInt16(value), defaultValue);
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16 ToInt16(SqlBoolean value, Int16 defaultValue) {
            return DefaultIfNull(ToInt16(value), defaultValue);
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16 ToInt16(Byte value, Int16 defaultValue) {
            return DefaultIfNull(ToInt16(value), defaultValue);
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16 ToInt16(Byte? value, Int16 defaultValue) {
            return DefaultIfNull(ToInt16(value), defaultValue);
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16 ToInt16(SqlByte value, Int16 defaultValue) {
            return DefaultIfNull(ToInt16(value), defaultValue);
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16 ToInt16(Int16 value, Int16 defaultValue) {
            return DefaultIfNull(ToInt16(value), defaultValue);
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16 ToInt16(Int16? value, Int16 defaultValue) {
            return DefaultIfNull(ToInt16(value), defaultValue);
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16 ToInt16(SqlInt16 value, Int16 defaultValue) {
            return DefaultIfNull(ToInt16(value), defaultValue);
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16 ToInt16(Int32 value, Int16 defaultValue) {
            return DefaultIfNull(ToInt16(value), defaultValue);
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16 ToInt16(Int32? value, Int16 defaultValue) {
            return DefaultIfNull(ToInt16(value), defaultValue);
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16 ToInt16(SqlInt32 value, Int16 defaultValue) {
            return DefaultIfNull(ToInt16(value), defaultValue);
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16 ToInt16(Int64 value, Int16 defaultValue) {
            return DefaultIfNull(ToInt16(value), defaultValue);
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16 ToInt16(Int64? value, Int16 defaultValue) {
            return DefaultIfNull(ToInt16(value), defaultValue);
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16 ToInt16(SqlInt64 value, Int16 defaultValue) {
            return DefaultIfNull(ToInt16(value), defaultValue);
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16 ToInt16(SqlParameter value, Int16 defaultValue) {
            return DefaultIfNull(ToInt16(value), defaultValue);
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16 ToInt16(Object value, Int16 defaultValue) {
            return DefaultIfNull(ToInt16(value), defaultValue);
        }

        #endregion

        #region Public Shared Methods - Nullable(Of Int16)

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16? ToInt16(Boolean value) {
            return ToInt16(BooleanDataConvert.ToInt16(value));
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16? ToInt16(Boolean? value) {
            return ToInt16(BooleanDataConvert.ToInt16(value));
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16? ToInt16(SqlBoolean value) {
            return ToInt16(BooleanDataConvert.ToInt16(value));
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16? ToInt16(Byte value) {
            return value;
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16? ToInt16(Byte? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16? ToInt16(SqlByte value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16? ToInt16(Int16 value) {
            return value;
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16? ToInt16(Int16? value) {
            return value;
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16? ToInt16(SqlInt16 value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16? ToInt16(Int32 value) {
            return (Int16) value;
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16? ToInt16(Int32? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return (Int16) value.Value;
            }
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16? ToInt16(SqlInt32 value) {
            if(value.IsNull) {
                return null;
            } else {
                return (Int16) value.Value;
            }
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16? ToInt16(Int64 value) {
            return (Int16) value;
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16? ToInt16(Int64? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return (Int16) value.Value;
            }
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16? ToInt16(SqlInt64 value) {
            if(value.IsNull) {
                return null;
            } else {
                return (Int16) value.Value;
            }
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16? ToInt16(SqlParameter value) {
            return ToInt16(value.Value);
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Int16? ToInt16(Object value) {
            if(value == null || value is DBNull) {
                return null;
                // ---------- Boolean ----------
            } else if(value is Boolean) {
                return ToInt16((Boolean) value);
            } else if(value is Boolean?) {
                return ToInt16((Boolean?) value);
            } else if(value is SqlBoolean) {
                return ToInt16((SqlBoolean) value);
                // ---------- Byte ----------
            } else if(value is Byte) {
                return ToInt16((Byte) value);
            } else if(value is Byte?) {
                return ToInt16((Byte?) value);
            } else if(value is SqlByte) {
                return ToInt16((SqlByte) value);
                // ---------- Int16 ----------
            } else if(value is Int16) {
                return ToInt16((Int16) value);
            } else if(value is Int16?) {
                return ToInt16((Int16?) value);
            } else if(value is SqlInt16) {
                return ToInt16((SqlInt16) value);
                // ---------- Int32 ----------
            } else if(value is Int32) {
                return ToInt16((Int32) value);
            } else if(value is Int32?) {
                return ToInt16((Int32?) value);
            } else if(value is SqlInt32) {
                return ToInt16((SqlInt32) value);
                // ---------- Int64 ----------
            } else if(value is Int64) {
                return ToInt16((Int64) value);
            } else if(value is Int64?) {
                return ToInt16((Int64?) value);
            } else if(value is SqlInt64) {
                return ToInt16((SqlInt64) value);
                // ---------- Enum ----------
            } else if(value is Enum) {
                return ToInt16(Convert.ToInt16(value));
                // ---------- Parameter ----------
            } else if(value is SqlParameter) {
                return ToInt16((SqlParameter) value);
            } else {
                throw new InvalidCastException(String.Format(Properties.Resources.ErrorInvalidCast, RcwHelper.TypeName(value), typeof(Int16?)));
            }
        }

        #endregion

        #region Public Shared Methods - Object

        /// <overloads>
        /// Convert a 16-bit integer value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Object ToObjInt16(Int16? value) {
            if(!value.HasValue) {
                return DBNull.Value;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a 16-bit integer value from one representation to another.
        /// </summary>
        public static Object ToObjInt16(SqlInt16 value) {
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
        public static Int16 DefaultIfNull(Int16? value) {
            return DefaultIfNull(value, (Int16) 0);
        }

        /// <summary>
        /// Convert a <c>NULL</c> value to the default value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return this value instead of null.</param>
        /// <returns>The new value.</returns>
        public static Int16 DefaultIfNull(Int16? value, Int16 defaultValue) {
            return value.GetValueOrDefault(defaultValue);
        }

        #endregion

        #region Public Shared Methods - NullIfDefault

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>The new value.</returns>
        public static Int16? NullIfDefault(Int16 value) {
            return NullIfDefault(value, (Int16) 0);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static Int16? NullIfDefault(Int16 value, Int16 defaultValue) {
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
        public static Int16? NullIfDefault(Int16? value) {
            return NullIfDefault(value, (Int16) 0);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static Int16? NullIfDefault(Int16? value, Int16 defaultValue) {
            if(value.GetValueOrDefault(defaultValue) == defaultValue) {
                return null;
            } else {
                return value;
            }
        }

        #endregion

    }

}
