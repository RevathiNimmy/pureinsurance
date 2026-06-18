using Microsoft.Win32;
using Sirius.Architecture.Utility;
using System;

namespace Sirius.Architecture.Configuration.Local {

    /// <summary>
    /// This class writes strongly-typed values to the Windows registry on the local machine.
    /// </summary>
    internal static class SwiftRegistryWrite {

        #region Public Methods - SetValue

        public static void SetValueAsBoolean(RegistryKey key, string name, Boolean value) {

            key.SetValue(name, BooleanDataConvert.ToInt32(value), RegistryValueKind.DWord);

        }

        public static void SetValueAsByte(RegistryKey key, string name, Byte value) {

            key.SetValue(name, value, RegistryValueKind.DWord);

        }

        public static void SetValueAsInt16(RegistryKey key, string name, Int16 value) {

            key.SetValue(name, value, RegistryValueKind.DWord);

        }

        public static void SetValueAsInt32(RegistryKey key, string name, Int32 value) {

            key.SetValue(name, value, RegistryValueKind.DWord);

        }

        public static void SetValueAsInt64(RegistryKey key, string name, Int64 value) {

            key.SetValue(name, value, RegistryValueKind.QWord);

        }

        public static void SetValueAsSingle(RegistryKey key, string name, Single value) {

            key.SetValue(name, SwiftLPFConvert.ToString(value), RegistryValueKind.String);

        }

        public static void SetValueAsDouble(RegistryKey key, string name, Double value) {

            key.SetValue(name, SwiftLPFConvert.ToString(value), RegistryValueKind.String);

        }

        public static void SetValueAsDecimal(RegistryKey key, string name, Decimal value) {

            key.SetValue(name, SwiftLPFConvert.ToString(value), RegistryValueKind.String);

        }

        public static void SetValueAsDateTime(RegistryKey key, string name, DateTime value) {

            key.SetValue(name, SwiftLPFConvert.ToString(value), RegistryValueKind.String);

        }

        public static void SetValueAsTimeSpan(RegistryKey key, string name, TimeSpan value) {

            key.SetValue(name, SwiftLPFConvert.ToString(value), RegistryValueKind.String);

        }

        public static void SetValueAsGuid(RegistryKey key, string name, Guid value) {

            key.SetValue(name, value.ToByteArray(), RegistryValueKind.Binary);

        }

        public static void SetValueAsByteArray(RegistryKey key, string name, Byte[] value) {

            key.SetValue(name, value, RegistryValueKind.Binary);

        }

        public static void SetValueAsString(RegistryKey key, string name, String value) {

            key.SetValue(name, value, RegistryValueKind.String);

        }

        #endregion

    }

}
