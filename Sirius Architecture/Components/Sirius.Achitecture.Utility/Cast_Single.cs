using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Sirius.Architecture.Utility {

    // All members of the class that deal with the Single logical data type.
    partial class Cast {

        #region Public Shared Methods - Single

        /// <overloads>
        /// Convert a single-precision Single value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single ToSingle(Single value, Single defaultValue) {
            return DefaultIfNull(ToSingle(value), defaultValue);
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single ToSingle(Single? value, Single defaultValue) {
            return DefaultIfNull(ToSingle(value), defaultValue);
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single ToSingle(SqlSingle value, Single defaultValue) {
            return DefaultIfNull(ToSingle(value), defaultValue);
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single ToSingle(Double value, Single defaultValue) {
            return DefaultIfNull(ToSingle(value), defaultValue);
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single ToSingle(Double? value, Single defaultValue) {
            return DefaultIfNull(ToSingle(value), defaultValue);
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single ToSingle(SqlDouble value, Single defaultValue) {
            return DefaultIfNull(ToSingle(value), defaultValue);
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single ToSingle(Decimal value, Single defaultValue) {
            return DefaultIfNull(ToSingle(value), defaultValue);
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single ToSingle(Decimal? value, Single defaultValue) {
            return DefaultIfNull(ToSingle(value), defaultValue);
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single ToSingle(SqlDecimal value, Single defaultValue) {
            return DefaultIfNull(ToSingle(value), defaultValue);
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single ToSingle(SqlMoney value, Single defaultValue) {
            return DefaultIfNull(ToSingle(value), defaultValue);
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single ToSingle(SqlParameter value, Single defaultValue) {
            return DefaultIfNull(ToSingle(value), defaultValue);
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single ToSingle(Object value, Single defaultValue) {
            return DefaultIfNull(ToSingle(value), defaultValue);
        }

        #endregion

        #region Public Shared Methods - Nullable(Of Single)

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single? ToSingle(Single value) {
            return value;
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single? ToSingle(Single? value) {
            return value;
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single? ToSingle(SqlSingle value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single? ToSingle(Double value) {
            return (Single) value;
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single? ToSingle(Double? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return (Single) value.Value;
            }
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single? ToSingle(SqlDouble value) {
            if(value.IsNull) {
                return null;
            } else {
                return (Single) value.Value;
            }
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single? ToSingle(Decimal value) {
            return (Single) value;
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single? ToSingle(Decimal? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return (Single) value.Value;
            }
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single? ToSingle(SqlDecimal value) {
            if(value.IsNull) {
                return null;
            } else {
                return (Single) value.Value;
            }
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single? ToSingle(SqlMoney value) {
            if(value.IsNull) {
                return null;
            } else {
                return (Single) value.Value;
            }
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single? ToSingle(SqlParameter value) {
            return ToSingle(value.Value);
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Single? ToSingle(Object value) {
            if(value == null || value is DBNull) {
                return null;
                // ---------- Single ----------
            } else if(value is Single) {
                return ToSingle((Single) value);
            } else if(value is Single?) {
                return ToSingle((Single?) value);
            } else if(value is SqlSingle) {
                return ToSingle((SqlSingle) value);
                // ---------- Double ----------
            } else if(value is Double) {
                return ToSingle((Double) value);
            } else if(value is Double?) {
                return ToSingle((Double?) value);
            } else if(value is SqlDouble) {
                return ToSingle((SqlDouble) value);
                // ---------- Decimal ----------
            } else if(value is Decimal) {
                return ToSingle((Decimal) value);
            } else if(value is Decimal?) {
                return ToSingle((Decimal?) value);
            } else if(value is SqlDecimal) {
                return ToSingle((SqlDecimal) value);
                // ---------- Money ----------
            } else if(value is SqlMoney) {
                return ToSingle((SqlMoney) value);
                // ---------- Parameter ----------
            } else if(value is SqlParameter) {
                return ToSingle((SqlParameter) value);
            } else {
                throw new InvalidCastException(String.Format(Properties.Resources.ErrorInvalidCast, RcwHelper.TypeName(value), typeof(Single?)));
            }
        }

        #endregion

        #region Public Shared Methods - Object

        /// <overloads>
        /// Convert a single-precision Single value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Object ToObjSingle(Single? value) {
            if(!value.HasValue) {
                return DBNull.Value;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a single-precision Single value from one representation to another.
        /// </summary>
        public static Object ToObjSingle(SqlSingle value) {
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
        public static Single DefaultIfNull(Single? value) {
            return DefaultIfNull(value, (Single) 0);
        }

        /// <summary>
        /// Convert a <c>NULL</c> value to the default value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return this value instead of null.</param>
        /// <returns>The new value.</returns>
        public static Single DefaultIfNull(Single? value, Single defaultValue) {
            return value.GetValueOrDefault(defaultValue);
        }

        #endregion

        #region Public Shared Methods - NullIfDefault

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>The new value.</returns>
        public static Single? NullIfDefault(Single value) {
            return NullIfDefault(value, (Single) 0);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static Single? NullIfDefault(Single value, Single defaultValue) {
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
        public static Single? NullIfDefault(Single? value) {
            return NullIfDefault(value, (Single) 0);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static Single? NullIfDefault(Single? value, Single defaultValue) {
            if(value.GetValueOrDefault(defaultValue) == defaultValue) {
                return null;
            } else {
                return value;
            }
        }

        #endregion

    }

}
