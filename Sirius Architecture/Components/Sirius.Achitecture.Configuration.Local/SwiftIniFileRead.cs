using Sirius.Architecture.Utility;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Sirius.Architecture.Configuration.Local {

    /// <summary>
    /// This class reads strongly-typed values from an INI file on the local machine.
    /// </summary>
    internal static class SwiftIniFileRead {

        #region Private P/Invoke Declarations

        private const string BlankName = "@";

        [DllImport("Kernel32.dll", EntryPoint = "GetPrivateProfileStringA")]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        #endregion

        #region Public Methods - GetValue

        public static Boolean GetValueAsBoolean(string fileName, string sectionName, string keyName, Boolean defaultValue) {

            return SwiftLPFConvert.ToBoolean(GetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(defaultValue)));

        }

        public static Byte GetValueAsByte(string fileName, string sectionName, string keyName, Byte defaultValue) {

            return SwiftLPFConvert.ToByte(GetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(defaultValue)));

        }

        public static Int16 GetValueAsInt16(string fileName, string sectionName, string keyName, Int16 defaultValue) {

            return SwiftLPFConvert.ToInt16(GetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(defaultValue)));

        }

        public static Int32 GetValueAsInt32(string fileName, string sectionName, string keyName, Int32 defaultValue) {

            return SwiftLPFConvert.ToInt32(GetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(defaultValue)));

        }

        public static Int64 GetValueAsInt64(string fileName, string sectionName, string keyName, Int64 defaultValue) {

            return SwiftLPFConvert.ToInt64(GetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(defaultValue)));

        }

        public static Single GetValueAsSingle(string fileName, string sectionName, string keyName, Single defaultValue) {

            return SwiftLPFConvert.ToSingle(GetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(defaultValue)));

        }

        public static Double GetValueAsDouble(string fileName, string sectionName, string keyName, Double defaultValue) {

            return SwiftLPFConvert.ToDouble(GetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(defaultValue)));

        }

        public static Decimal GetValueAsDecimal(string fileName, string sectionName, string keyName, Decimal defaultValue) {

            return SwiftLPFConvert.ToDecimal(GetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(defaultValue)));

        }

        public static DateTime GetValueAsDateTime(string fileName, string sectionName, string keyName, DateTime defaultValue) {

            return SwiftLPFConvert.ToDateTime(GetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(defaultValue)));

        }

        public static TimeSpan GetValueAsTimeSpan(string fileName, string sectionName, string keyName, TimeSpan defaultValue) {

            return SwiftLPFConvert.ToTimeSpan(GetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(defaultValue)));

        }

        public static Guid GetValueAsGuid(string fileName, string sectionName, string keyName, Guid defaultValue) {

            return SwiftLPFConvert.ToGuid(GetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(defaultValue)));

        }

        public static Byte[] GetValueAsByteArray(string fileName, string sectionName, string keyName, Byte[] defaultValue) {

            return SwiftLPFConvert.ToByteArray(GetValueAsString(fileName, sectionName, keyName, SwiftLPFConvert.ToString(defaultValue)));

        }

        public static String GetValueAsString(string fileName, string sectionName, string keyName, String defaultValue) {

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

            StringBuilder returnValue = new StringBuilder(255);
            int bytesCopied = GetPrivateProfileString(sectionName, keyName, defaultValue, returnValue, returnValue.Capacity + 1, fileName);

            // Different versions of Windows cannot be relied on to return consistent or useful data in bytesCopied.
            // We must compensate for this here.
            if(bytesCopied == 0) {
                // Deduce our own value and use it.
                bytesCopied = returnValue.ToString().IndexOf('\0');
                if(bytesCopied > -1) {
                    returnValue = returnValue.Remove(bytesCopied, returnValue.Length - bytesCopied);
                }
            } else if(bytesCopied < returnValue.Length) {
                // Believe the returned value and use that.
                returnValue = returnValue.Remove(bytesCopied, returnValue.Length - bytesCopied);
            } else {
                // Leave the string alone!
            }

            return returnValue.ToString();

        }

        #endregion

    }

}
