using System;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// String database value conversion methods. Use these to interpret non-String data that is stored
    /// in String columns in the Sirius database. This class exists ONLY to support existing columns;
    /// all new columns should use the appropriate SQL Server native types.
    /// </summary>
    public static class StringDataConvert {

        // This class is currently a wrapper round SiriusLPFConvert for ease of coding.
        // This implementation may change in future.

        #region Public Shared Methods - FromObject (Nullable)

        /// <overloads>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Boolean? ToBoolean(object value) {

            return ToBoolean(Cast.ToString(value));

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Byte? ToByte(object value) {

            return ToByte(Cast.ToString(value));

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int16? ToInt16(object value) {

            return ToInt16(Cast.ToString(value));

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int32? ToInt32(object value) {

            return ToInt32(Cast.ToString(value));

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int64? ToInt64(object value) {

            return ToInt64(Cast.ToString(value));

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Single? ToSingle(object value) {

            return ToSingle(Cast.ToString(value));

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Double? ToDouble(object value) {

            return ToDouble(Cast.ToString(value));

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Decimal? ToDecimal(object value) {

            return ToDecimal(Cast.ToString(value));

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static DateTime? ToDateTime(object value) {

            return ToDateTime(Cast.ToString(value));

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static TimeSpan? ToTimeSpan(object value) {

            return ToTimeSpan(Cast.ToString(value));

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Guid? ToGuid(object value) {

            return ToGuid(Cast.ToString(value));

        }

        /// <overloads>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        public static Byte[] ToByteArray(object value) {

            return ToByteArray(Cast.ToString(value));

        }

        #endregion

        #region Public Shared Methods - FromString (Nullable)

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Boolean? ToBoolean(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return SiriusLPFConvert.ToBoolean(value);
            }

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Byte? ToByte(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return SiriusLPFConvert.ToByte(value);
            }

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int16? ToInt16(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return SiriusLPFConvert.ToInt16(value);
            }

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int32? ToInt32(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return SiriusLPFConvert.ToInt32(value);
            }

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int64? ToInt64(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return SiriusLPFConvert.ToInt64(value);
            }

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Single? ToSingle(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return SiriusLPFConvert.ToSingle(value);
            }

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Double? ToDouble(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return SiriusLPFConvert.ToDouble(value);
            }

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Decimal? ToDecimal(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return SiriusLPFConvert.ToDecimal(value);
            }

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static DateTime? ToDateTime(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return SiriusLPFConvert.ToDateTime(value);
            }

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static TimeSpan? ToTimeSpan(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return SiriusLPFConvert.ToTimeSpan(value);
            }

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Guid? ToGuid(String value) {

            if(String.IsNullOrEmpty(value)) {
                return null;
            } else {
                return SiriusLPFConvert.ToGuid(value);
            }

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <returns>The strongly-typed value, or <see langword="null"/> if the text is null.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        public static Byte[] ToByteArray(String value) {

            if(value == null) {
                return null;
            } else {
                return SiriusLPFConvert.ToByteArray(value);
            }

        }

        #endregion

        #region Public Shared Methods - FromObject

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Boolean ToBoolean(object value, Boolean defaultValue) {

            return Cast.DefaultIfNull(ToBoolean(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Byte ToByte(object value, Byte defaultValue) {

            return Cast.DefaultIfNull(ToByte(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int16 ToInt16(object value, Int16 defaultValue) {

            return Cast.DefaultIfNull(ToInt16(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int32 ToInt32(object value, Int32 defaultValue) {

            return Cast.DefaultIfNull(ToInt32(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int64 ToInt64(object value, Int64 defaultValue) {

            return Cast.DefaultIfNull(ToInt64(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Single ToSingle(object value, Single defaultValue) {

            return Cast.DefaultIfNull(ToSingle(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Double ToDouble(object value, Double defaultValue) {

            return Cast.DefaultIfNull(ToDouble(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Decimal ToDecimal(object value, Decimal defaultValue) {

            return Cast.DefaultIfNull(ToDecimal(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static DateTime ToDateTime(object value, DateTime defaultValue) {

            return Cast.DefaultIfNull(ToDateTime(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static TimeSpan ToTimeSpan(object value, TimeSpan defaultValue) {

            return Cast.DefaultIfNull(ToTimeSpan(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Guid ToGuid(object value, Guid defaultValue) {

            return Cast.DefaultIfNull(ToGuid(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="InvalidCastException">The text is not a type that can be cast to a String.</exception>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        public static Byte[] ToByteArray(object value, Byte[] defaultValue) {

            return Cast.DefaultIfNull(ToByteArray(value), defaultValue);

        }

        #endregion

        #region Public Shared Methods - FromString

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Boolean ToBoolean(String value, Boolean defaultValue) {

            return Cast.DefaultIfNull(ToBoolean(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Byte ToByte(String value, Byte defaultValue) {

            return Cast.DefaultIfNull(ToByte(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int16 ToInt16(String value, Int16 defaultValue) {

            return Cast.DefaultIfNull(ToInt16(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int32 ToInt32(String value, Int32 defaultValue) {

            return Cast.DefaultIfNull(ToInt32(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Int64 ToInt64(String value, Int64 defaultValue) {

            return Cast.DefaultIfNull(ToInt64(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Single ToSingle(String value, Single defaultValue) {

            return Cast.DefaultIfNull(ToSingle(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Double ToDouble(String value, Double defaultValue) {

            return Cast.DefaultIfNull(ToDouble(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Decimal ToDecimal(String value, Decimal defaultValue) {

            return Cast.DefaultIfNull(ToDecimal(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static DateTime ToDateTime(String value, DateTime defaultValue) {

            return Cast.DefaultIfNull(ToDateTime(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static TimeSpan ToTimeSpan(String value, TimeSpan defaultValue) {

            return Cast.DefaultIfNull(ToTimeSpan(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        /// <exception cref="OverflowException">The value is outside the acceptable range for the destination type.</exception>
        public static Guid ToGuid(String value, Guid defaultValue) {

            return Cast.DefaultIfNull(ToGuid(value), defaultValue);

        }

        /// <summary>
        /// Convert text to a strongly-typed value using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="defaultValue">The value to return if the text is null or empty.</param>
        /// <returns>The strongly-typed value, or the specified default if the text is null or empty.</returns>
        /// <exception cref="FormatException">The text is not in the expected format for the Sirius legacy data type.</exception>
        public static Byte[] ToByteArray(String value, Byte[] defaultValue) {

            return Cast.DefaultIfNull(ToByteArray(value), defaultValue);

        }

        #endregion

        #region Public Shared Methods - ToString (Nullable)

        /// <overloads>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </overloads>
        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or <see langword="null"/> if the value is <see langword="null"/>.</returns>
        public static String ToString(Boolean? value) {

            if(!value.HasValue) {
                return null;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or <see langword="null"/> if the value is <see langword="null"/>.</returns>
        public static String ToString(Byte? value) {

            if(!value.HasValue) {
                return null;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or <see langword="null"/> if the value is <see langword="null"/>.</returns>
        public static String ToString(Int16? value) {

            if(!value.HasValue) {
                return null;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or <see langword="null"/> if the value is <see langword="null"/>.</returns>
        public static String ToString(Int32? value) {

            if(!value.HasValue) {
                return null;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or <see langword="null"/> if the value is <see langword="null"/>.</returns>
        public static String ToString(Int64? value) {

            if(!value.HasValue) {
                return null;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or <see langword="null"/> if the value is <see langword="null"/>.</returns>
        public static String ToString(Single? value) {

            if(!value.HasValue) {
                return null;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or <see langword="null"/> if the value is <see langword="null"/>.</returns>
        public static String ToString(Double? value) {

            if(!value.HasValue) {
                return null;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or <see langword="null"/> if the value is <see langword="null"/>.</returns>
        public static String ToString(Decimal? value) {

            if(!value.HasValue) {
                return null;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or <see langword="null"/> if the value is <see langword="null"/>.</returns>
        public static String ToString(DateTime? value) {

            if(!value.HasValue) {
                return null;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or <see langword="null"/> if the value is <see langword="null"/>.</returns>
        public static String ToString(TimeSpan? value) {

            if(!value.HasValue) {
                return null;
            } else {
                return ToString(value.Value);
            }

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent, or <see langword="null"/> if the value is <see langword="null"/>.</returns>
        public static String ToString(Guid? value) {

            if(!value.HasValue) {
                return null;
            } else {
                return ToString(value.Value);
            }

        }

        #endregion

        #region Public Shared Methods - ToString

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Boolean value) {

            return SiriusLPFConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Byte value) {

            return SiriusLPFConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Int16 value) {

            return SiriusLPFConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Int32 value) {

            return SiriusLPFConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Int64 value) {

            return SiriusLPFConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Single value) {

            return SiriusLPFConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Double value) {

            return SiriusLPFConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Decimal value) {

            return SiriusLPFConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(DateTime value) {

            return SiriusLPFConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(TimeSpan value) {

            return SiriusLPFConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Guid value) {

            return SiriusLPFConvert.ToString(value);

        }

        /// <summary>
        /// Convert a strongly-typed value to text using the Sirius legacy data type formatting rules.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Byte[] value) {

            return SiriusLPFConvert.ToString(value);

        }

        #endregion
    }
}
