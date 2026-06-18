using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using ComTypes = System.Runtime.InteropServices.ComTypes;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// Provides helper methods for runtime callable wrappers (RCW).
    /// </summary>
    public static class RcwHelper {

        #region Public Static Methods

        /// <summary>
        /// Safely release all COM references to this RCW. If the object is null, do nothing.
        /// </summary>
        /// <param name="obj">COM object</param>
        /// <exception cref="ArgumentException">Object is not a valid COM object.</exception>
        /// <remarks>
        /// This is a simple wrapper around <see cref="Marshal.FinalReleaseComObject"/>. The difference is,
        /// a null reference does nothing rather than throwing an exception, so the calling code does not
        /// need to include that safety check.
        /// </remarks>
        public static void Release(object obj) {

            if(obj != null) {
                Marshal.FinalReleaseComObject(obj);
            }
        }

        /// <summary>
        /// Return the COM type name of the specified COM object. If the object is null, return null.
        /// If the object is not a COM object or is not queryable, then return the .NET type name instead.
        /// </summary>
        /// <param name="obj">COM object</param>
        /// <remarks>
        /// Any exception that may occur while querying the COM object will be absorbed, and the .NET type
        /// name will be returned instead. Since this method is intended for use mainly in diagnostic code,
        /// throwing exceptions would be of little benefit.
        /// </remarks>
        public static string TypeName(object obj) {

            if(obj == null) {
                // Object is null.
                return null;
            }

            string clrTypeName = obj.GetType().ToString();

            if(!Marshal.IsComObject(obj)) {
                // Not a COM object, return the .NET type name.
                return clrTypeName;
            }

            // CAS safety check.
            new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();

            // Query the COM type information for this object.
            // Try getting the ProgID via IPersist first.
            {
                IPersist persist = null;
                try {
                    persist = obj as IPersist;
                    if(persist != null) {
                        Guid classId;
                        persist.GetClassID(out classId);
                        string progId;
                        ProgIDFromCLSID(ref classId, out progId);
                        return clrTypeName + "(" + progId + ")";
                    }
                } catch {
                    // Absorb all errors.
                } finally {
                    // Release all COM references acquired in this block.
                    if(persist != null) { Marshal.ReleaseComObject(persist); }
                }
            }

            // If the above failed, try querying ITypeInfo via IDispatch.
            {
                IDispatch dispatch = null;
                ComTypes.ITypeInfo typeInfo = null;
                try {
                    dispatch = obj as IDispatch;
                    if(dispatch != null) {
                        int rv = dispatch.GetTypeInfo(0, 0, out typeInfo);
                        if(rv >= 0) {
                            // Try to get the CLSID directly from the type information, then translate it to the ProgID.
                            ComTypes.TYPEATTR typeAttr;
                            IntPtr ppTypeAttr = IntPtr.Zero;
                            try {
                                typeInfo.GetTypeAttr(out ppTypeAttr);
                                typeAttr = (ComTypes.TYPEATTR) Marshal.PtrToStructure(ppTypeAttr, typeof(ComTypes.TYPEATTR));
                            } finally {
                                typeInfo.ReleaseTypeAttr(ppTypeAttr);
                            }
                            if(typeAttr.typekind == ComTypes.TYPEKIND.TKIND_COCLASS) {
                                Guid classId = typeAttr.guid;
                                string progId;
                                ProgIDFromCLSID(ref classId, out progId);
                                return clrTypeName + "(" + progId + ")";
                            }

                            // If the above failed, try to get the human-readable type name from the help info.
                            int helpContext;
                            string typeName, documentation, helpFile;
                            typeInfo.GetDocumentation(-1, out typeName, out documentation, out helpContext, out helpFile);
                            return clrTypeName + "(" + typeName + ")";
                        }
                    }
                } catch {
                    // Absorb all errors.
                } finally {
                    // Release all COM references acquired in this block.
                    if(typeInfo != null) { Marshal.ReleaseComObject(typeInfo); }
                    if(dispatch != null) { Marshal.ReleaseComObject(dispatch); }
                }
            }

            // If the above failed, return just the .NET type name as a last resort.
            return clrTypeName;
        }

        #endregion

        #region Private Types

        /// <summary>
        /// Translate a COM CLSID to a ProgID.
        /// </summary>
        [DllImport("ole32.dll")]
        private static extern int ProgIDFromCLSID([In] ref Guid clsid, [MarshalAs(UnmanagedType.LPWStr)] out string lplpszProgID);

        /// <summary>
        /// Private copy of the COM IPersist interface just for use in this class.
        /// </summary>
        [ComImport]
        [Guid("0000010C-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IPersist {
            void GetClassID(out Guid pClassID);
        }

        /// <summary>
        /// Private copy of the COM IDispatch interface just for use in this class.
        /// </summary>
        [ComImport]
        [Guid("00020400-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IDispatch {
            [PreserveSig]
            int GetTypeInfoCount(out int Count);

            [PreserveSig]
            int GetTypeInfo(
                [MarshalAs(UnmanagedType.U4)] int iTInfo,
                [MarshalAs(UnmanagedType.U4)] int lcid,
                out ComTypes.ITypeInfo typeInfo);

            [PreserveSig]
            int GetIDsOfNames(
                ref Guid riid,
                [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] string[] rgsNames,
                int cNames,
                int lcid,
                [MarshalAs(UnmanagedType.LPArray)] int[] rgDispId);

            [PreserveSig]
            int Invoke(
                int dispIdMember,
                ref Guid riid,
                uint lcid,
                ushort wFlags,
                ref ComTypes.DISPPARAMS pDispParams,
                out object pVarResult,
                ref ComTypes.EXCEPINFO pExcepInfo,
                IntPtr[] pArgErr);
        }

        #endregion
    }
}
