using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Sirius.Architecture.Utility {

    // All members of the class that deal with the Byte logical data type.
    partial class Cast {

        #region Public Shared Methods - Byte

        /// <overloads>
        /// Convert a Byte value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte ToByte(Boolean value, Byte defaultValue) {
            return DefaultIfNull(ToByte(value), defaultValue);
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte ToByte(Boolean? value, Byte defaultValue) {
            return DefaultIfNull(ToByte(value), defaultValue);
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte ToByte(SqlBoolean value, Byte defaultValue) {
            return DefaultIfNull(ToByte(value), defaultValue);
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte ToByte(Byte value, Byte defaultValue) {
            return DefaultIfNull(ToByte(value), defaultValue);
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte ToByte(Byte? value, Byte defaultValue) {
            return DefaultIfNull(ToByte(value), defaultValue);
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte ToByte(SqlByte value, Byte defaultValue) {
            return DefaultIfNull(ToByte(value), defaultValue);
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte ToByte(Int16 value, Byte defaultValue) {
            return DefaultIfNull(ToByte(value), defaultValue);
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte ToByte(Int16? value, Byte defaultValue) {
            return DefaultIfNull(ToByte(value), defaultValue);
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte ToByte(SqlInt16 value, Byte defaultValue) {
            return DefaultIfNull(ToByte(value), defaultValue);
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte ToByte(Int32 value, Byte defaultValue) {
            return DefaultIfNull(ToByte(value), defaultValue);
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte ToByte(Int32? value, Byte defaultValue) {
            return DefaultIfNull(ToByte(value), defaultValue);
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte ToByte(SqlInt32 value, Byte defaultValue) {
            return DefaultIfNull(ToByte(value), defaultValue);
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte ToByte(Int64 value, Byte defaultValue) {
            return DefaultIfNull(ToByte(value), defaultValue);
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte ToByte(Int64? value, Byte defaultValue) {
            return DefaultIfNull(ToByte(value), defaultValue);
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte ToByte(SqlInt64 value, Byte defaultValue) {
            return DefaultIfNull(ToByte(value), defaultValue);
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte ToByte(SqlParameter value, Byte defaultValue) {
            return DefaultIfNull(ToByte(value), defaultValue);
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte ToByte(Object value, Byte defaultValue) {
            return DefaultIfNull(ToByte(value), defaultValue);
        }

        #endregion

        #region Public Shared Methods - Nullable(Of Byte)

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte? ToByte(Boolean value) {
            return ToByte(BooleanDataConvert.ToByte(value));
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte? ToByte(Boolean? value) {
            return ToByte(BooleanDataConvert.ToByte(value));
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte? ToByte(SqlBoolean value) {
            return ToByte(BooleanDataConvert.ToByte(value));
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte? ToByte(Byte value) {
            return value;
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte? ToByte(Byte? value) {
            return value;
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte? ToByte(SqlByte value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte? ToByte(Int16 value) {
            return (Byte) value;
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte? ToByte(Int16? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return (Byte) value.Value;
            }
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte? ToByte(SqlInt16 value) {
            if(value.IsNull) {
                return null;
            } else {
                return (Byte) value.Value;
            }
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte? ToByte(Int32 value) {
            return (Byte) value;
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte? ToByte(Int32? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return (Byte) value.Value;
            }
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte? ToByte(SqlInt32 value) {
            if(value.IsNull) {
                return null;
            } else {
                return (Byte) value.Value;
            }
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte? ToByte(Int64 value) {
            return (Byte) value;
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte? ToByte(Int64? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return (Byte) value.Value;
            }
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte? ToByte(SqlInt64 value) {
            if(value.IsNull) {
                return null;
            } else {
                return (Byte) value.Value;
            }
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte? ToByte(SqlParameter value) {
            return ToByte(value.Value);
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Byte? ToByte(Object value) {
            if(value == null || value is DBNull) {
                return null;
                // ---------- Boolean ----------
            } else if(value is Boolean) {
                return ToByte((Boolean) value);
            } else if(value is Boolean?) {
                return ToByte((Boolean?) value);
            } else if(value is SqlBoolean) {
                return ToByte((SqlBoolean) value);
                // ---------- Byte ----------
            } else if(value is Byte) {
                return ToByte((Byte) value);
            } else if(value is Byte?) {
                return ToByte((Byte?) value);
            } else if(value is SqlByte) {
                return ToByte((SqlByte) value);
                // ---------- Int16 ----------
            } else if(value is Int16) {
                return ToByte((Int16) value);
            } else if(value is Int16?) {
                return ToByte((Int16?) value);
            } else if(value is SqlInt16) {
                return ToByte((SqlInt16) value);
                // ---------- Int32 ----------
            } else if(value is Int32) {
                return ToByte((Int32) value);
            } else if(value is Int32?) {
                return ToByte((Int32?) value);
            } else if(value is SqlInt32) {
                return ToByte((SqlInt32) value);
                // ---------- Int64 ----------
            } else if(value is Int64) {
                return ToByte((Int64) value);
            } else if(value is Int64?) {
                return ToByte((Int64?) value);
            } else if(value is SqlInt64) {
                return ToByte((SqlInt64) value);
                // ---------- Enum ----------
            } else if(value is Enum) {
                return ToByte(Convert.ToByte(value));
                // ---------- Parameter ----------
            } else if(value is SqlParameter) {
                return ToByte((SqlParameter) value);
            } else {
                throw new InvalidCastException(String.Format(Properties.Resources.ErrorInvalidCast, RcwHelper.TypeName(value), typeof(Byte?)));
            }
        }

        #endregion

        #region Public Shared Methods - Object

        /// <overloads>
        /// Convert a Byte value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Object ToObjByte(Byte? value) {
            if(!value.HasValue) {
                return DBNull.Value;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a Byte value from one representation to another.
        /// </summary>
        public static Object ToObjByte(SqlByte value) {
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
        public static Byte DefaultIfNull(Byte? value) {
            return DefaultIfNull(value, (Byte) 0);
        }

        /// <summary>
        /// Convert a <c>NULL</c> value to the default value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return this value instead of null.</param>
        /// <returns>The new value.</returns>
        public static Byte DefaultIfNull(Byte? value, Byte defaultValue) {
            return value.GetValueOrDefault(defaultValue);
        }

        #endregion

        #region Public Shared Methods - NullIfDefault

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>The new value.</returns>
        public static Byte? NullIfDefault(Byte value) {
            return NullIfDefault(value, (Byte) 0);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static Byte? NullIfDefault(Byte value, Byte defaultValue) {
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
        public static Byte? NullIfDefault(Byte? value) {
            return NullIfDefault(value, (Byte) 0);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static Byte? NullIfDefault(Byte? value, Byte defaultValue) {
            if(value.GetValueOrDefault(defaultValue) == defaultValue) {
                return null;
            } else {
                return value;
            }
        }

        #endregion

    }

}
