using System;

namespace Sirius.Architecture.Configuration.Local {

    /// <summary>
    /// This class reads and writes strongly-typed values to an INI file on the local machine
    /// in a way that is compatible with Swift VB6 code.
    /// All data types are stored as String using the Swift legacy persistence format.
    /// The values are not cached in memory, so they are always accurate.
    /// Use the product-specific config component provided by your team leader (if available) in preference to this class.
    /// </summary>
    public static class SwiftIniFileAccess {

        // The read and write classes both access the registry statelessly.
        // The access class does not use caching yet (I will implement this later).

        #region Public Methods - GetValue

        /// <summary>
        /// Read a strongly-typed value from an INI file on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="defaultValue">The default value to return if the INI value does not exist.</param>
        /// <returns>The INI value if it exists, otherwise the default value.</returns>
        public static Boolean GetValueAsBoolean(string fileName, string sectionName, string keyName, Boolean defaultValue) {

            return SwiftIniFileRead.GetValueAsBoolean(fileName, sectionName, keyName, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from an INI file on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="defaultValue">The default value to return if the INI value does not exist.</param>
        /// <returns>The INI value if it exists, otherwise the default value.</returns>
        public static Byte GetValueAsByte(string fileName, string sectionName, string keyName, Byte defaultValue) {

            return SwiftIniFileRead.GetValueAsByte(fileName, sectionName, keyName, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from an INI file on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="defaultValue">The default value to return if the INI value does not exist.</param>
        /// <returns>The INI value if it exists, otherwise the default value.</returns>
        public static Int16 GetValueAsInt16(string fileName, string sectionName, string keyName, Int16 defaultValue) {

            return SwiftIniFileRead.GetValueAsInt16(fileName, sectionName, keyName, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from an INI file on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="defaultValue">The default value to return if the INI value does not exist.</param>
        /// <returns>The INI value if it exists, otherwise the default value.</returns>
        public static Int32 GetValueAsInt32(string fileName, string sectionName, string keyName, Int32 defaultValue) {

            return SwiftIniFileRead.GetValueAsInt32(fileName, sectionName, keyName, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from an INI file on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="defaultValue">The default value to return if the INI value does not exist.</param>
        /// <returns>The INI value if it exists, otherwise the default value.</returns>
        public static Int64 GetValueAsInt64(string fileName, string sectionName, string keyName, Int64 defaultValue) {

            return SwiftIniFileRead.GetValueAsInt64(fileName, sectionName, keyName, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from an INI file on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="defaultValue">The default value to return if the INI value does not exist.</param>
        /// <returns>The INI value if it exists, otherwise the default value.</returns>
        public static Single GetValueAsSingle(string fileName, string sectionName, string keyName, Single defaultValue) {

            return SwiftIniFileRead.GetValueAsSingle(fileName, sectionName, keyName, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from an INI file on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="defaultValue">The default value to return if the INI value does not exist.</param>
        /// <returns>The INI value if it exists, otherwise the default value.</returns>
        public static Double GetValueAsDouble(string fileName, string sectionName, string keyName, Double defaultValue) {

            return SwiftIniFileRead.GetValueAsDouble(fileName, sectionName, keyName, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from an INI file on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="defaultValue">The default value to return if the INI value does not exist.</param>
        /// <returns>The INI value if it exists, otherwise the default value.</returns>
        public static Decimal GetValueAsDecimal(string fileName, string sectionName, string keyName, Decimal defaultValue) {

            return SwiftIniFileRead.GetValueAsDecimal(fileName, sectionName, keyName, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from an INI file on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="defaultValue">The default value to return if the INI value does not exist.</param>
        /// <returns>The INI value if it exists, otherwise the default value.</returns>
        public static DateTime GetValueAsDateTime(string fileName, string sectionName, string keyName, DateTime defaultValue) {

            return SwiftIniFileRead.GetValueAsDateTime(fileName, sectionName, keyName, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from an INI file on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="defaultValue">The default value to return if the INI value does not exist.</param>
        /// <returns>The INI value if it exists, otherwise the default value.</returns>
        public static TimeSpan GetValueAsTimeSpan(string fileName, string sectionName, string keyName, TimeSpan defaultValue) {

            return SwiftIniFileRead.GetValueAsTimeSpan(fileName, sectionName, keyName, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from an INI file on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="defaultValue">The default value to return if the INI value does not exist.</param>
        /// <returns>The INI value if it exists, otherwise the default value.</returns>
        public static Guid GetValueAsGuid(string fileName, string sectionName, string keyName, Guid defaultValue) {

            return SwiftIniFileRead.GetValueAsGuid(fileName, sectionName, keyName, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from an INI file on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="defaultValue">The default value to return if the INI value does not exist.</param>
        /// <returns>The INI value if it exists, otherwise the default value.</returns>
        public static Byte[] GetValueAsByteArray(string fileName, string sectionName, string keyName, Byte[] defaultValue) {

            return SwiftIniFileRead.GetValueAsByteArray(fileName, sectionName, keyName, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from an INI file on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="defaultValue">The default value to return if the INI value does not exist.</param>
        /// <returns>The INI value if it exists, otherwise the default value.</returns>
        public static String GetValueAsString(string fileName, string sectionName, string keyName, String defaultValue) {

            return SwiftIniFileRead.GetValueAsString(fileName, sectionName, keyName, defaultValue);

        }

        #endregion

        #region Public Methods - SetValue

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsBoolean(string fileName, string sectionName, string keyName, Boolean value) {

            SwiftIniFileWrite.SetValueAsBoolean(fileName, sectionName, keyName, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsByte(string fileName, string sectionName, string keyName, Byte value) {

            SwiftIniFileWrite.SetValueAsByte(fileName, sectionName, keyName, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsInt16(string fileName, string sectionName, string keyName, Int16 value) {

            SwiftIniFileWrite.SetValueAsInt16(fileName, sectionName, keyName, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsInt32(string fileName, string sectionName, string keyName, Int32 value) {

            SwiftIniFileWrite.SetValueAsInt32(fileName, sectionName, keyName, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsInt64(string fileName, string sectionName, string keyName, Int64 value) {

            SwiftIniFileWrite.SetValueAsInt64(fileName, sectionName, keyName, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsSingle(string fileName, string sectionName, string keyName, Single value) {

            SwiftIniFileWrite.SetValueAsSingle(fileName, sectionName, keyName, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsDouble(string fileName, string sectionName, string keyName, Double value) {

            SwiftIniFileWrite.SetValueAsDouble(fileName, sectionName, keyName, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsDecimal(string fileName, string sectionName, string keyName, Decimal value) {

            SwiftIniFileWrite.SetValueAsDecimal(fileName, sectionName, keyName, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsDateTime(string fileName, string sectionName, string keyName, DateTime value) {

            SwiftIniFileWrite.SetValueAsDateTime(fileName, sectionName, keyName, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsTimeSpan(string fileName, string sectionName, string keyName, TimeSpan value) {

            SwiftIniFileWrite.SetValueAsTimeSpan(fileName, sectionName, keyName, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsGuid(string fileName, string sectionName, string keyName, Guid value) {

            SwiftIniFileWrite.SetValueAsGuid(fileName, sectionName, keyName, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsByteArray(string fileName, string sectionName, string keyName, Byte[] value) {

            SwiftIniFileWrite.SetValueAsByteArray(fileName, sectionName, keyName, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="fileName">The INI file name. If the path is not specified, the Windows directory will be assumed.</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="keyName">Key name</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsString(string fileName, string sectionName, string keyName, String value) {

            SwiftIniFileWrite.SetValueAsString(fileName, sectionName, keyName, value);

        }

        #endregion

    }

}
