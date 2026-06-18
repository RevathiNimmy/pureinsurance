using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Sirius.Architecture.Configuration.Local {

    /// <summary>
    /// Win32 API declarations for use in RegistryKey4 code.
    /// </summary>
    internal static class Win32Native {

        #region Types

        [StructLayout(LayoutKind.Sequential)]
        internal class SECURITY_ATTRIBUTES {
            internal int nLength;
            internal byte[] pSecurityDescriptor;
            internal int bInheritHandle;
        }

        #endregion

        #region External Methods

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        internal static extern int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, IntPtr va_list_arguments);

        [SuppressUnmanagedCodeSecurity, ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport("advapi32.dll")]
        internal static extern int RegCloseKey(IntPtr hKey);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        internal static extern int RegCreateKeyEx(SafeRegistryHandle4 hKey, string lpSubKey, int Reserved, string lpClass, int dwOptions, int samDesired, SECURITY_ATTRIBUTES lpSecurityAttributes, out SafeRegistryHandle4 hkResult, out int lpdwDisposition);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        internal static extern int RegDeleteKey(SafeRegistryHandle4 hKey, string lpSubKey);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        internal static extern int RegDeleteKeyEx(SafeRegistryHandle4 hKey, string lpSubKey, int samDesired, int Reserved);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        internal static extern int RegEnumKeyEx(SafeRegistryHandle4 hKey, int dwIndex, StringBuilder lpName, out int lpcbName, int[] lpReserved, StringBuilder lpClass, int[] lpcbClass, long[] lpftLastWriteTime);

        [DllImport("advapi32.dll")]
        internal static extern int RegFlushKey(SafeRegistryHandle4 hKey);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        internal static extern int RegOpenKeyEx(SafeRegistryHandle4 hKey, string lpSubKey, int ulOptions, int samDesired, out SafeRegistryHandle4 hkResult);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        internal static extern int RegQueryInfoKey(SafeRegistryHandle4 hKey, StringBuilder lpClass, int[] lpcbClass, IntPtr lpReserved_MustBeZero, ref int lpcSubKeys, int[] lpcbMaxSubKeyLen, int[] lpcbMaxClassLen, ref int lpcValues, int[] lpcbMaxValueNameLen, int[] lpcbMaxValueLen, int[] lpcbSecurityDescriptor, int[] lpftLastWriteTime);

        #endregion

        #region Methods

        internal static string GetMessage(int errorCode) {
            StringBuilder lpBuffer = new StringBuilder(0x200);
            if(FormatMessage(0x3200, IntPtr.Zero, errorCode, 0, lpBuffer, lpBuffer.Capacity, IntPtr.Zero) != 0) {
                return lpBuffer.ToString();
            }
            return string.Format(Properties.Resources.UnknownError_Num, errorCode);
        }

        #endregion
    }
}
