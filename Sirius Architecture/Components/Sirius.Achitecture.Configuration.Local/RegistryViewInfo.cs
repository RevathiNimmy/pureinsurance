using Microsoft.Win32;
using System;

namespace Sirius.Architecture.Configuration.Local {

    /// <summary>
    /// Reflection data for the <c>RegistryView</c> enum. This is generated once then cached in memory for speed.
    /// </summary>
    internal sealed class RegistryViewInfo {

        #region Private Fields

        private Type _type;

        private RegistryViewInfo() {

            // All we are interested in here is whether the type exists (only in mscorlib version 4.0).

            _type = typeof(RegistryKey).Assembly.GetType("Microsoft.Win32.RegistryView");
        }

        #endregion

        #region Private Static Fields

        private static readonly RegistryViewInfo _singleton = new RegistryViewInfo();

        #endregion

        #region Public Static Properties

        public static Type Type { get { return _singleton._type; } }

        #endregion
    }
}
