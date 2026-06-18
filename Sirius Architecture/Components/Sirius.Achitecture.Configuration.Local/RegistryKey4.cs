using Microsoft.Win32;
using Sirius.Architecture.Utility;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;

namespace Sirius.Architecture.Configuration.Local {

    /// <summary>
    /// Additional <see cref="RegistryKey"/> methods that give access to the 32-bit registry view for 64-bit processes.
    /// </summary>
    public static class RegistryKey4 {

        // Large chunks of this code have been reverse-engineered from the mscorlib 4.0 assembly,
        // then modified just enough to get it to compile in both the 2.0 and 4.0 CLRs. Reflection
        // is used to (a) simulate 4.0 features that do not exist in 2.0, and (b) access 2.0 features
        // that are not exposed publicly. Reflection code is encapsulated in various helper classes
        // to reduce complexity inside methods.

        #region Private Simulated Instance Constructor

        private static RegistryKey NewRegistryKey(SafeRegistryHandle4 hkey, bool writable, bool systemKey, bool remoteKey, bool isPerfData, RegistryView4 view) {
            // Create a new real SafeRegistryHandle object with the same handle as the fake one,
            // take ownership of it, then remove ownership from the fake handle, so that it will
            // only get closed when the RegistryKey that owns it is disposed.
            var hkeyReal = SafeRegistryHandleInfo.Constructor.Invoke(new object[] { hkey.DangerousGetHandle(), true });
            hkey.SetHandleAsInvalid();
            // Create a new RegistryKey object with all the correct data.
            if(RegistryKeyInfo.Constructor5 != null) {
                return (RegistryKey) RegistryKeyInfo.Constructor5.Invoke(new object[] { hkeyReal, writable, systemKey, remoteKey, isPerfData });
            } else if(RegistryKeyInfo.Constructor6 != null) {
                return (RegistryKey) RegistryKeyInfo.Constructor6.Invoke(new object[] { hkeyReal, writable, systemKey, remoteKey, isPerfData, Enum.ToObject(RegistryKeyInfo.Type, (int) view) });
            } else {
                throw new PlatformNotSupportedException(Properties.Resources.IncompatibleRuntime);
            }
        }

        #endregion

        #region Private Simulated Instance Fields

        private static SafeRegistryHandle4 get_hkey(this RegistryKey @this) {
            // Extract the internal handle and wrap it in a new fake object, but don't take ownership
            // of it. This ensures that the handle will only get closed when the RegistryKey that
            // owns it is disposed.
            var hkeyReal = (SafeHandle) RegistryKeyInfo.HKeyField.GetValue(@this);
            if(hkeyReal != null) {
                return new SafeRegistryHandle4(hkeyReal.DangerousGetHandle(), false);
            } else {
                return null;
            }
        }
        private static void clear_hkey(this RegistryKey @this) {
            // Close the internal handle owned by the RegistryKey. If we were to take a copy and
            // close that, then we would not be accomplishing anything.
            var hkeyReal = (SafeHandle) RegistryKeyInfo.HKeyField.GetValue(@this);
            hkeyReal.SetHandleAsInvalid();
            RegistryKeyInfo.HKeyField.SetValue(@this, null);
        }

        private static string get_keyName(this RegistryKey @this) {
            return (string) RegistryKeyInfo.KeyNameField.GetValue(@this);
        }
        private static void set_keyName(this RegistryKey @this, string value) {
            RegistryKeyInfo.KeyNameField.SetValue(@this, value);
        }
        private static bool get_remoteKey(this RegistryKey @this) {
            return (bool) RegistryKeyInfo.RemoteKeyField.GetValue(@this);
        }
        private static void set_remoteKey(this RegistryKey @this, bool value) {
            RegistryKeyInfo.RemoteKeyField.SetValue(@this, value);
        }
        private static RegistryKeyPermissionCheck get_checkMode(this RegistryKey @this) {
            return (RegistryKeyPermissionCheck) RegistryKeyInfo.CheckModeField.GetValue(@this);
        }
        private static void set_checkMode(this RegistryKey @this, RegistryKeyPermissionCheck value) {
            RegistryKeyInfo.CheckModeField.SetValue(@this, value);
        }
        private static int get_state(this RegistryKey @this) {
            return (int) RegistryKeyInfo.StateField.GetValue(@this);
        }
        private static void set_state(this RegistryKey @this, int value) {
            RegistryKeyInfo.StateField.SetValue(@this, value);
        }

