using System;
using System.Xml;
using System.Xml.XPath;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// XML text conversion methods. This class is a safer version of <see cref="XmlConvert"/> and should be used instead.
    /// It handles blank and null values appropriately without throwing exceptions.
    /// </summary>
    public static class XmlSafeConvert {

        #region Public Shared Methods - ToValue

        /// <overloads>
        /// Return the text value of an XML node (and all its child nodes) even if it does not exist.
        /// </overloads>
        /// <summary>
        /// Return the text value of an XML node (and all its child nodes) even if it does not exist.
        /// </summary>
        /// <param name="value">The node to read.</param>
        /// <returns>The text value of the node (and all its child nodes), or <see langword="null"/> if the node itself is <see langword="null"/>.</returns>
        public static String ToValue(XPathNavigator value) {

            if(value == null) {
                return null;
            } else {
                return value.Value;
            }

        }

        /// <summary>
        /// Return the text value of an XML node (and all its child nodes) even if it does not exist.
        /// </summary>
        /// <param name="value">The node to read.</param>
        /// <returns>The text value of the node (and all its child nodes), or <see langword="null"/> if the node itself is <see langword="null"/>.</returns>
        public static String ToValue(XmlNode value) {

            if(value == null) {
                return null;
            } else {
                return value.InnerText;
                // NOTE: this has the same behaviour as XPathNavigator.Value above; XmlNode.Value behaves differently.
            }

        }

        #endregion

        #region Public Shared Methods - FromString (Nullable)

        /// <overloads>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        public static Boolean? ToBoolean(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return XmlConvert.ToBoolean(value);
            }

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Byte? ToByte(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return XmlConvert.ToByte(value);
            }

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int16? ToInt16(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return XmlConvert.ToInt16(value);
            }

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int32? ToInt32(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return XmlConvert.ToInt32(value);
            }

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int64? ToInt64(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return XmlConvert.ToInt64(value);
            }

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Single? ToSingle(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return XmlConvert.ToSingle(value);
            }

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Double? ToDouble(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return XmlConvert.ToDouble(value);
            }

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Decimal? ToDecimal(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return XmlConvert.ToDecimal(value);
            }

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        public static DateTime? ToDateTime(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.RoundtripKind);
            }

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        public static TimeSpan? ToTimeSpan(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return XmlConvert.ToTimeSpan(value);
            }

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        public static DateTimeOffset? ToDateTimeOffset(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return XmlConvert.ToDateTimeOffset(value);
            }

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        public static Guid? ToGuid(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return XmlConvert.ToGuid(value);
            }

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        public static Byte[] ToByteArray(String value) {

            if(value == null) {
                return null;
            } else {
                return BinHexConvert.ToByteArray(value);
            }

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <typeparam name="T">The enum type to convert to.</typeparam>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="ArgumentException">The text is not one of the expected values for the enumeration.</exception>
        public static T? ToEnum<T>(String value) where T : struct {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return (T) Enum.Parse(typeof(T), value);
            }

        }

        #endregion

        #region Public Shared Methods - FromString

        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        public static Boolean ToBoolean(String value, Boolean defaultValue) {

            return Cast.DefaultIfNull(ToBoolean(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Byte ToByte(String value, Byte defaultValue) {

            return Cast.DefaultIfNull(ToByte(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int16 ToInt16(String value, Int16 defaultValue) {

            return Cast.DefaultIfNull(ToInt16(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int32 ToInt32(String value, Int32 defaultValue) {

            return Cast.DefaultIfNull(ToInt32(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int64 ToInt64(String value, Int64 defaultValue) {

            return Cast.DefaultIfNull(ToInt64(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Single ToSingle(String value, Single defaultValue) {

            return Cast.DefaultIfNull(ToSingle(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Double ToDouble(String value, Double defaultValue) {

            return Cast.DefaultIfNull(ToDouble(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Decimal ToDecimal(String value, Decimal defaultValue) {

            return Cast.DefaultIfNull(ToDecimal(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        public static DateTime ToDateTime(String value, DateTime defaultValue) {

            return Cast.DefaultIfNull(ToDateTime(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        public static TimeSpan ToTimeSpan(String value, TimeSpan defaultValue) {

            return Cast.DefaultIfNull(ToTimeSpan(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        public static DateTimeOffset ToDateTimeOffset(String value, DateTimeOffset defaultValue) {

            return Cast.DefaultIfNull(ToDateTimeOffset(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        public static Guid ToGuid(String value, Guid defaultValue) {

            return Cast.DefaultIfNull(ToGuid(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the XSD data type.</exception>
        public static Byte[] ToByteArray(String value, Byte[] defaultValue) {

            return Cast.DefaultIfNull(ToByteArray(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the XSD data type formatting rules.
        /// </summary>
        /// <typeparam name="T">The enum type to convert to.</typeparam>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="ArgumentException">The text is not one of the expected values for the XSD data type.</exception>
        public static T ToEnum<T>(String value, T defaultValue) where T : struct {

            return ToEnum<T>(value).GetValueOrDefault(defaultValue);

        }

        #endregion

        #region Public Shared Methods - ToString (Nullable)

        /// <overloads>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or blank if the value is <see langword="null"/>.</returns>
        public static String ToString(Boolean? value) {

            if(!value.HasValue) {
                return String.Empty;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or blank if the value is <see langword="null"/>.</returns>
        public static String ToString(Byte? value) {

            if(!value.HasValue) {
                return String.Empty;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or blank if the value is <see langword="null"/>.</returns>
        public static String ToString(Int16? value) {

            if(!value.HasValue) {
                return String.Empty;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or blank if the value is <see langword="null"/>.</returns>
        public static String ToString(Int32? value) {

            if(!value.HasValue) {
                return String.Empty;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or blank if the value is <see langword="null"/>.</returns>
        public static String ToString(Int64? value) {

            if(!value.HasValue) {
                return String.Empty;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or blank if the value is <see langword="null"/>.</returns>
        public static String ToString(Single? value) {

            if(!value.HasValue) {
                return String.Empty;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or blank if the value is <see langword="null"/>.</returns>
        public static String ToString(Double? value) {

            if(!value.HasValue) {
                return String.Empty;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or blank if the value is <see langword="null"/>.</returns>
        public static String ToString(Decimal? value) {

            if(!value.HasValue) {
                return String.Empty;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or blank if the value is <see langword="null"/>.</returns>
        public static String ToString(DateTime? value) {

            if(!value.HasValue) {
                return String.Empty;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or blank if the value is <see langword="null"/>.</returns>
        public static String ToString(TimeSpan? value) {

            if(!value.HasValue) {
                return String.Empty;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or blank if the value is <see langword="null"/>.</returns>
        public static String ToString(DateTimeOffset? value) {

            if(!value.HasValue) {
                return String.Empty;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or blank if the value is <see langword="null"/>.</returns>
        public static String ToString(Guid? value) {

            if(!value.HasValue) {
                return String.Empty;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <typeparam name="T">The enum type to convert from.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or blank if the value is <see langword="null"/>.</returns>
        public static String ToString<T>(T? value) where T : struct {

            if(!value.HasValue) {
                return String.Empty;
            } else {
                return ToString(value.Value);
            }
        }

        #endregion

        #region Public Shared Methods - ToString

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Boolean value) {

            return XmlConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Byte value) {

            return XmlConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Int16 value) {

            return XmlConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Int32 value) {

            return XmlConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Int64 value) {

            return XmlConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Single value) {

            return XmlConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Double value) {

            return XmlConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Decimal value) {

            return XmlConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(DateTime value) {

            return XmlConvert.ToString(value, XmlDateTimeSerializationMode.RoundtripKind);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(TimeSpan value) {

            return XmlConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(DateTimeOffset value) {

            return XmlConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Guid value) {

            return XmlConvert.ToString(value).ToUpperInvariant();

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Byte[] value) {

            if(value == null) {
                return String.Empty;
            } else {
                return BinHexConvert.ToString(value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the XSD data type formatting rules.
        /// </summary>
        /// <typeparam name="T">The enum type to convert from.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString<T>(T value) where T : struct {

            return Enum.Format(typeof(T), value, "g");

        }

        #endregion
    }
}
