using Microsoft.Win32;
using System;

namespace Sirius.Architecture.Configuration.Local {

    /// <summary>
    /// This class reads and writes strongly-typed values to the Windows registry on the local machine
    /// in a way that is compatible with Sirius VB6 code.
    /// All data types are stored as String using the Sirius legacy persistence format.
    /// The values are not cached in memory, so they are always accurate.
    /// Use the product-specific config component provided by your team leader (if available) in preference to this class.
    /// </summary>
    /// <remarks>
    /// If your code is running in a 64-bit process, be aware that these methods access the 32-bit registry view
    /// for compatibility with VB6 code. There should be no need to access the 64-bit registry view for configuration
    /// settings, because new .NET-specific settings should not be stored in the registry. However, if 64-bit registry
    /// access is required for any reason, then open the key yourself and use the method overloads that take a key
    /// rather than a hive + key name.
    /// </remarks>
    public static class SiriusRegistryAccess {

        // The read and write classes both access the registry statelessly.
        // The access class does not need any caching because Windows already caches the registry itself.

        #region Public Methods - GetValue (Stateless)

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static Boolean GetValueAsBoolean(RegistryKey hive, string keyName, string name, Boolean defaultValue) {

            using(RegistryKey key = hive.OpenSubKey32(keyName)) {
                return GetValueAsBoolean(key, name, defaultValue);
            }

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static Byte GetValueAsByte(RegistryKey hive, string keyName, string name, Byte defaultValue) {

            using(RegistryKey key = hive.OpenSubKey32(keyName)) {
                return GetValueAsByte(key, name, defaultValue);
            }

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static Int16 GetValueAsInt16(RegistryKey hive, string keyName, string name, Int16 defaultValue) {

            using(RegistryKey key = hive.OpenSubKey32(keyName)) {
                return GetValueAsInt16(key, name, defaultValue);
            }

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static Int32 GetValueAsInt32(RegistryKey hive, string keyName, string name, Int32 defaultValue) {

            using(RegistryKey key = hive.OpenSubKey32(keyName)) {
                return GetValueAsInt32(key, name, defaultValue);
            }

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static Int64 GetValueAsInt64(RegistryKey hive, string keyName, string name, Int64 defaultValue) {

            using(RegistryKey key = hive.OpenSubKey32(keyName)) {
                return GetValueAsInt64(key, name, defaultValue);
            }

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static Single GetValueAsSingle(RegistryKey hive, string keyName, string name, Single defaultValue) {

            using(RegistryKey key = hive.OpenSubKey32(keyName)) {
                return GetValueAsSingle(key, name, defaultValue);
            }

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static Double GetValueAsDouble(RegistryKey hive, string keyName, string name, Double defaultValue) {

            using(RegistryKey key = hive.OpenSubKey32(keyName)) {
                return GetValueAsDouble(key, name, defaultValue);
            }

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static Decimal GetValueAsDecimal(RegistryKey hive, string keyName, string name, Decimal defaultValue) {

            using(RegistryKey key = hive.OpenSubKey32(keyName)) {
                return GetValueAsDecimal(key, name, defaultValue);
            }

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static DateTime GetValueAsDateTime(RegistryKey hive, string keyName, string name, DateTime defaultValue) {

            using(RegistryKey key = hive.OpenSubKey32(keyName)) {
                return GetValueAsDateTime(key, name, defaultValue);
            }

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static TimeSpan GetValueAsTimeSpan(RegistryKey hive, string keyName, string name, TimeSpan defaultValue) {

            using(RegistryKey key = hive.OpenSubKey32(keyName)) {
                return GetValueAsTimeSpan(key, name, defaultValue);
            }

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static Guid GetValueAsGuid(RegistryKey hive, string keyName, string name, Guid defaultValue) {

            using(RegistryKey key = hive.OpenSubKey32(keyName)) {
                return GetValueAsGuid(key, name, defaultValue);
            }

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static Byte[] GetValueAsByteArray(RegistryKey hive, string keyName, string name, Byte[] defaultValue) {

            using(RegistryKey key = hive.OpenSubKey32(keyName)) {
                return GetValueAsByteArray(key, name, defaultValue);
            }

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static String GetValueAsString(RegistryKey hive, string keyName, string name, String defaultValue) {

            using(RegistryKey key = hive.OpenSubKey32(keyName)) {
                return GetValueAsString(key, name, defaultValue);
            }

        }

        #endregion

        #region Public Methods - GetValue (Stateful)

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for reading.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static Boolean GetValueAsBoolean(RegistryKey key, string name, Boolean defaultValue) {

            return SiriusRegistryRead.GetValueAsBoolean(key, name, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for reading.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static Byte GetValueAsByte(RegistryKey key, string name, Byte defaultValue) {

            return SiriusRegistryRead.GetValueAsByte(key, name, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for reading.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static Int16 GetValueAsInt16(RegistryKey key, string name, Int16 defaultValue) {

            return SiriusRegistryRead.GetValueAsInt16(key, name, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for reading.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static Int32 GetValueAsInt32(RegistryKey key, string name, Int32 defaultValue) {

            return SiriusRegistryRead.GetValueAsInt32(key, name, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for reading.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static Int64 GetValueAsInt64(RegistryKey key, string name, Int64 defaultValue) {

            return SiriusRegistryRead.GetValueAsInt64(key, name, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for reading.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static Single GetValueAsSingle(RegistryKey key, string name, Single defaultValue) {

            return SiriusRegistryRead.GetValueAsSingle(key, name, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for reading.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static Double GetValueAsDouble(RegistryKey key, string name, Double defaultValue) {

            return SiriusRegistryRead.GetValueAsDouble(key, name, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for reading.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static Decimal GetValueAsDecimal(RegistryKey key, string name, Decimal defaultValue) {

            return SiriusRegistryRead.GetValueAsDecimal(key, name, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for reading.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static DateTime GetValueAsDateTime(RegistryKey key, string name, DateTime defaultValue) {

            return SiriusRegistryRead.GetValueAsDateTime(key, name, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for reading.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static TimeSpan GetValueAsTimeSpan(RegistryKey key, string name, TimeSpan defaultValue) {

            return SiriusRegistryRead.GetValueAsTimeSpan(key, name, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for reading.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static Guid GetValueAsGuid(RegistryKey key, string name, Guid defaultValue) {

            return SiriusRegistryRead.GetValueAsGuid(key, name, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for reading.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static Byte[] GetValueAsByteArray(RegistryKey key, string name, Byte[] defaultValue) {

            return SiriusRegistryRead.GetValueAsByteArray(key, name, defaultValue);

        }

        /// <summary>
        /// Read a strongly-typed value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for reading.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="defaultValue">The default value to return if the registry value does not exist.</param>
        /// <returns>The registry value if it exists, otherwise the default value.</returns>
        public static String GetValueAsString(RegistryKey key, string name, String defaultValue) {

            return SiriusRegistryRead.GetValueAsString(key, name, defaultValue);

        }

        #endregion

        #region Public Methods - SetValue (Stateless)

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsBoolean(RegistryKey hive, string keyName, string name, Boolean value) {

            using(RegistryKey key = hive.CreateSubKey32(keyName)) {
                SetValueAsBoolean(key, name, value);
            }

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsByte(RegistryKey hive, string keyName, string name, Byte value) {

            using(RegistryKey key = hive.CreateSubKey32(keyName)) {
                SetValueAsByte(key, name, value);
            }

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsInt16(RegistryKey hive, string keyName, string name, Int16 value) {

            using(RegistryKey key = hive.CreateSubKey32(keyName)) {
                SetValueAsInt16(key, name, value);
            }

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsInt32(RegistryKey hive, string keyName, string name, Int32 value) {

            using(RegistryKey key = hive.CreateSubKey32(keyName)) {
                SetValueAsInt32(key, name, value);
            }

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsInt64(RegistryKey hive, string keyName, string name, Int64 value) {

            using(RegistryKey key = hive.CreateSubKey32(keyName)) {
                SetValueAsInt64(key, name, value);
            }

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsSingle(RegistryKey hive, string keyName, string name, Single value) {

            using(RegistryKey key = hive.CreateSubKey32(keyName)) {
                SetValueAsSingle(key, name, value);
            }

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsDouble(RegistryKey hive, string keyName, string name, Double value) {

            using(RegistryKey key = hive.CreateSubKey32(keyName)) {
                SetValueAsDouble(key, name, value);
            }

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsDecimal(RegistryKey hive, string keyName, string name, Decimal value) {

            using(RegistryKey key = hive.CreateSubKey32(keyName)) {
                SetValueAsDecimal(key, name, value);
            }

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsDateTime(RegistryKey hive, string keyName, string name, DateTime value) {

            using(RegistryKey key = hive.CreateSubKey32(keyName)) {
                SetValueAsDateTime(key, name, value);
            }

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsTimeSpan(RegistryKey hive, string keyName, string name, TimeSpan value) {

            using(RegistryKey key = hive.CreateSubKey32(keyName)) {
                SetValueAsTimeSpan(key, name, value);
            }

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsGuid(RegistryKey hive, string keyName, string name, Guid value) {

            using(RegistryKey key = hive.CreateSubKey32(keyName)) {
                SetValueAsGuid(key, name, value);
            }

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsByteArray(RegistryKey hive, string keyName, string name, Byte[] value) {

            using(RegistryKey key = hive.CreateSubKey32(keyName)) {
                SetValueAsByteArray(key, name, value);
            }

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsString(RegistryKey hive, string keyName, string name, String value) {

            using(RegistryKey key = hive.CreateSubKey32(keyName)) {
                SetValueAsString(key, name, value);
            }

        }

        #endregion

        #region Public Methods - SetValue (Stateful)

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for writing.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsBoolean(RegistryKey key, string name, Boolean value) {

            SiriusRegistryWrite.SetValueAsBoolean(key, name, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for writing.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsByte(RegistryKey key, string name, Byte value) {

            SiriusRegistryWrite.SetValueAsByte(key, name, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for writing.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsInt16(RegistryKey key, string name, Int16 value) {

            SiriusRegistryWrite.SetValueAsInt16(key, name, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for writing.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsInt32(RegistryKey key, string name, Int32 value) {

            SiriusRegistryWrite.SetValueAsInt32(key, name, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for writing.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsInt64(RegistryKey key, string name, Int64 value) {

            SiriusRegistryWrite.SetValueAsInt64(key, name, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for writing.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsSingle(RegistryKey key, string name, Single value) {

            SiriusRegistryWrite.SetValueAsSingle(key, name, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for writing.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsDouble(RegistryKey key, string name, Double value) {

            SiriusRegistryWrite.SetValueAsDouble(key, name, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for writing.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsDecimal(RegistryKey key, string name, Decimal value) {

            SiriusRegistryWrite.SetValueAsDecimal(key, name, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for writing.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsDateTime(RegistryKey key, string name, DateTime value) {

            SiriusRegistryWrite.SetValueAsDateTime(key, name, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for writing.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsTimeSpan(RegistryKey key, string name, TimeSpan value) {

            SiriusRegistryWrite.SetValueAsTimeSpan(key, name, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for writing.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsGuid(RegistryKey key, string name, Guid value) {

            SiriusRegistryWrite.SetValueAsGuid(key, name, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for writing.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsByteArray(RegistryKey key, string name, Byte[] value) {

            SiriusRegistryWrite.SetValueAsByteArray(key, name, value);

        }

        /// <summary>
        /// Write a strongly-typed value to the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for writing.</param>
        /// <param name="name">The name of the registry value to access.</param>
        /// <param name="value">The value to write.</param>
        public static void SetValueAsString(RegistryKey key, string name, String value) {

            SiriusRegistryWrite.SetValueAsString(key, name, value);

        }

        #endregion

        #region Public Methods - DeleteValue (Stateless)

        /// <summary>
        /// Delete a value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="hive">The top-level registry hive to access.</param>
        /// <param name="keyName">The full path of the registry key to access.</param>
        /// <param name="name">The name of the registry value to delete.</param>
        public static void DeleteValue(RegistryKey hive, string keyName, string name) {

            using(RegistryKey key = hive.OpenSubKey32(keyName, true)) {
                DeleteValue(key, name);
            }

        }

        #endregion

        #region Public Methods - DeleteValue (Stateful)

        /// <summary>
        /// Delete a value from the Windows registry on the local machine.
        /// </summary>
        /// <param name="key">The registry key to access. This key must already be open for writing.</param>
        /// <param name="name">The name of the registry value to delete.</param>
        public static void DeleteValue(RegistryKey key, string name) {

            if(key != null) {
                key.DeleteValue(name, false);
            }

        }

        #endregion

    }

}
