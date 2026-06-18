using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Sirius.Architecture.Utility {

    // All members of the class that deal with the Double logical data type.
    partial class Cast {

        #region Public Shared Methods - Double

        /// <overloads>
        /// Convert a Double-precision Single value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double ToDouble(Single value, Double defaultValue) {
            return DefaultIfNull(ToDouble(value), defaultValue);
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double ToDouble(Single? value, Double defaultValue) {
            return DefaultIfNull(ToDouble(value), defaultValue);
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double ToDouble(SqlSingle value, Double defaultValue) {
            return DefaultIfNull(ToDouble(value), defaultValue);
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double ToDouble(Double value, Double defaultValue) {
            return DefaultIfNull(ToDouble(value), defaultValue);
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double ToDouble(Double? value, Double defaultValue) {
            return DefaultIfNull(ToDouble(value), defaultValue);
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double ToDouble(SqlDouble value, Double defaultValue) {
            return DefaultIfNull(ToDouble(value), defaultValue);
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double ToDouble(Decimal value, Double defaultValue) {
            return DefaultIfNull(ToDouble(value), defaultValue);
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double ToDouble(Decimal? value, Double defaultValue) {
            return DefaultIfNull(ToDouble(value), defaultValue);
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double ToDouble(SqlDecimal value, Double defaultValue) {
            return DefaultIfNull(ToDouble(value), defaultValue);
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double ToDouble(SqlMoney value, Double defaultValue) {
            return DefaultIfNull(ToDouble(value), defaultValue);
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double ToDouble(SqlParameter value, Double defaultValue) {
            return DefaultIfNull(ToDouble(value), defaultValue);
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double ToDouble(Object value, Double defaultValue) {
            return DefaultIfNull(ToDouble(value), defaultValue);
        }

        #endregion

        #region Public Shared Methods - Nullable(Of Double)

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double? ToDouble(Single value) {
            return value;
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double? ToDouble(Single? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double? ToDouble(SqlSingle value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double? ToDouble(Double value) {
            return value;
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double? ToDouble(Double? value) {
            return value;
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double? ToDouble(SqlDouble value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double? ToDouble(Decimal value) {
            return (Double) value;
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double? ToDouble(Decimal? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return (Double) value.Value;
            }
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double? ToDouble(SqlDecimal value) {
            if(value.IsNull) {
                return null;
            } else {
                return (Double) value.Value;
            }
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double? ToDouble(SqlMoney value) {
            if(value.IsNull) {
                return null;
            } else {
                return (Double) value.Value;
            }
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double? ToDouble(SqlParameter value) {
            return ToDouble(value.Value);
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Double? ToDouble(Object value) {
            if(value == null || value is DBNull) {
                return null;
                // ---------- Single ----------
            } else if(value is Single) {
                return ToDouble((Single) value);
            } else if(value is Single?) {
                return ToDouble((Single?) value);
            } else if(value is SqlSingle) {
                return ToDouble((SqlSingle) value);
                // ---------- Double ----------
            } else if(value is Double) {
                return ToDouble((Double) value);
            } else if(value is Double?) {
                return ToDouble((Double?) value);
            } else if(value is SqlDouble) {
                return ToDouble((SqlDouble) value);
                // ---------- Decimal ----------
            } else if(value is Decimal) {
                return ToDouble((Decimal) value);
            } else if(value is Decimal?) {
                return ToDouble((Decimal?) value);
            } else if(value is SqlDecimal) {
                return ToDouble((SqlDecimal) value);
                // ---------- Money ----------
            } else if(value is SqlMoney) {
                return ToDouble((SqlMoney) value);
                // ---------- Parameter ----------
            } else if(value is SqlParameter) {
                return ToDouble((SqlParameter) value);
            } else {
                throw new InvalidCastException(String.Format(Properties.Resources.ErrorInvalidCast, RcwHelper.TypeName(value), typeof(Double?)));
            }
        }

        #endregion

        #region Public Shared Methods - Object

        /// <overloads>
        /// Convert a Double-precision Single value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Object ToObjDouble(Double? value) {
            if(!value.HasValue) {
                return DBNull.Value;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a Double-precision Single value from one representation to another.
        /// </summary>
        public static Object ToObjDouble(SqlDouble value) {
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
        public static Double DefaultIfNull(Double? value) {
            return DefaultIfNull(value, (Double) 0);
        }

        /// <summary>
        /// Convert a <c>NULL</c> value to the default value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return this value instead of null.</param>
        /// <returns>The new value.</returns>
        public static Double DefaultIfNull(Double? value, Double defaultValue) {
            return value.GetValueOrDefault(defaultValue);
        }

        #endregion

        #region Public Shared Methods - NullIfDefault

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>The new value.</returns>
        public static Double? NullIfDefault(Double value) {
            return NullIfDefault(value, (Double) 0);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static Double? NullIfDefault(Double value, Double defaultValue) {
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
        public static Double? NullIfDefault(Double? value) {
            return NullIfDefault(value, (Double) 0);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static Double? NullIfDefault(Double? value, Double defaultValue) {
            if(value.GetValueOrDefault(defaultValue) == defaultValue) {
                return null;
            } else {
                return value;
            }
        }

        #endregion

    }

}
