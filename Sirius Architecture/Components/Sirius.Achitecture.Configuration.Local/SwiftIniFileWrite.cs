using Sirius.Architecture.Utility;
using System;
using System.Runtime.InteropServices;

namespace Sirius.Architecture.Configuration.Local {

    /// <summary>
    /// This class writes strongly-typed values to an INI file on the local machine.
    /// </summary>
    internal static class SwiftIniFileWrite {

        #region Private P/Invoke Declarations

        private const string BlankName = "@";

        [DllImport("Kernel32.dll", EntryPoint = "WritePrivateProfileStringA")]
        private static extern int WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        #endregion

        #region Public Methods - SetValue

        public static void SetValueAsBoolean(string fileName, string sectionName, string keyName, Boolean value) {

            SetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(value));

        }

        public static void SetValueAsByte(string fileName, string sectionName, string keyName, Byte value) {

            SetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(value));

        }

        public static void SetValueAsInt16(string fileName, string sectionName, string keyName, Int16 value) {

            SetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(value));

        }

        public static void SetValueAsInt32(string fileName, string sectionName, string keyName, Int32 value) {

            SetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(value));

        }

        public static void SetValueAsInt64(string fileName, string sectionName, string keyName, Int64 value) {

            SetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(value));

        }

        public static void SetValueAsSingle(string fileName, string sectionName, string keyName, Single value) {

            SetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(value));

        }

        public static void SetValueAsDouble(string fileName, string sectionName, string keyName, Double value) {

            SetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(value));

        }

        public static void SetValueAsDecimal(string fileName, string sectionName, string keyName, Decimal value) {

            SetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(value));

        }

        public static void SetValueAsDateTime(string fileName, string sectionName, string keyName, DateTime value) {

            SetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(value));

        }

        public static void SetValueAsTimeSpan(string fileName, string sectionName, string keyName, TimeSpan value) {

            SetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(value));

        }

        public static void SetValueAsGuid(string fileName, string sectionName, string keyName, Guid value) {

            SetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(value));

        }

        public static void SetValueAsByteArray(string fileName, string sectionName, string keyName, Byte[] value) {

            SetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(value));

        }

        public static void SetValueAsString(string fileName, string sectionName, string keyName, String value) {

            // Safety checks - make sure that the names are always valid. If they are null or blank,
            // then the API function has different behaviour which we don't want to see.
            if(string.IsNullOrEmpty(fileName)) {
                throw new ArgumentException(Properties.Resources.IniFileNameBlankException, "fileName");
            }
            if(string.IsNullOrEmpty(sectionName)) {
                throw new ArgumentException(Properties.Resources.IniSectionNameBlankException, "sectionName");
            }
            if(string.IsNullOrEmpty(keyName)) {
                keyName = BlankName;
            }

            WritePrivateProfileString(sectionName, keyName, value, fileName);

        }

        #endregion

    }

}
