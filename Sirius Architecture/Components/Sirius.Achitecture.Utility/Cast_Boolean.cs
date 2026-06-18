using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Sirius.Architecture.Utility {

    // All members of the class that deal with the Boolean logical data type.
    partial class Cast {

        #region Public Shared Methods - Boolean

        /// <overloads>
        /// Convert a boolean value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean ToBoolean(Boolean value, Boolean defaultValue) {
            return DefaultIfNull(ToBoolean(value), defaultValue);
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean ToBoolean(Boolean? value, Boolean defaultValue) {
            return DefaultIfNull(ToBoolean(value), defaultValue);
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean ToBoolean(SqlBoolean value, Boolean defaultValue) {
            return DefaultIfNull(ToBoolean(value), defaultValue);
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean ToBoolean(Byte value, Boolean defaultValue) {
            return DefaultIfNull(ToBoolean(value), defaultValue);
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean ToBoolean(Byte? value, Boolean defaultValue) {
            return DefaultIfNull(ToBoolean(value), defaultValue);
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean ToBoolean(SqlByte value, Boolean defaultValue) {
            return DefaultIfNull(ToBoolean(value), defaultValue);
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean ToBoolean(Int16 value, Boolean defaultValue) {
            return DefaultIfNull(ToBoolean(value), defaultValue);
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean ToBoolean(Int16? value, Boolean defaultValue) {
            return DefaultIfNull(ToBoolean(value), defaultValue);
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean ToBoolean(SqlInt16 value, Boolean defaultValue) {
            return DefaultIfNull(ToBoolean(value), defaultValue);
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean ToBoolean(Int32 value, Boolean defaultValue) {
            return DefaultIfNull(ToBoolean(value), defaultValue);
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean ToBoolean(Int32? value, Boolean defaultValue) {
            return DefaultIfNull(ToBoolean(value), defaultValue);
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean ToBoolean(SqlInt32 value, Boolean defaultValue) {
            return DefaultIfNull(ToBoolean(value), defaultValue);
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean ToBoolean(Int64 value, Boolean defaultValue) {
            return DefaultIfNull(ToBoolean(value), defaultValue);
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean ToBoolean(Int64? value, Boolean defaultValue) {
            return DefaultIfNull(ToBoolean(value), defaultValue);
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean ToBoolean(SqlInt64 value, Boolean defaultValue) {
            return DefaultIfNull(ToBoolean(value), defaultValue);
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean ToBoolean(SqlParameter value, Boolean defaultValue) {
            return DefaultIfNull(ToBoolean(value), defaultValue);
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean ToBoolean(Object value, Boolean defaultValue) {
            return DefaultIfNull(ToBoolean(value), defaultValue);
        }

        #endregion

        #region Public Shared Methods - Nullable(Of Boolean)

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean? ToBoolean(Boolean value) {
            return value;
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean? ToBoolean(Boolean? value) {
            return value;
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean? ToBoolean(SqlBoolean value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean? ToBoolean(Byte value) {
            return ToBoolean(BooleanDataConvert.ToBoolean(value));
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean? ToBoolean(Byte? value) {
            return ToBoolean(BooleanDataConvert.ToBoolean(value));
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean? ToBoolean(SqlByte value) {
            return ToBoolean(BooleanDataConvert.ToBoolean(value));
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean? ToBoolean(Int16 value) {
            return ToBoolean(BooleanDataConvert.ToBoolean(value));
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean? ToBoolean(Int16? value) {
            return ToBoolean(BooleanDataConvert.ToBoolean(value));
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean? ToBoolean(SqlInt16 value) {
            return ToBoolean(BooleanDataConvert.ToBoolean(value));
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean? ToBoolean(Int32 value) {
            return ToBoolean(BooleanDataConvert.ToBoolean(value));
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean? ToBoolean(Int32? value) {
            return ToBoolean(BooleanDataConvert.ToBoolean(value));
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean? ToBoolean(SqlInt32 value) {
            return ToBoolean(BooleanDataConvert.ToBoolean(value));
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean? ToBoolean(Int64 value) {
            return ToBoolean(BooleanDataConvert.ToBoolean(value));
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean? ToBoolean(Int64? value) {
            return ToBoolean(BooleanDataConvert.ToBoolean(value));
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean? ToBoolean(SqlInt64 value) {
            return ToBoolean(BooleanDataConvert.ToBoolean(value));
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean? ToBoolean(SqlParameter value) {
            return ToBoolean(value.Value);
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Boolean? ToBoolean(Object value) {
            if(value == null || value is DBNull) {
                return null;
                // ---------- Boolean ----------
            } else if(value is Boolean) {
                return ToBoolean((Boolean) value);
            } else if(value is Boolean?) {
                return ToBoolean((Boolean?) value);
            } else if(value is SqlBoolean) {
                return ToBoolean((SqlBoolean) value);
                // ---------- Byte ----------
            } else if(value is Byte) {
                return ToBoolean((Byte) value);
            } else if(value is Byte?) {
                return ToBoolean((Byte?) value);
            } else if(value is SqlByte) {
                return ToBoolean((SqlByte) value);
                // ---------- Int16 ----------
            } else if(value is Int16) {
                return ToBoolean((Int16) value);
            } else if(value is Int16?) {
                return ToBoolean((Int16?) value);
            } else if(value is SqlInt16) {
                return ToBoolean((SqlInt16) value);
                // ---------- Int32 ----------
            } else if(value is Int32) {
                return ToBoolean((Int32) value);
            } else if(value is Int32?) {
                return ToBoolean((Int32?) value);
            } else if(value is SqlInt32) {
                return ToBoolean((SqlInt32) value);
                // ---------- Int64 ----------
            } else if(value is Int64) {
                return ToBoolean((Int64) value);
            } else if(value is Int64?) {
                return ToBoolean((Int64?) value);
            } else if(value is SqlInt64) {
                return ToBoolean((SqlInt64) value);
                // ---------- Parameter ----------
            } else if(value is SqlParameter) {
                return ToBoolean((SqlParameter) value);
            } else {
                throw new InvalidCastException(String.Format(Properties.Resources.ErrorInvalidCast, RcwHelper.TypeName(value), typeof(Boolean?)));
            }
        }

        #endregion

        #region Public Shared Methods - Object

        /// <overloads>
        /// Convert a boolean value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Object ToObjBoolean(Boolean? value) {
            if(!value.HasValue) {
                return DBNull.Value;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a boolean value from one representation to another.
        /// </summary>
        public static Object ToObjBoolean(SqlBoolean value) {
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
        public static Boolean DefaultIfNull(Boolean? value) {
            return DefaultIfNull(value, false);
        }

        /// <summary>
        /// Convert a <c>NULL</c> value to the default value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return this value instead of null.</param>
        /// <returns>The new value.</returns>
        public static Boolean DefaultIfNull(Boolean? value, Boolean defaultValue) {
            return value.GetValueOrDefault(defaultValue);
        }

        #endregion

        #region Public Shared Methods - NullIfDefault

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>The new value.</returns>
        public static Boolean? NullIfDefault(Boolean value) {
            return NullIfDefault(value, false);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static Boolean? NullIfDefault(Boolean value, Boolean defaultValue) {
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
        public static Boolean? NullIfDefault(Boolean? value) {
            return NullIfDefault(value, false);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static Boolean? NullIfDefault(Boolean? value, Boolean defaultValue) {
            if(value.GetValueOrDefault(defaultValue) == defaultValue) {
                return null;
            } else {
                return value;
            }
        }

        #endregion

    }

}
