using Microsoft.Win32.SafeHandles;
using System;
using System.Reflection;

namespace Sirius.Architecture.Configuration.Local {

    /// <summary>
    /// Reflection data for the <c>SafeRegistryHandle</c> class. This is generated once then cached in memory for speed.
    /// </summary>
    internal sealed class SafeRegistryHandleInfo {

        #region Private Fields

        private bool _isCompatibleRuntime;
        private Type _type;
        private ConstructorInfo _constructor;

        private SafeRegistryHandleInfo() {

            // We have to be really strict here, because Microsoft could change the private
            // implementation of the class at any time.

            _isCompatibleRuntime = false;

            // This type exists in both mscorlib 2.0 and 4.0, but is only public in 4.0.
            _type = typeof(SafeHandleZeroOrMinusOneIsInvalid).Assembly.GetType("Microsoft.Win32.SafeHandles.SafeRegistryHandle");
            if(_type == null) {
                return;
            }

            // This constructor exists in both mscorlib 2.0 and 4.0, but is only public in 4.0.
            _constructor = _type.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding,
                null,
                new Type[] {typeof(IntPtr), typeof(bool)},
                null);

            _isCompatibleRuntime = _constructor != null;
        }

        #endregion

        #region Private Static Fields

        private static readonly SafeRegistryHandleInfo _singleton = new SafeRegistryHandleInfo();

        #endregion

        #region Public Static Properties

        public static bool IsCompatibleRuntime { get { return _singleton._isCompatibleRuntime; } }
        public static Type Type { get { return _singleton._type; } }
        public static ConstructorInfo Constructor { get { return _singleton._constructor; } }

        #endregion
    }
}
