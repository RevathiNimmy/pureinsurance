using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Sirius.Architecture.Utility {

    // All members of the class that deal with the Int64 logical data type.
    partial class Cast {

        #region Public Shared Methods - Int64

        /// <overloads>
        /// Convert a 64-bit integer value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64 ToInt64(Boolean value, Int64 defaultValue) {
            return DefaultIfNull(ToInt64(value), defaultValue);
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64 ToInt64(Boolean? value, Int64 defaultValue) {
            return DefaultIfNull(ToInt64(value), defaultValue);
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64 ToInt64(SqlBoolean value, Int64 defaultValue) {
            return DefaultIfNull(ToInt64(value), defaultValue);
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64 ToInt64(Byte value, Int64 defaultValue) {
            return DefaultIfNull(ToInt64(value), defaultValue);
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64 ToInt64(Byte? value, Int64 defaultValue) {
            return DefaultIfNull(ToInt64(value), defaultValue);
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64 ToInt64(SqlByte value, Int64 defaultValue) {
            return DefaultIfNull(ToInt64(value), defaultValue);
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64 ToInt64(Int16 value, Int64 defaultValue) {
            return DefaultIfNull(ToInt64(value), defaultValue);
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64 ToInt64(Int16? value, Int64 defaultValue) {
            return DefaultIfNull(ToInt64(value), defaultValue);
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64 ToInt64(SqlInt16 value, Int64 defaultValue) {
            return DefaultIfNull(ToInt64(value), defaultValue);
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64 ToInt64(Int32 value, Int64 defaultValue) {
            return DefaultIfNull(ToInt64(value), defaultValue);
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64 ToInt64(Int32? value, Int64 defaultValue) {
            return DefaultIfNull(ToInt64(value), defaultValue);
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64 ToInt64(SqlInt32 value, Int64 defaultValue) {
            return DefaultIfNull(ToInt64(value), defaultValue);
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64 ToInt64(Int64 value, Int64 defaultValue) {
            return DefaultIfNull(ToInt64(value), defaultValue);
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64 ToInt64(Int64? value, Int64 defaultValue) {
            return DefaultIfNull(ToInt64(value), defaultValue);
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64 ToInt64(SqlInt64 value, Int64 defaultValue) {
            return DefaultIfNull(ToInt64(value), defaultValue);
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support reading values from ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static Int64 ToInt64(Decimal value, Int64 defaultValue) {
            return DefaultIfNull(ToInt64(value), defaultValue);
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support reading values from ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static Int64 ToInt64(Decimal? value, Int64 defaultValue) {
            return DefaultIfNull(ToInt64(value), defaultValue);
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support reading values from ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static Int64 ToInt64(SqlDecimal value, Int64 defaultValue) {
            return DefaultIfNull(ToInt64(value), defaultValue);
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64 ToInt64(SqlParameter value, Int64 defaultValue) {
            return DefaultIfNull(ToInt64(value), defaultValue);
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64 ToInt64(Object value, Int64 defaultValue) {
            return DefaultIfNull(ToInt64(value), defaultValue);
        }

        #endregion

        #region Public Shared Methods - Nullable(Of Int64)

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64? ToInt64(Boolean value) {
            return ToInt64(BooleanDataConvert.ToInt32(value));
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64? ToInt64(Boolean? value) {
            return ToInt64(BooleanDataConvert.ToInt32(value));
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64? ToInt64(SqlBoolean value) {
            return ToInt64(BooleanDataConvert.ToInt32(value));
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64? ToInt64(Byte value) {
            return value;
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64? ToInt64(Byte? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64? ToInt64(SqlByte value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64? ToInt64(Int16 value) {
            return value;
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64? ToInt64(Int16? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64? ToInt64(SqlInt16 value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64? ToInt64(Int32 value) {
            return value;
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64? ToInt64(Int32? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64? ToInt64(SqlInt32 value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64? ToInt64(Int64 value) {
            return value;
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64? ToInt64(Int64? value) {
            return value;
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64? ToInt64(SqlInt64 value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support reading values from ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static Int64? ToInt64(Decimal value) {
            return (Int64) value;
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support reading values from ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static Int64? ToInt64(Decimal? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return (Int64) value.Value;
            }
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support reading values from ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static Int64? ToInt64(SqlDecimal value) {
            if(value.IsNull) {
                return null;
            } else {
                return (Int64) value.Value;
            }
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64? ToInt64(SqlParameter value) {
            return ToInt64(value.Value);
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Int64? ToInt64(Object value) {
            if(value == null || value is DBNull) {
                return null;
                // ---------- Boolean ----------
            } else if(value is Boolean) {
                return ToInt64((Boolean) value);
            } else if(value is Boolean?) {
                return ToInt64((Boolean?) value);
            } else if(value is SqlBoolean) {
                return ToInt64((SqlBoolean) value);
                // ---------- Byte ----------
            } else if(value is Byte) {
                return ToInt64((Byte) value);
            } else if(value is Byte?) {
                return ToInt64((Byte?) value);
            } else if(value is SqlByte) {
                return ToInt64((SqlByte) value);
                // ---------- Int16 ----------
            } else if(value is Int16) {
                return ToInt64((Int16) value);
            } else if(value is Int16?) {
                return ToInt64((Int16?) value);
            } else if(value is SqlInt16) {
                return ToInt64((SqlInt16) value);
                // ---------- Int32 ----------
            } else if(value is Int32) {
                return ToInt64((Int32) value);
            } else if(value is Int32?) {
                return ToInt64((Int32?) value);
            } else if(value is SqlInt32) {
                return ToInt64((SqlInt32) value);
                // ---------- Int64 ----------
            } else if(value is Int64) {
                return ToInt64((Int64) value);
            } else if(value is Int64?) {
                return ToInt64((Int64?) value);
            } else if(value is SqlInt64) {
                return ToInt64((SqlInt64) value);
                // ---------- Decimal ----------
            } else if(value is Decimal) {
                return ToInt64((Decimal) value);
            } else if(value is Decimal?) {
                return ToInt64((Decimal?) value);
            } else if(value is SqlDecimal) {
                return ToInt64((SqlDecimal) value);
                // ---------- Enum ----------
            } else if(value is Enum) {
                return ToInt64(Convert.ToInt64(value));
                // ---------- Parameter ----------
            } else if(value is SqlParameter) {
                return ToInt64((SqlParameter) value);
            } else {
                throw new InvalidCastException(String.Format(Properties.Resources.ErrorInvalidCast, RcwHelper.TypeName(value), typeof(Int64?)));
            }
        }

        #endregion

        #region Public Shared Methods - Object

        /// <overloads>
        /// Convert a 64-bit integer value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Object ToObjInt64(Int64? value) {
            if(!value.HasValue) {
                return DBNull.Value;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a 64-bit integer value from one representation to another.
        /// </summary>
        public static Object ToObjInt64(SqlInt64 value) {
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
        public static Int64 DefaultIfNull(Int64? value) {
            return DefaultIfNull(value, (Int64) 0);
        }

        /// <summary>
        /// Convert a <c>NULL</c> value to the default value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return this value instead of null.</param>
        /// <returns>The new value.</returns>
        public static Int64 DefaultIfNull(Int64? value, Int64 defaultValue) {
            return value.GetValueOrDefault(defaultValue);
        }

        #endregion

        #region Public Shared Methods - NullIfDefault

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>The new value.</returns>
        public static Int64? NullIfDefault(Int64 value) {
            return NullIfDefault(value, (Int64) 0);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static Int64? NullIfDefault(Int64 value, Int64 defaultValue) {
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
        public static Int64? NullIfDefault(Int64? value) {
            return NullIfDefault(value, (Int64) 0);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static Int64? NullIfDefault(Int64? value, Int64 defaultValue) {
            if(value.GetValueOrDefault(defaultValue) == defaultValue) {
                return null;
            } else {
                return value;
            }
        }

        #endregion

    }

}
