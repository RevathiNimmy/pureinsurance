using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Artinsoft.VB6.Utils
{
    /// <summary>
    /// The ReflectionHelper class provides functionality to handle the instantiation, 
    /// setting/reading properties and method invocation using reflection on the .NET Framework. 
    /// Using of this class is optional, and will only appear if it is selected in the Upgrade Profile. 
    /// It is used when it is necessary to continue using late-bound calls to 
    /// objects in the migrated application.
    /// </summary>
    public class ReflectionHelper
    {
        /// <summary>
        /// External method from oleaut32.dll, named VariantClear.
        /// </summary>
        [DllImport("oleaut32.dll", PreserveSig = false)]
        public static extern void VariantClear(HandleRef pObject);

        /// <summary>
        /// Represents a structure to find the size of variant types from unmanaged code.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct FindSizeOfVariant
        {
            [MarshalAs(UnmanagedType.Struct)]
            public object var;
            public byte b;
        }

        /// <summary>
        /// Indicates the size of a variant type in unmanaged code.
        /// </summary>
        private static readonly int VariantSize = 0;

        /// <summary>
        /// Static Constructor of ReflectionHelper.
        /// </summary>
        static ReflectionHelper()
        {
            VariantSize = (int)Marshal.OffsetOf(typeof(FindSizeOfVariant), "b");
        }

        /// <summary>
        /// Functions to check if a member exists.
        /// </summary>
        /// <param name="obj">The object containing the member.</param>
        /// <param name="propName">The name of the member.</param>
        /// <returns>True if the member exists.</returns>
        protected internal static bool ExistMember(object obj, string propName)
        {
            try
            {
                GetMember(obj, propName);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets a member from an object by reflection.
        /// </summary>
        /// <param name="obj">The source object that should be checked for the property.</param>
        /// <param name="propName">The name of the property that is required.</param>
        /// <param name="indexes">In the case that property represents an array 
        /// the index(es) must be specified here.</param>
        /// <param name="expectedType">The expected type of the property.</param>
        /// <returns>Returns the value of the member.</returns>
        public static object GetMember(object obj, string propName, object[] indexes, Type expectedType)
        {
            object Result = null;
            FieldInfo fInfo = null;
            PropertyInfo pInfo = null;
            List<string> namesToTry = new List<string>();
            bool memberFound = false;

            if (obj == null)
                throw new Exception("Object required");

            if (obj.GetType().IsCOMObject)
                Result = GetMemberByIDispatch(obj, propName, indexes, out memberFound);

            if (!memberFound)
            {
                namesToTry.Add(propName);
                namesToTry.Add(propName.ToLower());
                namesToTry.Add("get_" + propName);

                foreach (string pName in namesToTry)
                {
                    fInfo = obj.GetType().GetField(pName);

                    if (fInfo != null)
                    {
                        if ((fInfo.FieldType.IsArray) && (indexes != null) && (indexes.Length > 0))
                        {
                            int[] intIndexes = new int[indexes.Length];
                            indexes.CopyTo(intIndexes, 0);

                            Array objArray = (Array)fInfo.GetValue(obj);
                            Result = objArray.GetValue(intIndexes);
                            memberFound = true;
                        }
                        else
                        {
                            Result = fInfo.GetValue(obj);
                            memberFound = true;
                        }
                    }
                    else
                    {
                        try
                        {
                            pInfo = obj.GetType().GetProperty(pName);
                        }
                        catch (System.Reflection.AmbiguousMatchException)
                        {
                            pInfo = obj.GetType().GetProperty(pName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.IgnoreCase);
                        }
                        catch (Exception e2)
                        {
                            throw e2;
                        }

                        if (pInfo != null)
                        {
                            if ((pInfo.PropertyType.IsArray) && (indexes != null) && (indexes.Length > 0))
                            {
                                Result = pInfo.GetValue(obj, indexes);
                                memberFound = true;
                            }
                            else
                            {
                                Result = pInfo.GetValue(obj, null);
                                memberFound = true;
                            }
                        }
                    }

                    if (memberFound)
                        break;
                }
            }

            if (!memberFound)
            {
                foreach (string fName in namesToTry)
                {
                    Result = Invoke(obj, fName, indexes, out memberFound);
                    if (memberFound)
                        break;
                }
            }

            if (!memberFound)
                throw new Exception("Object doesn't support this property or method '" + propName + "'");

            return Result;
        }

        /// <summary>
        /// Gets a member from an object by reflection.
        /// </summary>
        /// <param name="obj">The source object that should be checked for the property.</param>
        /// <param name="propName">The name of the property that is required.</param>
        /// <param name="indexes">In the case that property represents an array 
        /// the index(es) must be specified here.</param>
        /// <returns>Returns the value of the member.</returns>
        public static object GetMember(object obj, string propName, object[] indexes)
        {
            return GetMember(obj, propName, indexes, Type.GetType("object"));
        }

        /// <summary>
        /// Gets a member from an object by reflection.
        /// </summary>
        /// <param name="obj">The source object that should be checked for the property.</param>
        /// <param name="propName">The name of the property that is required.</param>
        /// <param name="expectedType">The expected type of the property.</param>
        /// <returns>Returns the value of the member.</returns>
        public static object GetMember(object obj, string propName, Type expectedType)
        {
            return GetMember(obj, propName, null, expectedType);
        }

        /// <summary>
        /// Gets a member from an object by reflection.
        /// </summary>
        /// <param name="obj">The source object that should be checked for the property.</param>
        /// <param name="propName">The name of the property that is required.</param>
        /// <returns>Returns the value of the member.</returns>
        public static object GetMember(object obj, string propName)
        {
            return GetMember(obj, propName, null, Type.GetType("object"));
        }

        /// <summary>
        /// Function to retrieve a member using the IDispatch interface. 
        /// Useful when the object is a ComObject wrapping another object.
        /// </summary>
        /// <param name="obj">The source object to check for the property.</param>
        /// <param name="propName">The name of the property that is requested.</param>
        /// <param name="indexes">The list of indexes if the property represents an array.</param>
        /// <param name="memberFound">A flag to indicate that the member was found.</param>
        /// <returns>The value of the member if it is found.</returns>
        private static object GetMemberByIDispatch(object obj, string propName, object[] indexes, out bool memberFound)
        {
            object Result = null;
            IDispatch disp = null;
            string[] propNames = null;
            int[] dispIds = null;
            Guid guid = Guid.Empty;
            int hr = 0;
            tagDISPPARAMS tagDispParams = null;
            tagEXCEPINFO tagExceptionInfo = null;
            object[] results = null;
            int dwFlags = 0;
            List<string> namesToTry = new List<string>();

            memberFound = false;
            try
            {
                disp = obj as IDispatch;
                if (disp != null)
                {
                    namesToTry.Add(propName);
                    namesToTry.Add(propName.ToLower());
                    namesToTry.Add("get_" + propName);

                    propNames = namesToTry.ToArray();
                    dispIds = new int[propNames.Length];
                    hr = disp.GetIDsOfNames(ref guid, propNames, propNames.Length, -1, dispIds);
                    if (hr == 0)
                    {
                        //TODO. Needs to implement the case where parameters are necessary
                        tagDispParams = new tagDISPPARAMS();
                        tagDispParams.cArgs = 0;

                        tagExceptionInfo = new tagEXCEPINFO();
                        results = new object[1];
                        dwFlags = (int)tagINVOKEKIND.INVOKE_PROPERTYGET;
                        hr = disp.Invoke(dispIds[0], ref guid, -1, dwFlags, tagDispParams, results, tagExceptionInfo, null);

                        if ((hr == 0) && (results != null) && (results.Length > 0))
                        {
                            Result = results[0];
                            memberFound = true;
                        }
                    }
                }
            }
            catch { }

            return Result;
        }


        /// <summary>
        /// Sets the value of a member from an object by reflection.
        /// </summary>
        /// <param name="obj">The source object that should be checked for the property.</param>
        /// <param name="propName">The name of the property that is required.</param>
        /// <param name="indexes">In the case that property represents an array 
        /// the index(es) must be specified here.</param>
        /// <param name="value">The new value to be set.</param>
        /// <param name="expectedType">The expected type of the property.</param>
        public static void SetMember(object obj, string propName, object[] indexes, object value, Type expectedType)
        {
            FieldInfo fInfo = null;
            PropertyInfo pInfo = null;
            List<string> namesToTry = new List<string>();
            bool memberSet = false;

            if (obj == null)
                throw new Exception("Object required");

            namesToTry.Add(propName);
            namesToTry.Add(propName.ToLower());
            namesToTry.Add("set_" + propName);

            foreach (string pName in namesToTry)
            {
                fInfo = obj.GetType().GetField(pName);

                if (fInfo != null)
                {
                    if ((fInfo.FieldType.IsArray) && (indexes != null) && (indexes.Length > 0))
                    {
                        int[] intIndexes = new int[indexes.Length];
                        indexes.CopyTo(intIndexes, 0);

                        Array objArray = (Array)fInfo.GetValue(obj);
                        if (!objArray.GetType().GetElementType().Equals(value.GetType()))
                            value = Convert.ChangeType(value, objArray.GetType().GetElementType());

                        objArray.SetValue(value, intIndexes);
                        fInfo.SetValue(obj, objArray);
                        memberSet = true;
                    }
                    else
                    {
                        if (!fInfo.FieldType.Equals(value.GetType()))
                            value = Convert.ChangeType(value, fInfo.FieldType);

                        fInfo.SetValue(obj, value);
                        memberSet = true;
                    }
                }
                else
                {
                    try
                    {
                        pInfo = obj.GetType().GetProperty(pName);
                    }
                    catch (System.Reflection.AmbiguousMatchException)
                    {
                        pInfo = obj.GetType().GetProperty(pName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.IgnoreCase);
                    }
                    catch (Exception e2)
                    {
                        throw e2;
                    }

                    if (pInfo != null)
                    {
                        if ((pInfo.PropertyType.IsArray) && (indexes != null) && (indexes.Length > 0))
                        {
                            if (!pInfo.PropertyType.GetElementType().Equals(value.GetType()))
                                value = Convert.ChangeType(value, pInfo.PropertyType.GetElementType());

                            pInfo.SetValue(obj, value, indexes);
                            memberSet = true;
                        }
                        else
                        {
                            if (!pInfo.PropertyType.Equals(value.GetType()))
                                value = Convert.ChangeType(value, pInfo.PropertyType);

                            pInfo.SetValue(obj, value, null);
                            memberSet = true;
                        }
                    }
                }

                if (memberSet)
                    break;
            }

            if (!memberSet)
            {
                object[] parameters = new object[indexes.Length + 1];
                Array.Copy(indexes, 0, parameters, 0, indexes.Length);
                parameters[parameters.Length - 1] = value;
                foreach (string fName in namesToTry)
                {
                    Invoke(obj, fName, parameters, out memberSet);

                    if (memberSet)
                        break;
                }
            }


            if (!memberSet)
                throw new Exception("Object doesn't support this property or method '" + propName + "'");

        }

        /// <summary>
        /// Sets the value of a member from an object by reflection.
        /// </summary>
        /// <param name="obj">The source object that should be checked for the property.</param>
        /// <param name="propName">The name of the property that is required.</param>
        /// <param name="value">The new value to be set.</param>
        /// <param name="expectedType">The expected type of the property.</param>
        public static void SetMember(object obj, string propName, object value, Type expectedType)
        {
            SetMember(obj, propName, null, value, expectedType);
        }

        /// <summary>
        /// Sets the value of a member from an object by reflection.
        /// </summary>
        /// <param name="obj">The source object that should be checked for the property.</param>
        /// <param name="propName">The name of the property that is required.</param>
        /// <param name="indexes">In the case that property represents an array 
        /// the index(es) must be specified here.</param>
        /// <param name="value">The new value to be set.</param>
        public static void SetMember(object obj, string propName, object[] indexes, object value)
        {
            SetMember(obj, propName, indexes, value, Type.GetType("object"));
        }

        /// <summary>
        /// Sets the value of a member from an object by reflection.
        /// </summary>
        /// <param name="obj">The source object that should be checked for the property.</param>
        /// <param name="propName">The name of the property that is required.</param>
        /// <param name="value">The new value to be set.</param>
        public static void SetMember(object obj, string propName, object value)
        {
            SetMember(obj, propName, null, value, Type.GetType("object"));
        }



        /// <summary>
        /// Invokes a member from an object by reflection.
        /// </summary>
        /// <param name="obj">The source object that should be checked for the member.</param>
        /// <param name="memberName">The name of the member.</param>
        /// <param name="parameters">An array containing the values of the parameters 
        /// to be used in the invocation.</param>
        /// <param name="types">An array containing the types for each one of the parameters.</param>
        /// <param name="Invoked">A flag to indicate if the member was sucessfully invoked.</param>
        /// <returns>The value returned by the invocation if one is returned.</returns>
        public static object Invoke(object obj, string memberName, object[] parameters, Type[] types, out bool Invoked)
        {
            object Result = null;
            Invoked = false;

            if (obj == null)
                throw new Exception("Object required");

            if (obj.GetType().IsCOMObject)
                Result = InvokeByIDispatch(obj, memberName, parameters, out Invoked);

            if (!Invoked)
            {
                MethodInfo mInfo = null;
                if (types != null)
                    mInfo = obj.GetType().GetMethod(memberName, types);
                else
                    mInfo = obj.GetType().GetMethod(memberName);

                if (mInfo != null)
                {
                    Result = mInfo.Invoke(obj, parameters);
                    Invoked = true;
                }
                else
                {
                    if (types != null)
                        mInfo = obj.GetType().GetMethod(memberName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.IgnoreCase, null, types, null);
                    else
                        mInfo = obj.GetType().GetMethod(memberName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.IgnoreCase);

                    if (mInfo != null)
                    {
                        Result = mInfo.Invoke(obj, parameters);
                        Invoked = true;
                    }
                    else
                    {
                        mInfo = obj.GetType().GetMethod(memberName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.IgnoreCase);
                        if (mInfo != null)
                        {
                            try
                            {
                                Result = mInfo.Invoke(obj, parameters);
                                Invoked = true;
                            }
                            catch { }
                        }
                    }
                }
            }

            if (!Invoked)
                throw new Exception("Object doesn't support this property or method '" + memberName + "'");

            return Result;
        }

        /// <summary>
        /// Function to invoke a member using the IDispatch interface. 
        /// Useful when the object is a ComObject wrapping another object.
        /// </summary>
        /// <param name="obj">The source object to check for the property.</param>
        /// <param name="memberName">The name of the member that should be invoked.</param>
        /// <param name="parameters">The list of parameters for the member.</param>
        /// <param name="Invoked">A flag to indicate if the member was sucessfully invoked.</param>
        /// <returns>The return value of the member if one is retrieved.</returns>
        private static object InvokeByIDispatch(object obj, string memberName, object[] parameters, out bool Invoked)
        {
            object Result = null;
            IDispatch disp = null;
            string[] memberNames = null;
            int[] dispIds = null;
            Guid guid = Guid.Empty;
            int hr = 0;
            tagDISPPARAMS tagDispParams = null;
            tagEXCEPINFO tagExceptionInfo = null;
            object[] results = null;
            int dwFlags = 0;
            List<string> namesToTry = new List<string>();

            Invoked = false;
            try
            {
                disp = obj as IDispatch;
                if (disp != null)
                {
                    namesToTry.Add(memberName);

                    memberNames = namesToTry.ToArray();
                    dispIds = new int[memberNames.Length];
                    hr = disp.GetIDsOfNames(ref guid, memberNames, memberNames.Length, -1, dispIds);
                    if (hr == 0)
                    {
                        if (parameters != null)
                            Array.Reverse(parameters);

                        tagDispParams = new tagDISPPARAMS();
                        tagDispParams.cArgs = (parameters == null) ? 0 : parameters.Length;
                        tagDispParams.rgvarg = (parameters == null) ? IntPtr.Zero : ArrayToVARIANTVector(parameters);

                        tagExceptionInfo = new tagEXCEPINFO();
                        results = new object[1];
                        dwFlags = (int)tagINVOKEKIND.INVOKE_FUNC;
                        hr = disp.Invoke(dispIds[0], ref guid, -1, dwFlags, tagDispParams, results, tagExceptionInfo, null);

                        if ((hr == 0) && (results != null) && (results.Length > 0))
                        {
                            Result = results[0];
                            Invoked = true;
                        }
                    }
                }
            }
            catch { }
            finally
            {
                if ((tagDispParams != null) && (tagDispParams.rgvarg != IntPtr.Zero))
                    FreeVariantVector(tagDispParams.rgvarg, parameters.Length);
            }

            return Result;
        }

        /// <summary>
        /// Frees the memory allocated to a Variant Vector.
        /// </summary>
        /// <param name="mem">The pointer to the variant vector.</param>
        /// <param name="len">The number of elements in the variant vector.</param>
        private static unsafe void FreeVariantVector(IntPtr mem, int len)
        {
            byte* numPtr = (byte*)mem;
            for (int i = 0; i < len; i++)
            {
                VariantClear(new HandleRef(null, (IntPtr)(numPtr + (VariantSize * i))));
            }
            Marshal.FreeCoTaskMem(mem);
        }

        /// <summary>
        /// Turns an array of objects into a array of variants, 
        /// returns the IntPtr of the array of variants.
        /// </summary>
        /// <param name="args">The list of parameters.</param>
        /// <returns>The IntPtr of the array of variants.</returns>
        private static unsafe IntPtr ArrayToVARIANTVector(object[] args)
        {
            int length = args.Length;
            IntPtr ptr = Marshal.AllocCoTaskMem(length * VariantSize);
            byte* numPtr = (byte*)ptr;
            for (int i = 0; i < length; i++)
            {
                Marshal.GetNativeVariantForObject(args[i], (IntPtr)(numPtr + (VariantSize * i)));
            }
            return ptr;
        }

        /// <summary>
        /// Invokes a member from an object by reflection.
        /// </summary>
        /// <param name="obj">The source object that should be checked for the member.</param>
        /// <param name="memberName">The name of the member.</param>
        /// <param name="parameters">An array containing the values of the parameters 
        /// to be used in the invocation.</param>
        /// <param name="Invoked">A flag to indicate if the member was sucessfully invoked.</param>
        /// <returns>The value returned by the invocation if one is returned.</returns>
        public static object Invoke(object obj, string memberName, object[] parameters, out bool Invoked)
        {
            Type[] types = null;

            if (parameters != null)
            {
                types = new Type[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                    types[i] = parameters[i].GetType();
            }

            return Invoke(obj, memberName, parameters, types, out Invoked);
        }

        /// <summary>
        /// Invokes a member from an object by reflection.
        /// </summary>
        /// <param name="obj">The source object that should be checked for the member.</param>
        /// <param name="memberName">The name of the member.</param>
        /// <param name="parameters">An array containing the values of the parameters 
        /// to be used in the invocation.</param>
        /// <returns>The value returned by the invocation if one is returned.</returns>
        public static object Invoke(object obj, string memberName, object[] parameters)
        {
            bool Invoked = false;
            return Invoke(obj, memberName, parameters, out Invoked);
        }
    }

    // Types declarations used when the object is a ComObject. 
    // Necessary if we are going to use reflection to reach members using 
    // the IDispatch interface of com objects.

    /// <summary>
    /// ITypeComp IDispatch interface.
    /// </summary>
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00020403-0000-0000-C000-000000000046")]
    internal interface ITypeComp
    {
        void Bind([MarshalAs(UnmanagedType.LPWStr)] string szName, int lHashVal, short wFlags, out ITypeInfo ppTInfo, out System.Runtime.InteropServices.ComTypes.DESCKIND pDescKind, out System.Runtime.InteropServices.ComTypes.BINDPTR pBindPtr);
        void BindType([MarshalAs(UnmanagedType.LPWStr)] string szName, int lHashVal, out ITypeInfo ppTInfo, out ITypeComp ppTComp);
    }

    /// <summary>
    /// ITypeLib IDispatch interface.
    /// </summary>
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00020402-0000-0000-C000-000000000046")]
    internal interface ITypeLib
    {
        void RemoteGetTypeInfoCount([Out, MarshalAs(UnmanagedType.LPArray)] int[] pcTInfo);
        void GetTypeInfo([In, MarshalAs(UnmanagedType.U4)] int index, [Out, MarshalAs(UnmanagedType.LPArray)] ITypeInfo[] ppTInfo);
        void GetTypeInfoType([In, MarshalAs(UnmanagedType.U4)] int index, [Out, MarshalAs(UnmanagedType.LPArray)] tagTYPEKIND[] pTKind);
        void GetTypeInfoOfGuid([In] ref Guid guid, [Out, MarshalAs(UnmanagedType.LPArray)] ITypeInfo[] ppTInfo);
        void RemoteGetLibAttr(IntPtr ppTLibAttr, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pDummy);
        void GetTypeComp([Out, MarshalAs(UnmanagedType.LPArray)] ITypeComp[] ppTComp);
        void RemoteGetDocumentation(int index, [In, MarshalAs(UnmanagedType.U4)] int refPtrFlags, [Out, MarshalAs(UnmanagedType.LPArray)] string[] pBstrName, [Out, MarshalAs(UnmanagedType.LPArray)] string[] pBstrDocString, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pdwHelpContext, [Out, MarshalAs(UnmanagedType.LPArray)] string[] pBstrHelpFile);
        void RemoteIsName([In, MarshalAs(UnmanagedType.LPWStr)] string szNameBuf, [In, MarshalAs(UnmanagedType.U4)] int lHashVal, [Out, MarshalAs(UnmanagedType.LPArray)] IntPtr[] pfName, [Out, MarshalAs(UnmanagedType.LPArray)] string[] pBstrLibName);
        void RemoteFindName([In, MarshalAs(UnmanagedType.LPWStr)] string szNameBuf, [In, MarshalAs(UnmanagedType.U4)] int lHashVal, [Out, MarshalAs(UnmanagedType.LPArray)] ITypeInfo[] ppTInfo, [Out, MarshalAs(UnmanagedType.LPArray)] int[] rgMemId, [In, Out, MarshalAs(UnmanagedType.LPArray)] short[] pcFound, [Out, MarshalAs(UnmanagedType.LPArray)] string[] pBstrLibName);
        void LocalReleaseTLibAttr();
    }

    /// <summary>
    /// tagTYPEKIND IDispatch enumeration.
    /// </summary>
    internal enum tagTYPEKIND
    {
        TKIND_ENUM,
        TKIND_RECORD,
        TKIND_MODULE,
        TKIND_INTERFACE,
        TKIND_DISPATCH,
        TKIND_COCLASS,
        TKIND_ALIAS,
        TKIND_UNION,
        TKIND_MAX
    }

    /// <summary>
    /// tagINVOKEKIND IDispatch enumeration.
    /// </summary>
    internal enum tagINVOKEKIND
    {
        INVOKE_FUNC = 1,
        INVOKE_PROPERTYGET = 2,
        INVOKE_PROPERTYPUT = 4,
        INVOKE_PROPERTYPUTREF = 8
    }

    /// <summary>
    /// ITypeInfo IDispatch interface.
    /// </summary>
    [ComImport, Guid("00020401-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ITypeInfo
    {
        [PreserveSig]
        int GetTypeAttr(ref IntPtr pTypeAttr);
        [PreserveSig]
        int GetTypeComp([Out, MarshalAs(UnmanagedType.LPArray)] ITypeComp[] ppTComp);
        [PreserveSig]
        int GetFuncDesc([In, MarshalAs(UnmanagedType.U4)] int index, ref IntPtr pFuncDesc);
        [PreserveSig]
        int GetVarDesc([In, MarshalAs(UnmanagedType.U4)] int index, ref IntPtr pVarDesc);
        [PreserveSig]
        int GetNames(int memid, [Out, MarshalAs(UnmanagedType.LPArray)] string[] rgBstrNames, [In, MarshalAs(UnmanagedType.U4)] int cMaxNames, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pcNames);
        [PreserveSig]
        int GetRefTypeOfImplType([In, MarshalAs(UnmanagedType.U4)] int index, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pRefType);
        [PreserveSig]
        int GetImplTypeFlags([In, MarshalAs(UnmanagedType.U4)] int index, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pImplTypeFlags);
        [PreserveSig]
        int GetIDsOfNames(IntPtr rgszNames, int cNames, IntPtr pMemId);
        [PreserveSig]
        int Invoke();
        [PreserveSig]
        int GetDocumentation(int memid, ref string pBstrName, ref string pBstrDocString, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pdwHelpContext, [Out, MarshalAs(UnmanagedType.LPArray)] string[] pBstrHelpFile);
        [PreserveSig]
        int GetDllEntry(int memid, tagINVOKEKIND invkind, [Out, MarshalAs(UnmanagedType.LPArray)] string[] pBstrDllName, [Out, MarshalAs(UnmanagedType.LPArray)] string[] pBstrName, [Out, MarshalAs(UnmanagedType.LPArray)] short[] pwOrdinal);
        [PreserveSig]
        int GetRefTypeInfo(IntPtr hreftype, ref ITypeInfo pTypeInfo);
        [PreserveSig]
        int AddressOfMember();
        [PreserveSig]
        int CreateInstance([In] ref Guid riid, [Out, MarshalAs(UnmanagedType.LPArray)] object[] ppvObj);
        [PreserveSig]
        int GetMops(int memid, [Out, MarshalAs(UnmanagedType.LPArray)] string[] pBstrMops);
        [PreserveSig]
        int GetContainingTypeLib([Out, MarshalAs(UnmanagedType.LPArray)] ITypeLib[] ppTLib, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pIndex);
        [PreserveSig]
        void ReleaseTypeAttr(IntPtr typeAttr);
        [PreserveSig]
        void ReleaseFuncDesc(IntPtr funcDesc);
        [PreserveSig]
        void ReleaseVarDesc(IntPtr varDesc);
    }

    /// <summary>
    /// IDispatch interface.
    /// </summary>
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00020400-0000-0000-C000-000000000046")]
    internal interface IDispatch
    {
        int GetTypeInfoCount();
        int GetTypeInfo(uint iTInfo, uint lcid, ref IntPtr TypeLib);
        [PreserveSig]
        int GetIDsOfNames([In] ref Guid riid, [In, MarshalAs(UnmanagedType.LPArray)] string[] rgszNames, [In, MarshalAs(UnmanagedType.U4)] int cNames, [In, MarshalAs(UnmanagedType.U4)] int lcid, [Out, MarshalAs(UnmanagedType.LPArray)] int[] rgDispId);
        [PreserveSig]
        int Invoke(int dispIdMember, [In] ref Guid riid, [In, MarshalAs(UnmanagedType.U4)] int lcid, [In, MarshalAs(UnmanagedType.U4)] int dwFlags, [In, Out] tagDISPPARAMS pDispParams, [Out, MarshalAs(UnmanagedType.LPArray)] object[] pVarResult, [In, Out] tagEXCEPINFO pExcepInfo, [Out, MarshalAs(UnmanagedType.LPArray)] IntPtr[] pArgErr);
    }

    /// <summary>
    /// tagDISPPARAMS IDispatch class.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal sealed class tagDISPPARAMS
    {
        public IntPtr rgvarg;
        public IntPtr rgdispidNamedArgs;
        [MarshalAs(UnmanagedType.U4)]
        public int cArgs;
        [MarshalAs(UnmanagedType.U4)]
        public int cNamedArgs;
    }

    /// <summary>
    /// tagEXCEPINFO IDispatch class.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal class tagEXCEPINFO
    {
        [MarshalAs(UnmanagedType.U2)]
        public short wCode;
        [MarshalAs(UnmanagedType.U2)]
        public short wReserved;
        [MarshalAs(UnmanagedType.BStr)]
        public string bstrSource;
        [MarshalAs(UnmanagedType.BStr)]
        public string bstrDescription;
        [MarshalAs(UnmanagedType.BStr)]
        public string bstrHelpFile;
        [MarshalAs(UnmanagedType.U4)]
        public int dwHelpContext;
        public IntPtr pvReserved = IntPtr.Zero;
        public IntPtr pfnDeferredFillIn = IntPtr.Zero;
        [MarshalAs(UnmanagedType.U4)]
        public int scode;
    }
}
