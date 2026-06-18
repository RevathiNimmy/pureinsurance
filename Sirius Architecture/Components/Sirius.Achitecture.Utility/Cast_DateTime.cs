using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Sirius.Architecture.Utility {

    // All members of the class that deal with the DateTime logical data type.
    partial class Cast {

        #region Public Shared Methods - DateTime

        /// <overloads>
        /// Convert a date/time value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTime ToDateTime(DateTime value, DateTime defaultValue) {
            return DefaultIfNull(ToDateTime(value), defaultValue);
        }

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTime ToDateTime(DateTime? value, DateTime defaultValue) {
            return DefaultIfNull(ToDateTime(value), defaultValue);
        }

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTime ToDateTime(SqlDateTime value, DateTime defaultValue) {
            return DefaultIfNull(ToDateTime(value), defaultValue);
        }

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTime ToDateTime(TimeSpan value, DateTime defaultValue) {
            return DefaultIfNull(ToDateTime(value), defaultValue);
        }

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTime ToDateTime(TimeSpan? value, DateTime defaultValue) {
            return DefaultIfNull(ToDateTime(value), defaultValue);
        }

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTime ToDateTime(SqlParameter value, DateTime defaultValue) {
            return DefaultIfNull(ToDateTime(value), defaultValue);
        }

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTime ToDateTime(Object value, DateTime defaultValue) {
            return DefaultIfNull(ToDateTime(value), defaultValue);
        }

        #endregion

        #region Public Shared Methods - Nullable(Of DateTime)

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTime? ToDateTime(DateTime value) {
            return value;
        }

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTime? ToDateTime(DateTime? value) {
            return value;
        }

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTime? ToDateTime(SqlDateTime value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTime? ToDateTime(TimeSpan value) {
            return new DateTime(value.Ticks);
        }

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTime? ToDateTime(TimeSpan? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return ToDateTime(value.Value);
            }
        }

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTime? ToDateTime(SqlParameter value) {
            return ToDateTime(value.Value);
        }

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTime? ToDateTime(Object value) {
            if(value == null || value is DBNull) {
                return null;
                // ---------- DateTime ----------
            } else if(value is DateTime) {
                return ToDateTime((DateTime) value);
            } else if(value is DateTime?) {
                return ToDateTime((DateTime?) value);
            } else if(value is SqlDateTime) {
                return ToDateTime((SqlDateTime) value);
                // ---------- TimeSpan ----------
            } else if(value is TimeSpan) {
                return ToDateTime((TimeSpan) value);
            } else if(value is TimeSpan?) {
                return ToDateTime((TimeSpan?) value);
                // ---------- Parameter ----------
            } else if(value is SqlParameter) {
                return ToDateTime((SqlParameter) value);
            } else {
                throw new InvalidCastException(String.Format(Properties.Resources.ErrorInvalidCast, RcwHelper.TypeName(value), typeof(DateTime?)));
            }
        }

        #endregion

        #region Public Shared Methods - Object

        /// <overloads>
        /// Convert a date/time value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static Object ToObjDateTime(DateTime? value) {
            if(!value.HasValue) {
                return DBNull.Value;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static Object ToObjDateTime(SqlDateTime value) {
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
        public static DateTime DefaultIfNull(DateTime? value) {
            return DefaultIfNull(value, DateTime.MinValue);
        }

        /// <summary>
        /// Convert a <c>NULL</c> value to the default value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return this value instead of null.</param>
        /// <returns>The new value.</returns>
        public static DateTime DefaultIfNull(DateTime? value, DateTime defaultValue) {
            return value.GetValueOrDefault(defaultValue);
        }

        #endregion

        #region Public Shared Methods - NullIfDefault

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>The new value.</returns>
        public static DateTime? NullIfDefault(DateTime value) {
            return NullIfDefault(value, DateTime.MinValue);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static DateTime? NullIfDefault(DateTime value, DateTime defaultValue) {
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
        public static DateTime? NullIfDefault(DateTime? value) {
            return NullIfDefault(value, DateTime.MinValue);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static DateTime? NullIfDefault(DateTime? value, DateTime defaultValue) {
            if(value.GetValueOrDefault(defaultValue) == defaultValue) {
                return null;
            } else {
                return value;
            }
        }

        #endregion

    }

}
