using System;
using System.ComponentModel;
using System.Globalization;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// Sirius Legacy Persistence Format conversion methods. Use these to read and write data in a manner that is fully compatible
    /// with existing Sirius Legacy code that persists data to a text storage format.
    /// This class supports the Configuration components, and is not intended to be used in your code.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static class SiriusLPFConvert {

        #region Private Shared Variables

        // NOTE: Static data.
        // Constructing this object is thread-safe, but using it may not be. The code currently takes the risk
        // that two threads attempting to read from the object simultaneously will not cause a problem. It is
        // a small risk because data is only being read.
        private static readonly CultureInfo _culture;

        #endregion

        #region Constructors

        static SiriusLPFConvert() {

            // The VB6 and VB.NET CDate() operators behave subtly differently from each other, which is silly
            // when the whole point of them is to preserve existing code behaviour. We must therefore roll our
            // own conversions. After discussion with Rob Courtney and Mark Evans, it has been decided to use
            // the current culture, for 100% compatibility with the VB6 code.
            _culture = CultureInfo.CurrentCulture;

        }

        #endregion

        #region Public Shared Methods - FromString

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value.</returns>
        /// <exception cref="ArgumentNullException">The text is null.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Boolean ToBoolean(String value) {

            Boolean valueBoolean = false;
            Int64 valueInt64 = 0;
            if(value == null) {
                throw new ArgumentNullException("value");
            } else if(Boolean.TryParse(value, out valueBoolean)) {
                return valueBoolean;
            } else if(Int64.TryParse(value, NumberStyles.Any, _culture, out valueInt64)) {
                return valueInt64 != 0;
            } else {
                throw new FormatException(Properties.Resources.SiriusLPFConvertFormatException);
            }

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value.</returns>
        /// <exception cref="ArgumentNullException">The text is null.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Byte ToByte(String value) {

            return Byte.Parse(value, NumberStyles.Any, _culture);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value.</returns>
        /// <exception cref="ArgumentNullException">The text is null.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int16 ToInt16(String value) {

            return Int16.Parse(value, NumberStyles.Any, _culture);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value.</returns>
        /// <exception cref="ArgumentNullException">The text is null.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int32 ToInt32(String value) {

            return Int32.Parse(value, NumberStyles.Any, _culture);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value.</returns>
        /// <exception cref="ArgumentNullException">The text is null.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int64 ToInt64(String value) {

            return Int64.Parse(value, NumberStyles.Any, _culture);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value.</returns>
        /// <exception cref="ArgumentNullException">The text is null.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Single ToSingle(String value) {

            return Single.Parse(value, NumberStyles.Any, _culture);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value.</returns>
        /// <exception cref="ArgumentNullException">The text is null.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Double ToDouble(String value) {

            return Double.Parse(value, NumberStyles.Any, _culture);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value.</returns>
        /// <exception cref="ArgumentNullException">The text is null.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Decimal ToDecimal(String value) {

            return Decimal.Parse(value, NumberStyles.Any, _culture);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value.</returns>
        /// <exception cref="ArgumentNullException">The text is null.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static DateTime ToDateTime(String value) {

            // Mimic the VB6 CDate() operator behaviour. The VB.NET CDate() operator does not do this automatically.
            DateTime valueDateTime = DateTime.Parse(value, _culture, DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.AllowWhiteSpaces);
            if(valueDateTime.Date == DateTime.MinValue) {
                valueDateTime = new DateTime(1899, 12, 30, valueDateTime.Hour, valueDateTime.Minute, valueDateTime.Second, DateTimeKind.Unspecified);
            }
            return valueDateTime;

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value.</returns>
        /// <exception cref="ArgumentNullException">The text is null.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static TimeSpan ToTimeSpan(String value) {

            return TimeSpan.Parse(value);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value.</returns>
        /// <exception cref="ArgumentNullException">The text is null.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Guid ToGuid(String value) {

            return new Guid(value);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value.</returns>
        /// <exception cref="ArgumentNullException">The text is null.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        public static Byte[] ToByteArray(String value) {

            if(value == null) {
                throw new ArgumentNullException("value");
            }
            return BinHexConvert.ToByteArray(value);

        }

        #endregion

        #region Public Shared Methods - ToString

        /// <overloads>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Boolean value) {

            return value.ToString(_culture);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Byte value) {

            return value.ToString(_culture);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Int16 value) {

            return value.ToString(_culture);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Int32 value) {

            return value.ToString(_culture);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Int64 value) {

            return value.ToString(_culture);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Single value) {

            return value.ToString(_culture);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Double value) {

            return value.ToString(_culture);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Decimal value) {

            return value.ToString(_culture);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(DateTime value) {

            // Return <short date> <long time>, which will always be correctly recognised by the VB6 CDate() operator.
            return value.ToString("G", _culture);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(TimeSpan value) {

            return value.ToString();

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Guid value) {

            return value.ToString("B", _culture).ToUpperInvariant();

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Byte[] value) {

            return BinHexConvert.ToString(value);

        }

        #endregion
    }
}
