using System;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// <c>Nullable(Of T)</c> does not implement <c>IFormattable</c>, so we implement it here.
    /// Call these methods exactly as you would call them on the corresponding non-nullable type.
    /// A null value is rendered as an empty string.
    /// </summary>
    public static class NullableFormat {

        /// <summary>
        /// Formats the value of the current instance using the specified format.
        /// </summary>
        public static string ToString<T>(this T? value, string format) where T : struct, IFormattable {
            // IFormattable only provides the 2-parameter overload, so simulate this one.
            return ToString<T>(value, format, null);
        }

        /// <summary>
        /// Formats the value of the current instance using the specified format.
        /// </summary>
        public static string ToString<T>(this T? value, IFormatProvider formatProvider) where T : struct, IFormattable {
            // IFormattable only provides the 2-parameter overload, so simulate this one.
            return ToString<T>(value, null, formatProvider);
        }

        /// <summary>
        /// Formats the value of the current instance using the specified format.
        /// </summary>
        public static string ToString<T>(this T? value, string format, IFormatProvider formatProvider) where T : struct, IFormattable {
            // Call IFormattable for non-null, and return empty string for null.
            if(!value.HasValue) {
                return string.Empty;
            } else {
                return ((IFormattable) value.Value).ToString(format, formatProvider);
            }
        }
    }
}
