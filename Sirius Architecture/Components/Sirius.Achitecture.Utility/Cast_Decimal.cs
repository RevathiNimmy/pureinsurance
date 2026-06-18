using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Sirius.Architecture.Utility {

    // All members of the class that deal with the Decimal logical data type.
    partial class Cast {

        #region Public Shared Methods - Decimal

        /// <overloads>
        /// Convert a Decimal value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal ToDecimal(Single value, Decimal defaultValue) {
            return DefaultIfNull(ToDecimal(value), defaultValue);
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal ToDecimal(Single? value, Decimal defaultValue) {
            return DefaultIfNull(ToDecimal(value), defaultValue);
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal ToDecimal(SqlSingle value, Decimal defaultValue) {
            return DefaultIfNull(ToDecimal(value), defaultValue);
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal ToDecimal(Double value, Decimal defaultValue) {
            return DefaultIfNull(ToDecimal(value), defaultValue);
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal ToDecimal(Double? value, Decimal defaultValue) {
            return DefaultIfNull(ToDecimal(value), defaultValue);
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal ToDecimal(SqlDouble value, Decimal defaultValue) {
            return DefaultIfNull(ToDecimal(value), defaultValue);
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal ToDecimal(Decimal value, Decimal defaultValue) {
            return DefaultIfNull(ToDecimal(value), defaultValue);
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal ToDecimal(Decimal? value, Decimal defaultValue) {
            return DefaultIfNull(ToDecimal(value), defaultValue);
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal ToDecimal(SqlDecimal value, Decimal defaultValue) {
            return DefaultIfNull(ToDecimal(value), defaultValue);
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal ToDecimal(SqlMoney value, Decimal defaultValue) {
            return DefaultIfNull(ToDecimal(value), defaultValue);
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support writing values to ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static Decimal ToDecimal(Int64 value, Decimal defaultValue) {
            return DefaultIfNull(ToDecimal(value), defaultValue);
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support writing values to ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static Decimal ToDecimal(Int64? value, Decimal defaultValue) {
            return DefaultIfNull(ToDecimal(value), defaultValue);
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support writing values to ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static Decimal ToDecimal(SqlInt64 value, Decimal defaultValue) {
            return DefaultIfNull(ToDecimal(value), defaultValue);
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal ToDecimal(SqlParameter value, Decimal defaultValue) {
            return DefaultIfNull(ToDecimal(value), defaultValue);
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal ToDecimal(Object value, Decimal defaultValue) {
            return DefaultIfNull(ToDecimal(value), defaultValue);
        }

        #endregion

        #region Public Shared Methods - Nullable(Of Decimal)

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal? ToDecimal(Single value) {
            return (Decimal) value;
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal? ToDecimal(Single? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return (Decimal) value.Value;
            }
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal? ToDecimal(SqlSingle value) {
            if(value.IsNull) {
                return null;
            } else {
                return (Decimal) value.Value;
            }
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal? ToDecimal(Double value) {
            return (Decimal) value;
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal? ToDecimal(Double? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return (Decimal) value.Value;
            }
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal? ToDecimal(SqlDouble value) {
            if(value.IsNull) {
                return null;
            } else {
                return (Decimal) value.Value;
            }
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal? ToDecimal(Decimal value) {
            return value;
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal? ToDecimal(Decimal? value) {
            return value;
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal? ToDecimal(SqlDecimal value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal? ToDecimal(SqlMoney value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support writing values to ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static Decimal? ToDecimal(Int64 value) {
            return value;
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support writing values to ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static Decimal? ToDecimal(Int64? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support writing values to ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static Decimal? ToDecimal(SqlInt64 value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal? ToDecimal(SqlParameter value) {
            return ToDecimal(value.Value);
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Decimal? ToDecimal(Object value) {
            if(value == null || value is DBNull) {
                return null;
                // ---------- Single ----------
            } else if(value is Single) {
                return ToDecimal((Single) value);
            } else if(value is Single?) {
                return ToDecimal((Single?) value);
            } else if(value is SqlSingle) {
                return ToDecimal((SqlSingle) value);
                // ---------- Double ----------
            } else if(value is Double) {
                return ToDecimal((Double) value);
            } else if(value is Double?) {
                return ToDecimal((Double?) value);
            } else if(value is SqlDouble) {
                return ToDecimal((SqlDouble) value);
                // ---------- Decimal ----------
            } else if(value is Decimal) {
                return ToDecimal((Decimal) value);
            } else if(value is Decimal?) {
                return ToDecimal((Decimal?) value);
            } else if(value is SqlDecimal) {
                return ToDecimal((SqlDecimal) value);
                // ---------- Money ----------
            } else if(value is SqlMoney) {
                return ToDecimal((SqlMoney) value);
                // ---------- Int64 ----------
            } else if(value is Int64) {
                return ToDecimal((Int64) value);
            } else if(value is Int64?) {
                return ToDecimal((Int64?) value);
            } else if(value is SqlInt64) {
                return ToDecimal((SqlInt64) value);
                // ---------- Parameter ----------
            } else if(value is SqlParameter) {
                return ToDecimal((SqlParameter) value);
            } else {
                throw new InvalidCastException(String.Format(Properties.Resources.ErrorInvalidCast, RcwHelper.TypeName(value), typeof(Decimal?)));
            }
        }

        #endregion

        #region Public Shared Methods - Object

        /// <overloads>
        /// Convert a Decimal value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Object ToObjDecimal(Decimal? value) {
            if(!value.HasValue) {
                return DBNull.Value;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Object ToObjDecimal(SqlDecimal value) {
            if(value.IsNull) {
                return DBNull.Value;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a Decimal value from one representation to another.
        /// </summary>
        public static Object ToObjDecimal(SqlMoney value) {
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
        public static Decimal DefaultIfNull(Decimal? value) {
            return DefaultIfNull(value, (Decimal) 0);
        }

        /// <summary>
        /// Convert a <c>NULL</c> value to the default value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return this value instead of null.</param>
        /// <returns>The new value.</returns>
        public static Decimal DefaultIfNull(Decimal? value, Decimal defaultValue) {
            return value.GetValueOrDefault(defaultValue);
        }

        #endregion

        #region Public Shared Methods - NullIfDefault

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>The new value.</returns>
        public static Decimal? NullIfDefault(Decimal value) {
            return NullIfDefault(value, (Decimal) 0);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static Decimal? NullIfDefault(Decimal value, Decimal defaultValue) {
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
        public static Decimal? NullIfDefault(Decimal? value) {
            return NullIfDefault(value, (Decimal) 0);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static Decimal? NullIfDefault(Decimal? value, Decimal defaultValue) {
            if(value.GetValueOrDefault(defaultValue) == defaultValue) {
                return null;
            } else {
                return value;
            }
        }

        #endregion

    }

}