        #endregion

        #region Public Simulated Instance Methods (Safe 32-Bit Only Access)

        /// <summary>
        /// Retrieves a subkey as read-only from the 32-bit registry view.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static RegistryKey OpenSubKey32(this RegistryKey @this, string name) {
            if(Force32BitView()) {
                return @this.OpenSubKey(name, RegistryView4.Registry32);
            } else {
                return @this.OpenSubKey(name);
            }
        }

        /// <summary>
        /// Retrieves a specified subkey from the 32-bit registry view, and specifies whether write access is be applied to the key.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="name"></param>
        /// <param name="writable"></param>
        /// <returns></returns>
        public static RegistryKey OpenSubKey32(this RegistryKey @this, string name, bool writable) {
            if(Force32BitView()) {
                return @this.OpenSubKey(name, writable, RegistryView4.Registry32);
            } else {
                return @this.OpenSubKey(name, writable);
            }
        }

        /// <summary>
        /// Creates a new subkey or opens an existing subkey in the 32-bit registry view for write access.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="subkey"></param>
        /// <returns></returns>
        public static RegistryKey CreateSubKey32(this RegistryKey @this, string subkey) {
            if(Force32BitView()) {
                return @this.CreateSubKey(subkey, RegistryView4.Registry32);
            } else {
                return @this.CreateSubKey(subkey);
            }
        }

        /// <summary>
        /// Deletes the specified subkey from the 32-bit registry view.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="subkey"></param>
        public static void DeleteSubKey32(this RegistryKey @this, string subkey) {
            if(Force32BitView()) {
                @this.DeleteSubKey(subkey, RegistryView4.Registry32);
            } else {
                @this.DeleteSubKey(subkey);
            }
        }

        /// <summary>
        /// Deletes the specified subkey from the 32-bit registry view, and specifies whether an exception is raised if the subkey is not found.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="subkey"></param>
        /// <param name="throwOnMissingSubKey"></param>
        public static void DeleteSubKey32(this RegistryKey @this, string subkey, bool throwOnMissingSubKey) {
            if(Force32BitView()) {
                @this.DeleteSubKey(subkey, throwOnMissingSubKey, RegistryView4.Registry32);
            } else {
                @this.DeleteSubKey(subkey, throwOnMissingSubKey);
            }
        }

        /// <summary>
        /// Deletes a subkey and any child subkeys recursively from the 32-bit registry view.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="subkey"></param>
        public static void DeleteSubKeyTree32(this RegistryKey @this, string subkey) {
            if(Force32BitView()) {
                @this.DeleteSubKeyTree(subkey, RegistryView4.Registry32);
            } else {
                @this.DeleteSubKeyTree(subkey);
            }
        }

        /// <summary>
        /// Deletes the specified subkey and any child subkeys recursively from the 32-bit registry view, and specifies whether an exception is raised if the subkey is not found.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="subkey"></param>
        /// <param name="throwOnMissingSubKey"></param>
        public static void DeleteSubKeyTree32(this RegistryKey @this, string subkey, bool throwOnMissingSubKey) {
            if(Force32BitView()) {
                @this.DeleteSubKeyTree(subkey, throwOnMissingSubKey, RegistryView4.Registry32);
            } else if(throwOnMissingSubKey) {
                @this.DeleteSubKeyTree(subkey);
            } else {
                // throwOnMissingSubKey = false not implemented in mscorlib 2.0, so we must simulate it.
                try { @this.DeleteSubKeyTree(subkey); } catch(ArgumentException) { }
            }
        }

