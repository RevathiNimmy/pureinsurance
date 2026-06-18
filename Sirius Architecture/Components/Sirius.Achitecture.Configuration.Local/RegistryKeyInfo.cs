using Microsoft.Win32;
using System;
using System.Reflection;

namespace Sirius.Architecture.Configuration.Local {

    /// <summary>
    /// Reflection data for the <c>RegistryKey</c> class. This is generated once then cached in memory for speed.
    /// </summary>
    internal sealed class RegistryKeyInfo {

        #region Private Fields

        private bool _isCompatibleRuntime;
        private Type _type;
        private ConstructorInfo _constructor5;
        private ConstructorInfo _constructor6;
        private FieldInfo _hkeyField;
        private FieldInfo _keyNameField;
        private FieldInfo _remoteKeyField;
        private FieldInfo _checkModeField;
        private FieldInfo _stateField;
        private FieldInfo _regViewField;

        private RegistryKeyInfo() {

            // We have to be really strict here, because Microsoft could change the private
            // implementation of the class at any time.

            _isCompatibleRuntime = false;

            // This type exists and is public in both mscorlib 2.0 and 4.0.
            _type = typeof(RegistryKey);

            // This constructor is only present in mscorlib version 2.0, and is private.
            if(SafeRegistryHandleInfo.Type != null) {
                _constructor5 = _type.GetConstructor(
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding,
                    null,
                    new Type[] {SafeRegistryHandleInfo.Type, typeof(bool), typeof(bool), typeof(bool), typeof(bool)},
                    null);
            }

            // This constructor is only present in mscorlib version 4.0, and is private.
            if(SafeRegistryHandleInfo.Type != null && RegistryViewInfo.Type != null) {
                _constructor6 = _type.GetConstructor(
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding,
                    null,
                    new Type[] {SafeRegistryHandleInfo.Type, typeof(bool), typeof(bool), typeof(bool), typeof(bool), RegistryViewInfo.Type},
                    null);
            }

            // These fields exist and are private in both mscorlib 2.0 and 4.0.
            _hkeyField = _type.GetField("hkey", BindingFlags.Instance | BindingFlags.NonPublic);
            _keyNameField = _type.GetField("keyName", BindingFlags.Instance | BindingFlags.NonPublic);
            _remoteKeyField = _type.GetField("remoteKey", BindingFlags.Instance | BindingFlags.NonPublic);
            _checkModeField = _type.GetField("checkMode", BindingFlags.Instance | BindingFlags.NonPublic);
            _stateField = _type.GetField("state", BindingFlags.Instance | BindingFlags.NonPublic);

            // This field only exists in mscorlib version 4.0, and is private.
            _regViewField = _type.GetField("regView", BindingFlags.Instance | BindingFlags.NonPublic);

            _isCompatibleRuntime = (_constructor5 != null || _constructor6 != null) &&
                _hkeyField != null && _hkeyField.FieldType == SafeRegistryHandleInfo.Type &&
                _keyNameField != null && _keyNameField.FieldType == typeof(string) &&
                _remoteKeyField != null && _remoteKeyField.FieldType == typeof(bool) &&
                _checkModeField != null && _checkModeField.FieldType == typeof(RegistryKeyPermissionCheck) &&
                _stateField != null && _stateField.FieldType == typeof(int) &&
                (_regViewField == null || _regViewField.FieldType == RegistryViewInfo.Type);
        }

        #endregion

        #region Private Static Fields

        private static readonly RegistryKeyInfo _singleton = new RegistryKeyInfo();

        #endregion

        #region Public Static Properties

        public static bool IsCompatibleRuntime { get { return _singleton._isCompatibleRuntime; } }
        public static Type Type { get { return _singleton._type; } }
        public static ConstructorInfo Constructor5 { get { return _singleton._constructor5; } }
        public static ConstructorInfo Constructor6 { get { return _singleton._constructor6; } }
        public static FieldInfo HKeyField { get { return _singleton._hkeyField; } }
        public static FieldInfo KeyNameField { get { return _singleton._keyNameField; } }
        public static FieldInfo RemoteKeyField { get { return _singleton._remoteKeyField; } }
        public static FieldInfo CheckModeField { get { return _singleton._checkModeField; } }
        public static FieldInfo StateField { get { return _singleton._stateField; } }
        public static FieldInfo RegViewField { get { return _singleton._regViewField; } }

        #endregion
    }
}
