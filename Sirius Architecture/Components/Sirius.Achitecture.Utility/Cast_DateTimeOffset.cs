using System;
using System.Data.SqlClient;

namespace Sirius.Architecture.Utility {

    // All members of the class that deal with the DateTimeOffset logical data type.
    partial class Cast {

        #region Public Shared Methods - DateTimeOffset

        /// <overloads>
        /// Convert a date/time value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTimeOffset ToDateTimeOffset(DateTimeOffset value, DateTimeOffset defaultValue) {
            return DefaultIfNull(ToDateTimeOffset(value), defaultValue);
        }

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTimeOffset ToDateTimeOffset(DateTimeOffset? value, DateTimeOffset defaultValue) {
            return DefaultIfNull(ToDateTimeOffset(value), defaultValue);
        }

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTimeOffset ToDateTimeOffset(SqlParameter value, DateTimeOffset defaultValue) {
            return DefaultIfNull(ToDateTimeOffset(value), defaultValue);
        }

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTimeOffset ToDateTimeOffset(Object value, DateTimeOffset defaultValue) {
            return DefaultIfNull(ToDateTimeOffset(value), defaultValue);
        }

        #endregion

        #region Public Shared Methods - Nullable(Of DateTimeOffset)

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTimeOffset? ToDateTimeOffset(DateTimeOffset value) {
            return value;
        }

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTimeOffset? ToDateTimeOffset(DateTimeOffset? value) {
            return value;
        }

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTimeOffset? ToDateTimeOffset(SqlParameter value) {
            return ToDateTimeOffset(value.Value);
        }

        /// <summary>
        /// Convert a date/time value from one representation to another.
        /// </summary>
        public static DateTimeOffset? ToDateTimeOffset(Object value) {
            if(value == null || value is DBNull) {
                return null;
                // ---------- DateTimeOffset ----------
            } else if(value is DateTimeOffset) {
                return ToDateTimeOffset((DateTimeOffset) value);
            } else if(value is DateTimeOffset?) {
                return ToDateTimeOffset((DateTimeOffset?) value);
                // ---------- Parameter ----------
            } else if(value is SqlParameter) {
                return ToDateTimeOffset((SqlParameter) value);
            } else {
                throw new InvalidCastException(String.Format(Properties.Resources.ErrorInvalidCast, RcwHelper.TypeName(value), typeof(DateTimeOffset?)));
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
        public static Object ToObjDateTimeOffset(DateTimeOffset? value) {
            if(!value.HasValue) {
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
        public static DateTimeOffset DefaultIfNull(DateTimeOffset? value) {
            return DefaultIfNull(value, DateTimeOffset.MinValue);
        }

        /// <summary>
        /// Convert a <c>NULL</c> value to the default value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return this value instead of null.</param>
        /// <returns>The new value.</returns>
        public static DateTimeOffset DefaultIfNull(DateTimeOffset? value, DateTimeOffset defaultValue) {
            return value.GetValueOrDefault(defaultValue);
        }

        #endregion

        #region Public Shared Methods - NullIfDefault

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>The new value.</returns>
        public static DateTimeOffset? NullIfDefault(DateTimeOffset value) {
            return NullIfDefault(value, DateTimeOffset.MinValue);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static DateTimeOffset? NullIfDefault(DateTimeOffset value, DateTimeOffset defaultValue) {
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
        public static DateTimeOffset? NullIfDefault(DateTimeOffset? value) {
            return NullIfDefault(value, DateTimeOffset.MinValue);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static DateTimeOffset? NullIfDefault(DateTimeOffset? value, DateTimeOffset defaultValue) {
            if(value.GetValueOrDefault(defaultValue) == defaultValue) {
                return null;
            } else {
                return value;
            }
        }

        #endregion

    }

}