        #endregion

        #region Private Simulated Instance Methods (With RegistryView Parameter)

        private static RegistryKey OpenSubKey(this RegistryKey @this, string name, RegistryView4 view) {
            return @this.OpenSubKey(name, false, view);
        }

        private static RegistryKey OpenSubKey(this RegistryKey @this, string name, RegistryKeyPermissionCheck permissionCheck, RegistryView4 view) {
            ValidateKeyMode(permissionCheck);
            return @this.InternalOpenSubKey(name, permissionCheck, GetRegistryKeyAccess(permissionCheck, view), view);
        }

        private static RegistryKey OpenSubKey(this RegistryKey @this, string name, bool writable, RegistryView4 view) {
            ValidateKeyName(name);
            @this.EnsureNotDisposed();
            name = FixupName(name);
            @this.CheckOpenSubKeyPermission(name, writable);
            SafeRegistryHandle4 hkResult = null;
            int errorCode = Win32Native.RegOpenKeyEx(@this.get_hkey(), name, 0, GetRegistryKeyAccess(writable, view), out hkResult);
            if((errorCode == 0) && !hkResult.IsInvalid) {
                var key = NewRegistryKey(hkResult, writable, false, @this.get_remoteKey(), false, view);
                @key.set_checkMode(@this.GetSubKeyPermissonCheck(writable));
                key.set_keyName(@this.get_keyName() + @"\" + name);
                return key;
            }
            if((errorCode == 5) || (errorCode == 0x542)) {
                throw new SecurityException(Properties.Resources.Security_RegistryPermission);
            }
            return null;
        }

        private static RegistryKey OpenSubKey(this RegistryKey @this, string name, RegistryKeyPermissionCheck permissionCheck, RegistryRights rights, RegistryView4 view) {
            return @this.InternalOpenSubKey(name, permissionCheck, (int) rights, view);
        }

        private static RegistryKey CreateSubKey(this RegistryKey @this, string subkey, RegistryView4 view) {
            return @this.CreateSubKey(subkey, @this.get_checkMode(), view);
        }

        private static RegistryKey CreateSubKey(this RegistryKey @this, string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryView4 view) {
            return @this.CreateSubKeyInternal(subkey, permissionCheck, null, RegistryOptions4.None, view);
        }

        private static RegistryKey CreateSubKey(this RegistryKey @this, string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions4 options, RegistryView4 view) {
            return @this.CreateSubKeyInternal(subkey, permissionCheck, null, options, view);
        }

        private static RegistryKey CreateSubKey(this RegistryKey @this, string subkey, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity, RegistryView4 view) {
            return @this.CreateSubKeyInternal(subkey, permissionCheck, registrySecurity, RegistryOptions4.None, view);
        }

        private static RegistryKey CreateSubKey(this RegistryKey @this, string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions4 registryOptions, RegistrySecurity registrySecurity, RegistryView4 view) {
            return @this.CreateSubKeyInternal(subkey, permissionCheck, registrySecurity, registryOptions, view);
        }

        private static void DeleteSubKey(this RegistryKey @this, string subkey, RegistryView4 view) {
            @this.DeleteSubKey(subkey, true, view);
        }

        private static void DeleteSubKey(this RegistryKey @this, string subkey, bool throwOnMissingSubKey, RegistryView4 view) {
            ValidateKeyName(subkey);
            @this.EnsureWriteable();
            subkey = FixupName(subkey);
            @this.CheckSubKeyWritePermission(subkey);
            RegistryKey key = @this.InternalOpenSubKey(subkey, false, view);
            if(key != null) {
                try {
                    if(key.InternalSubKeyCount() > 0) {
                        throw new InvalidOperationException(Properties.Resources.InvalidOperation_RegRemoveSubKey);
                    }
                } finally {
                    key.Close();
                }
                int errorCode;
                try {
                    errorCode = Win32Native.RegDeleteKeyEx(@this.get_hkey(), subkey, (int) view, 0);
                } catch(EntryPointNotFoundException) {
                    errorCode = Win32Native.RegDeleteKey(@this.get_hkey(), subkey);
                }
                switch(errorCode) {
                case 0:
                    return;
                case 2:
                    if(throwOnMissingSubKey) {
                        throw new ArgumentException(Properties.Resources.Arg_RegSubKeyAbsent);
                    }
                    return;
                }
                @this.Win32Error(errorCode, null);
            } else if(throwOnMissingSubKey) {
                throw new ArgumentException(Properties.Resources.Arg_RegSubKeyAbsent);
            }
        }

        private static void DeleteSubKeyTree(this RegistryKey @this, string subkey, RegistryView4 view) {
            @this.DeleteSubKeyTree(subkey, true, view);
        }

        private static void DeleteSubKeyTree(this RegistryKey @this, string subkey, bool throwOnMissingSubKey, RegistryView4 view) {
            ValidateKeyName(subkey);
            if((subkey.Length == 0) && @this.IsSystemKey()) {
                throw new ArgumentException(Properties.Resources.Arg_RegKeyDelHive);
            }
            @this.EnsureWriteable();
            subkey = FixupName(subkey);
            @this.CheckSubTreeWritePermission(subkey);
            RegistryKey key = @this.InternalOpenSubKey(subkey, true, view);
            if(key != null) {
                try {
                    if(key.InternalSubKeyCount() > 0) {
                        string[] subKeyNames = key.InternalGetSubKeyNames();
                        for(int i = 0; i < subKeyNames.Length; i++) {
                            key.DeleteSubKeyTreeInternal(subKeyNames[i], view);
                        }
                    }
                } finally {
                    key.Close();
                }
                int errorCode;
                try {
                    errorCode = Win32Native.RegDeleteKeyEx(@this.get_hkey(), subkey, (int) view, 0);
                } catch(EntryPointNotFoundException) {
                    errorCode = Win32Native.RegDeleteKey(@this.get_hkey(), subkey);
                }
                if(errorCode != 0) {
                    @this.Win32Error(errorCode, null);
                }
            } else if(throwOnMissingSubKey) {
                throw new ArgumentException(Properties.Resources.Arg_RegSubKeyAbsent);
            }
        }

        #endregion

        #region Private Simulated Instance Methods (Internal)

        private static RegistryKey InternalOpenSubKey(this RegistryKey @this, string name, bool writable, RegistryView4 view) {
            ValidateKeyName(name);
            @this.EnsureNotDisposed();
            SafeRegistryHandle4 hkResult = null;
            int errorCode = Win32Native.RegOpenKeyEx(@this.get_hkey(), name, 0, GetRegistryKeyAccess(writable, view), out hkResult);
            if((errorCode == 0) && !hkResult.IsInvalid) {
                var key = NewRegistryKey(hkResult, writable, false, @this.get_remoteKey(), false, view);
                key.set_keyName(@this.get_keyName() + @"\" + name);
                return key;
            }
            return null;
        }

        private static RegistryKey InternalOpenSubKey(this RegistryKey @this, string name, RegistryKeyPermissionCheck permissionCheck, int rights, RegistryView4 view) {
            ValidateKeyName(name);
            ValidateKeyMode(permissionCheck);
            ValidateKeyRights(rights);
            @this.EnsureNotDisposed();
            name = FixupName(name);
            @this.CheckOpenSubKeyPermission(name, permissionCheck);
            SafeRegistryHandle4 hkResult = null;
            int errorCode = Win32Native.RegOpenKeyEx(@this.get_hkey(), name, 0, rights | (int) view, out hkResult);
            if((errorCode == 0) && !hkResult.IsInvalid) {
                var key = NewRegistryKey(hkResult, permissionCheck == RegistryKeyPermissionCheck.ReadWriteSubTree, false, @this.get_remoteKey(), false, view);
                key.set_keyName(@this.get_keyName() + @"\" + name);
                key.set_checkMode(permissionCheck);
                return key;
            }
            if((errorCode == 5) || (errorCode == 0x542)) {
                throw new SecurityException(Properties.Resources.Security_RegistryPermission);
            }
            return null;
        }

        private static RegistryKey CreateSubKeyInternal(this RegistryKey @this, string subkey, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity, RegistryOptions4 registryOptions, RegistryView4 view) {
            ValidateKeyOptions(registryOptions);
            ValidateKeyName(subkey);
            ValidateKeyMode(permissionCheck);
            @this.EnsureWriteable();
            subkey = FixupName(subkey);
            if(!@this.get_remoteKey()) {
                RegistryKey key = @this.InternalOpenSubKey(subkey, permissionCheck != RegistryKeyPermissionCheck.ReadSubTree, view);
                if(key != null) {
                    @this.CheckSubKeyWritePermission(subkey);
                    @this.CheckSubTreePermission(subkey, permissionCheck);
                    key.set_checkMode(permissionCheck);
                    return key;
                }
            }
            @this.CheckSubKeyCreatePermission(subkey);
            Win32Native.SECURITY_ATTRIBUTES structure = null;
            if(registrySecurity != null) {
                structure = new Win32Native.SECURITY_ATTRIBUTES {
                    nLength = Marshal.SizeOf(structure)
                };
                structure.pSecurityDescriptor = registrySecurity.GetSecurityDescriptorBinaryForm();
            }
            int lpdwDisposition = 0;
            SafeRegistryHandle4 hkResult = null;
            int errorCode = Win32Native.RegCreateKeyEx(@this.get_hkey(), subkey, 0, null, (int) registryOptions, GetRegistryKeyAccess(permissionCheck != RegistryKeyPermissionCheck.ReadSubTree, view), structure, out hkResult, out lpdwDisposition);
            if((errorCode == 0) && !hkResult.IsInvalid) {
                var key2 = NewRegistryKey(hkResult, permissionCheck != RegistryKeyPermissionCheck.ReadSubTree, false, @this.get_remoteKey(), false, view);
                @this.CheckSubTreePermission(subkey, permissionCheck);
                key2.set_checkMode(permissionCheck);
                if(subkey.Length == 0) {
                    key2.set_keyName(@this.get_keyName());
                } else {
                    key2.set_keyName(@this.get_keyName() + @"\" + subkey);
                }
                return key2;
            }
            if(errorCode != 0) {
                @this.Win32Error(errorCode, @this.get_keyName() + @"\" + subkey);
            }
            return null;
        }

        private static void DeleteSubKeyTreeInternal(this RegistryKey @this, string subkey, RegistryView4 view) {
            RegistryKey key = @this.InternalOpenSubKey(subkey, true, view);
            if(key != null) {
                try {
                    if(key.InternalSubKeyCount() > 0) {
                        string[] subKeyNames = key.InternalGetSubKeyNames();
                        for(int i = 0; i < subKeyNames.Length; i++) {
                            key.DeleteSubKeyTreeInternal(subKeyNames[i], view);
                        }
                    }
                } finally {
                    key.Close();
                }
                int errorCode;
                try {
                    errorCode = Win32Native.RegDeleteKeyEx(@this.get_hkey(), subkey, (int) view, 0);
                } catch(EntryPointNotFoundException) {
                    errorCode = Win32Native.RegDeleteKey(@this.get_hkey(), subkey);
                }
                if(errorCode != 0) {
                    @this.Win32Error(errorCode, null);
                }
            } else {
                throw new ArgumentException(Properties.Resources.Arg_RegSubKeyAbsent);
            }
        }

        private static void EnsureNotDisposed(this RegistryKey @this) {
            if(@this.get_hkey() == null) {
                throw new ObjectDisposedException(@this.get_keyName(), Properties.Resources.ObjectDisposed_RegKeyClosed);
            }
        }

        private static void EnsureWriteable(this RegistryKey @this) {
            @this.EnsureNotDisposed();
            if(!@this.IsWritable()) {
                throw new UnauthorizedAccessException(Properties.Resources.UnauthorizedAccess_RegistryNoWrite);
            }
        }

        private static bool IsSystemKey(this RegistryKey @this) {
            return ((@this.get_state() & 2) != 0);
        }

        private static bool IsWritable(this RegistryKey @this) {
            return ((@this.get_state() & 4) != 0);
        }

        private static bool IsPerfDataKey(this RegistryKey @this) {
            return ((@this.get_state() & 8) != 0);
        }

        private static RegistryKeyPermissionCheck GetSubKeyPermissonCheck(this RegistryKey @this, bool subkeyWritable) {
            if(@this.get_checkMode() == RegistryKeyPermissionCheck.Default) {
                return @this.get_checkMode();
            }
            if(subkeyWritable) {
                return RegistryKeyPermissionCheck.ReadWriteSubTree;
            }
            return RegistryKeyPermissionCheck.ReadSubTree;
        }

        private static void Win32Error(this RegistryKey @this, int errorCode, string str) {
            switch(errorCode) {
            case 2:
                throw new IOException(Properties.Resources.Arg_RegKeyNotFound, errorCode);
            case 5:
                if(str != null) {
                    throw new UnauthorizedAccessException(string.Format(Properties.Resources.UnauthorizedAccess_RegistryKeyGeneric_Key, str));
                }
                throw new UnauthorizedAccessException();
            case 6:
                if(!@this.IsPerfDataKey()) {
                    @this.clear_hkey();
                }
                break;
            case 0xEA:
                if(@this.get_remoteKey()) {
                    return;
                }
                break;
            }
            throw new IOException(Win32Native.GetMessage(errorCode), errorCode);
        }

        private static int InternalSubKeyCount(this RegistryKey @this) {
            @this.EnsureNotDisposed();
            int lpcSubKeys = 0;
            int lpcValues = 0;
            int errorCode = Win32Native.RegQueryInfoKey(@this.get_hkey(), null, null, IntPtr.Zero, ref lpcSubKeys, null, null, ref lpcValues, null, null, null, null);
            if(errorCode != 0) {
                @this.Win32Error(errorCode, null);
            }
            return lpcSubKeys;
        }

        private static string[] InternalGetSubKeyNames(this RegistryKey @this) {
            @this.EnsureNotDisposed();
            int num = @this.InternalSubKeyCount();
            string[] strArray = new string[num];
            if(num > 0) {
                StringBuilder lpName = new StringBuilder(0x100);
                for(int i = 0; i < num; i++) {
                    int capacity = lpName.Capacity;
                    int errorCode = Win32Native.RegEnumKeyEx(@this.get_hkey(), i, lpName, out capacity, null, null, null, null);
                    if(errorCode != 0) {
                        @this.Win32Error(errorCode, null);
                    }
                    strArray[i] = lpName.ToString();
                }
            }
            return strArray;
        }

        #endregion

        #region Private Static Methods (Internal)

        private static void ValidateKeyMode(RegistryKeyPermissionCheck mode) {
            if((mode < RegistryKeyPermissionCheck.Default) || (mode > RegistryKeyPermissionCheck.ReadWriteSubTree)) {
                throw new ArgumentException(Properties.Resources.Argument_InvalidRegistryKeyPermissionCheck, "mode");
            }
        }

        private static void ValidateKeyName(string name) {
            if(name == null) {
                throw new ArgumentNullException("name");
            }
            int index = name.IndexOf(@"\", StringComparison.OrdinalIgnoreCase);
            int startIndex = 0;
            while(index != -1) {
                if((index - startIndex) > 0xff) {
                    throw new ArgumentException(Properties.Resources.Arg_RegKeyStrLenBug);
                }
                startIndex = index + 1;
                index = name.IndexOf(@"\", startIndex, StringComparison.OrdinalIgnoreCase);
            }
            if((name.Length - startIndex) > 0xff) {
                throw new ArgumentException(Properties.Resources.Arg_RegKeyStrLenBug);
            }
        }

        private static void ValidateKeyOptions(RegistryOptions4 options) {
            if((options < RegistryOptions4.None) || (options > RegistryOptions4.Volatile)) {
                throw new ArgumentException(Properties.Resources.Argument_InvalidRegistryOptionsCheck, "options");
            }
        }

        private static void ValidateKeyRights(int rights) {
            if((rights & -983104) != 0) {
                throw new SecurityException(Properties.Resources.Security_RegistryPermission);
            }
        }

        private static void ValidateKeyView(RegistryView4 view) {
            if(((view != RegistryView4.Default) && (view != RegistryView4.Registry32)) && (view != RegistryView4.Registry64)) {
                throw new ArgumentException(Properties.Resources.Argument_InvalidRegistryViewCheck, "view");
            }
        }

        private static int GetRegistryKeyAccess(bool isWritable, RegistryView4 view) {
            int num;
            if(!isWritable) {
                num = 0x00020019;
            } else {
                num = 0x0002001f;
            }
            return num | (int) view;
        }

        private static int GetRegistryKeyAccess(RegistryKeyPermissionCheck mode, RegistryView4 view) {
            int num = 0;
            switch(mode) {
            case RegistryKeyPermissionCheck.Default:
            case RegistryKeyPermissionCheck.ReadSubTree:
                num = 0x20019;
                break;
            case RegistryKeyPermissionCheck.ReadWriteSubTree:
                num = 0x2001f;
                break;
            }
            return num | (int) view;
        }

        private static string FixupName(string name) {
            if(name.IndexOf('\\') == -1) {
                return name;
            }
            StringBuilder path = new StringBuilder(name);
            FixupPath(path);
            int num = path.Length - 1;
            if((num >= 0) && (path[num] == '\\')) {
                path.Length = num;
            }
            return path.ToString();
        }

        private static void FixupPath(StringBuilder path) {
            int num2;
            int length = path.Length;
            bool flag = false;
            char ch = (char) 0xffff;
            for(num2 = 1; num2 < (length - 1); num2++) {
                if(path[num2] == '\\') {
                    num2++;
                    while(num2 < length) {
                        if(path[num2] != '\\') {
                            break;
                        }
                        path[num2] = ch;
                        num2++;
                        flag = true;
                    }
                }
            }
            if(flag) {
                num2 = 0;
                int num3 = 0;
                while(num2 < length) {
                    if(path[num2] == ch) {
                        num2++;
                    } else {
                        path[num3] = path[num2];
                        num2++;
                        num3++;
                    }
                }
                path.Length += num3 - num2;
            }
        }

        #endregion

        #region Private Methods (Code Access Security)

        private static void CheckOpenSubKeyPermission(this RegistryKey @this, string subkeyName, RegistryKeyPermissionCheck subKeyCheck) {
            if((subKeyCheck == RegistryKeyPermissionCheck.Default) && (@this.get_checkMode() == RegistryKeyPermissionCheck.Default)) {
                CheckSubKeyReadPermission(@this, subkeyName);
            }
            CheckSubTreePermission(@this, subkeyName, subKeyCheck);
        }

        private static void CheckOpenSubKeyPermission(this RegistryKey @this, string subkeyName, bool subKeyWritable) {
            if(@this.get_checkMode() == RegistryKeyPermissionCheck.Default) {
                CheckSubKeyReadPermission(@this, subkeyName);
            }
            if(subKeyWritable && (@this.get_checkMode() == RegistryKeyPermissionCheck.ReadSubTree)) {
                CheckSubTreeReadWritePermission(@this, subkeyName);
            }
        }

        private static void CheckSubKeyCreatePermission(this RegistryKey @this, string subkeyName) {
            if(@this.get_remoteKey()) {
                CheckUnmanagedCodePermission();
            } else if(@this.get_checkMode() == RegistryKeyPermissionCheck.Default) {
                new RegistryPermission(RegistryPermissionAccess.Create, @this.get_keyName() + @"\" + subkeyName + @"\.").Demand();
            }
        }

        private static void CheckSubKeyReadPermission(this RegistryKey @this, string subkeyName) {
            if(@this.get_remoteKey()) {
                CheckUnmanagedCodePermission();
            } else {
                new RegistryPermission(RegistryPermissionAccess.Read, @this.get_keyName() + @"\" + subkeyName + @"\.").Demand();
            }
        }

        private static void CheckSubKeyWritePermission(this RegistryKey @this, string subkeyName) {
            if(@this.get_remoteKey()) {
                CheckUnmanagedCodePermission();
            } else if(@this.get_checkMode() == RegistryKeyPermissionCheck.Default) {
                new RegistryPermission(RegistryPermissionAccess.Write, @this.get_keyName() + @"\" + subkeyName + @"\.").Demand();
            }
        }

        private static void CheckSubTreePermission(this RegistryKey @this, string subkeyName, RegistryKeyPermissionCheck subKeyCheck) {
            if(subKeyCheck == RegistryKeyPermissionCheck.ReadSubTree) {
                if(@this.get_checkMode() == RegistryKeyPermissionCheck.Default) {
                    CheckSubTreeReadPermission(@this, subkeyName);
                }
            } else if((subKeyCheck == RegistryKeyPermissionCheck.ReadWriteSubTree) && (@this.get_checkMode() != RegistryKeyPermissionCheck.ReadWriteSubTree)) {
                CheckSubTreeReadWritePermission(@this, subkeyName);
            }
        }

        private static void CheckSubTreeReadPermission(this RegistryKey @this, string subkeyName) {
            if(@this.get_remoteKey()) {
                CheckUnmanagedCodePermission();
            } else if(@this.get_checkMode() == RegistryKeyPermissionCheck.Default) {
                new RegistryPermission(RegistryPermissionAccess.Read, @this.get_keyName() + @"\" + subkeyName + @"\").Demand();
            }
        }

        private static void CheckSubTreeReadWritePermission(this RegistryKey @this, string subkeyName) {
            if(@this.get_remoteKey()) {
                CheckUnmanagedCodePermission();
            } else {
                new RegistryPermission(RegistryPermissionAccess.Write | RegistryPermissionAccess.Read, @this.get_keyName() + @"\" + subkeyName).Demand();
            }
        }

        private static void CheckSubTreeWritePermission(this RegistryKey @this, string subkeyName) {
            if(@this.get_remoteKey()) {
                CheckUnmanagedCodePermission();
            } else if(@this.get_checkMode() == RegistryKeyPermissionCheck.Default) {
                new RegistryPermission(RegistryPermissionAccess.Write, @this.get_keyName() + @"\" + subkeyName + @"\").Demand();
            }
        }

        private static void CheckUnmanagedCodePermission() {
            new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
        }

        #endregion

        #region Private Static Methods (32/64 bit switch)

        private static bool Force32BitView() {
            // Unfortunately, despite my best efforts, the reflection-based code still runs 50% slower
            // than the native code. I don't like the idea of introducing an artificial delay into a
            // 32-bit process just for the sake of exercising the code more, so the reflection-based code
            // will only be run when absolutely necessary, i.e. in a 64-bit process. The conditional
            // compilation flag is for testing purposes.
#if !TestReflectionCodeIn32Bit
            if(Environment4.Is32BitProcess) {
                return false;
            } else
#endif
            if(RegistryKeyInfo.IsCompatibleRuntime && SafeRegistryHandleInfo.IsCompatibleRuntime) {
                return true;
            } else {
                throw new PlatformNotSupportedException(Properties.Resources.IncompatibleRuntime);
            }
        }

        #endregion
    }
}
