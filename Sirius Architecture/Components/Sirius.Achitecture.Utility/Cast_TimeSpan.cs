using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Sirius.Architecture.Utility {

    // All members of the class that deal with the TimeSpan logical data type.
    partial class Cast {

        #region Public Shared Methods - TimeSpan

        /// <overloads>
        /// Convert a time value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a time value from one representation to another.
        /// </summary>
        public static TimeSpan ToTimeSpan(DateTime value, TimeSpan defaultValue) {
            return DefaultIfNull(ToTimeSpan(value), defaultValue);
        }

        /// <summary>
        /// Convert a time value from one representation to another.
        /// </summary>
        public static TimeSpan ToTimeSpan(DateTime? value, TimeSpan defaultValue) {
            return DefaultIfNull(ToTimeSpan(value), defaultValue);
        }

        /// <summary>
        /// Convert a time value from one representation to another.
        /// </summary>
        public static TimeSpan ToTimeSpan(SqlDateTime value, TimeSpan defaultValue) {
            return DefaultIfNull(ToTimeSpan(value), defaultValue);
        }

        /// <summary>
        /// Convert a time value from one representation to another.
        /// </summary>
        public static TimeSpan ToTimeSpan(TimeSpan value, TimeSpan defaultValue) {
            return DefaultIfNull(ToTimeSpan(value), defaultValue);
        }

        /// <summary>
        /// Convert a time value from one representation to another.
        /// </summary>
        public static TimeSpan ToTimeSpan(TimeSpan? value, TimeSpan defaultValue) {
            return DefaultIfNull(ToTimeSpan(value), defaultValue);
        }

        /// <summary>
        /// Convert a time value from one representation to another.
        /// </summary>
        public static TimeSpan ToTimeSpan(SqlParameter value, TimeSpan defaultValue) {
            return DefaultIfNull(ToTimeSpan(value), defaultValue);
        }

        /// <summary>
        /// Convert a time value from one representation to another.
        /// </summary>
        public static TimeSpan ToTimeSpan(Object value, TimeSpan defaultValue) {
            return DefaultIfNull(ToTimeSpan(value), defaultValue);
        }

        #endregion

        #region Public Shared Methods - Nullable(Of TimeSpan)

        /// <summary>
        /// Convert a time value from one representation to another.
        /// </summary>
        public static TimeSpan? ToTimeSpan(DateTime value) {
            return value.TimeOfDay;
        }

        /// <summary>
        /// Convert a time value from one representation to another.
        /// </summary>
        public static TimeSpan? ToTimeSpan(DateTime? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return ToTimeSpan(value.Value);
            }
        }

        /// <summary>
        /// Convert a time value from one representation to another.
        /// </summary>
        public static TimeSpan? ToTimeSpan(SqlDateTime value) {
            if(value.IsNull) {
                return null;
            } else {
                return ToTimeSpan(value.Value);
            }
        }

        /// <summary>
        /// Convert a time value from one representation to another.
        /// </summary>
        public static TimeSpan? ToTimeSpan(TimeSpan value) {
            return value;
        }

        /// <summary>
        /// Convert a time value from one representation to another.
        /// </summary>
        public static TimeSpan? ToTimeSpan(TimeSpan? value) {
            return value;
        }

        /// <summary>
        /// Convert a time value from one representation to another.
        /// </summary>
        public static TimeSpan? ToTimeSpan(SqlParameter value) {
            return ToTimeSpan(value.Value);
        }

        /// <summary>
        /// Convert a time value from one representation to another.
        /// </summary>
        public static TimeSpan? ToTimeSpan(Object value) {
            if(value == null || value is DBNull) {
                return null;
                // ---------- DateTime ----------
            } else if(value is DateTime) {
                return ToTimeSpan((DateTime) value);
            } else if(value is DateTime?) {
                return ToTimeSpan((DateTime?) value);
            } else if(value is SqlDateTime) {
                return ToTimeSpan((SqlDateTime) value);
                // ---------- TimeSpan ----------
            } else if(value is TimeSpan) {
                return ToTimeSpan((TimeSpan) value);
            } else if(value is TimeSpan?) {
                return ToTimeSpan((TimeSpan?) value);
                // ---------- Parameter ----------
            } else if(value is SqlParameter) {
                return ToTimeSpan((SqlParameter) value);
            } else {
                throw new InvalidCastException(String.Format(Properties.Resources.ErrorInvalidCast, RcwHelper.TypeName(value), typeof(TimeSpan?)));
            }
        }

        #endregion

        #region Public Shared Methods - DefaultIfNull

        /// <summary>
        /// Convert a <c>NULL</c> value to the default value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>The new value.</returns>
        public static TimeSpan DefaultIfNull(TimeSpan? value) {
            return DefaultIfNull(value, TimeSpan.Zero);
        }

        /// <summary>
        /// Convert a <c>NULL</c> value to the default value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return this value instead of null.</param>
        /// <returns>The new value.</returns>
        public static TimeSpan DefaultIfNull(TimeSpan? value, TimeSpan defaultValue) {
            return value.GetValueOrDefault(defaultValue);
        }

        #endregion

        #region Public Shared Methods - NullIfDefault

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>The new value.</returns>
        public static TimeSpan? NullIfDefault(TimeSpan value) {
            return NullIfDefault(value, TimeSpan.Zero);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static TimeSpan? NullIfDefault(TimeSpan value, TimeSpan defaultValue) {
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
        public static TimeSpan? NullIfDefault(TimeSpan? value) {
            return NullIfDefault(value, TimeSpan.Zero);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static TimeSpan? NullIfDefault(TimeSpan? value, TimeSpan defaultValue) {
            if(value.GetValueOrDefault(defaultValue) == defaultValue) {
                return null;
            } else {
                return value;
            }
        }

        #endregion

    }

}
