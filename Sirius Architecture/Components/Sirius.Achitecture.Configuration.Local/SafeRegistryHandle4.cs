using Microsoft.Win32.SafeHandles;
using System;
using System.Security.Permissions;

namespace Sirius.Architecture.Configuration.Local {

    /// <summary>
    /// A work-alike copy of the internal .NET class of the same name.
    /// This exists purely so that the P/Invoke marshaller has something to manipulate.
    /// </summary>
    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
    internal sealed class SafeRegistryHandle4 : SafeHandleZeroOrMinusOneIsInvalid {

        #region SafeHandleZeroOrMinusOneIsInvalid Members

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        internal SafeRegistryHandle4()
            : base(true) {
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public SafeRegistryHandle4(IntPtr preexistingHandle, bool ownsHandle)
            : base(ownsHandle) {
            base.SetHandle(preexistingHandle);
        }

        protected override bool ReleaseHandle() {
            return Win32Native.RegCloseKey(base.handle) == 0;
        }

        #endregion
    }
}
