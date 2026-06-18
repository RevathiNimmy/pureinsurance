using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Artinsoft.VB6.Utils
{
    /// <summary>
    /// The DefaultPropHelper resolves late binding access to DefaultProp members.
    /// </summary>
    public class DefaultPropHelper
    {
        /// <summary>
        /// The default properties loaded from DefaultProperties.xml resource file
        /// </summary>
        private static Dictionary<string, string> defaultProps = null;

        /// <summary>
        /// Obtains the default property from 'obj' and sets the 'propertyValue' to it.
        /// </summary>
        /// <param name="obj">The object to be set the 'propertyValue'.</param>
        /// <param name="propertyValue">The value to be set to the default property of the 'obj'.</param>
        /// <param name="index">The arguments to invoke the default property if needed.</param>
        public static void SetDefaultProperty(ref object obj, object propertyValue, params object[] index)
        {
            try
            {
                PropertyInfo pInfo = null;
                Type type = (obj==null)?null:obj.GetType();
                if (IsPrimitive(type))
                {
                    obj = propertyValue;
                    return;
                }
                else if (IsIntrinsic(type))
                    pInfo = (PropertyInfo)ObtainDefaultPropertyFromIntrinsic(type, true);
                else if (IsCOMObject(type))
                    pInfo = (PropertyInfo)ObtainDefaultPropertyFromCOM(obj);
                else
                    pInfo = (PropertyInfo)ObtainDefaultPropertyFromInternalClass(obj);

                if (IsPrimitive(pInfo.PropertyType))
                    pInfo.SetValue(obj, GetValueForcedToType(propertyValue, pInfo.PropertyType), index);
                else
                {
                    object newObj = pInfo.GetValue(obj, new object[] { });
                    SetDefaultProperty(ref newObj, propertyValue, index);
                }
            }
            catch (Exception e)
            {
                throw new DefaultPropertyException("Error setting Default Property", e);
            }
        }

        /// <summary>
        /// Obtains the default property value from the 'obj'.
        /// </summary>
        /// <param name="obj">The object to get the default property.</param>
        /// <typeparam name="T">The type, which the default property value must be casted.</typeparam>
        /// <param name="index">The arguments to invoke the default property if needed.</param>
        /// <returns>The default property value obtained from the 'obj'</returns>
        public static T GetDefaultProperty<T>(object obj, params object[] index)
        {
            try
            {
                MemberInfo pInfo = null;
                Type type = (obj == null) ? null : obj.GetType();
                if (IsPrimitive(type))
                    return (T)GetValueForcedToType(obj, typeof(T));
                else if (IsIntrinsic(type))
                    pInfo = ObtainDefaultPropertyFromIntrinsic(type, false);
                else if (IsCOMObject(type))
                    pInfo = ObtainDefaultPropertyFromCOM(obj);
                else
                    pInfo = ObtainDefaultPropertyFromInternalClass(obj);

                object noCastedValue = null;
                object[] parameters = new object[] { };
                bool isAlreadyPrimitive = false;

                if (pInfo is PropertyInfo)
                    isAlreadyPrimitive = IsPrimitive(((PropertyInfo)pInfo).PropertyType);
                else
                    isAlreadyPrimitive = IsPrimitive(((MethodInfo)pInfo).ReturnType);
                if (isAlreadyPrimitive) parameters = index;

                if (pInfo is PropertyInfo)
                    noCastedValue = ((PropertyInfo)pInfo).GetValue(obj, parameters);
                else
                    noCastedValue = ((MethodInfo)pInfo).Invoke(obj, parameters);

                if (isAlreadyPrimitive)
                    return (T)GetValueForcedToType(noCastedValue, typeof(T));
                else
                    return GetDefaultProperty<T>(noCastedValue, index);
            }
            catch (Exception e)
            {
                throw new DefaultPropertyException("Error getting Default Property", e);
            }
        }

        /// <summary>
        /// Obtains the default property value from the 'obj'.
        /// </summary>
        /// <param name="obj">The object to get the default property.</param>
        /// <param name="index">The arguments to invoke the default property if needed.</param>
        /// <returns>The default property value obtained from the 'obj'</returns>
        public static object GetDefaultProperty(object obj, params object[] index)
        {
            return GetDefaultProperty<object>(obj, index);
        }

        /// <summary>
        /// Indicates if 'type' is a Primitive Type.
        /// </summary>
        /// <param name="type">The 'System.Type' to be checked.</param>
        /// <returns>True if the 'type' is a primitive Type.</returns>
        private static bool IsPrimitive(Type type)
        {
            return (type == null || type.IsPrimitive || type.IsEnum || (type.FullName == "System.String") || (type.FullName == "System.Object"));
        }

        /// <summary>
        /// Indicates if 'type' is a Intrisic Type, it means it is not a COM object and it has a default property.
        /// </summary>
        /// <param name="type">The 'System.Type' to be checked.</param>
        /// <returns>True if the 'type' is a intrinsic Type.</returns>
        private static bool IsIntrinsic(Type type)
        {
            if (defaultProps == null) LoadDefaultProps();
            return defaultProps.ContainsKey(type.FullName) ||
                   defaultProps.ContainsKey(type.FullName + ".Get") ||
                   defaultProps.ContainsKey(type.FullName + ".Set");
        }

        /// <summary>
        /// Indicates if 'type' is a COMObject or a wrapped ActiveX object (AxHost).
        /// </summary>
        /// <param name="type">The 'System.Type' to be checked.</param>
        /// <returns>True if the 'type' is a COMObject Type.</returns>
        private static bool IsCOMObject(Type type)
        {
            return (type.IsCOMObject || ((type.BaseType!=null) && (type.BaseType.FullName == "System.Windows.Forms.AxHost")));
        }

        /// <summary>
        /// Gets the indicated 'propertyName' in the 'type'.
        /// </summary>
        /// <param name="type">The 'System.Type' where to look for.</param>
        /// <param name="propertyName">The name of the property to look for.</param>
        /// <returns>A 'MemberInfo' containing the indicated default property.</returns>
        private static MemberInfo ObtainProperty(Type type, string propertyName)
        {
            return type.GetMember(propertyName)[0];
        }

        /// <summary>
        /// Gets the default property in an Intrinsic type.
        /// </summary>
        /// <param name="type">The 'System.Type' to look for.</param>
        /// <param name="theSetProperty">Indicates if look for the Set/Get default property.</param>
        /// <returns>A 'MemberInfo' containing the default property.</returns>
        private static MemberInfo ObtainDefaultPropertyFromIntrinsic(Type type, bool theSetProperty)
        {
            if (defaultProps.ContainsKey(type.FullName))
                return ObtainProperty(type, defaultProps[type.FullName]);
            else
                return ObtainProperty(type, defaultProps[type.FullName] + (theSetProperty ? ".Set" : ".Get"));
        }

        /// <summary>
        /// Gets the default property in a COMObject type.
        /// </summary>
        /// <param name="obj">The 'obj' where to look for.</param>
        /// <returns>A 'MemberInfo' containing the default property.</returns>
        private static MemberInfo ObtainDefaultPropertyFromCOM(object obj)
        {
            MemberInfo pInfo = null;

            Type type = obj.GetType();
            if ((type.BaseType != null) && (type.BaseType.FullName == "System.Windows.Forms.AxHost"))
            {
                object ocxObj = ((AxHost)obj).GetOcx();
                IDispatch ocxIDisp = (IDispatch)ocxObj;
                IntPtr ocxTypeInfo = IntPtr.Zero;
                ocxIDisp.GetTypeInfo(0, 0, ref ocxTypeInfo);
                type = Marshal.GetTypeForITypeInfo(ocxTypeInfo);
            }
            MemberInfo[] mInfo = type.GetDefaultMembers();
            if (mInfo.Length == 1)
            {
                pInfo = ObtainProperty(obj.GetType(), mInfo[0].Name);
            }
            else
            {
                throw new DefaultPropertyException("Default Property for object '" + obj.ToString() + "' was not found");
            }
            return pInfo;
        }


        /// <summary>
        /// Gets the default property in a user internal class.
        /// </summary>
        /// <param name="obj">The 'obj' where to look for.</param>
        /// <returns>A 'MemberInfo' containing the default property or null if it is not found.</returns>
        private static MemberInfo ObtainDefaultPropertyFromInternalClass(object obj)
        {
            MemberInfo pInfo = null;

            Type type = obj.GetType();
            MemberInfo[] mInfo = type.GetDefaultMembers();
            if (mInfo.Length == 1)
            {
                pInfo = ObtainProperty(type, mInfo[0].Name);
            }
            return pInfo;
        }

        /// <summary>
        /// Loads the 'DefaultProperties.xml' resource file containing the intrinsic default properties.
        /// </summary>
        private static void LoadDefaultProps()
        {
            defaultProps = new Dictionary<string,string>();
            try
            {
                XmlDocument defPropsXML = new XmlDocument();
                Assembly theAssembly = Assembly.GetExecutingAssembly();
                defPropsXML.Load(theAssembly.GetManifestResourceStream(theAssembly.FullName.Substring(0,theAssembly.FullName.IndexOf(',')) + ".Artinsoft.VB6.Utils.DefaultProperties.xml"));
                if (null != defPropsXML)
                {
                    foreach (XmlNode node in defPropsXML.GetElementsByTagName("DefaultProperty"))
                    {
                        defaultProps.Add(node.Attributes["FullName"].Value, node.InnerText);
                    }
                }
            }
            catch (Exception e)
            {
                throw new DefaultPropertyException("Error Loading Default Property file", e);
            }
        }

        /// <summary>
        /// Converts the 'propertyValue' instance to 'targetType'.
        /// </summary>
        /// <param name="propertyValue">The value to be converted.</param>
        /// <param name="targetType">The Type to be converted.</param>
        /// <returns>A value converted to 'targetType'.</returns>
        private static object GetValueForcedToType(object propertyValue, Type targetType)
        {
            if (targetType.FullName == "System.Object")
                return propertyValue;
            else if (targetType.FullName == "System.String")
                return GetValueForcedToString(propertyValue);
            else if (targetType.FullName == "System.Double")
                return GetValueForcedToDouble(propertyValue);
            else if (targetType.FullName == "System.Float")
                return GetValueForcedToFloat(propertyValue);
            else if (targetType.FullName == "System.Int64")
                return GetValueForcedToLong(propertyValue);
            else if (targetType.FullName == "System.Int32")
                return GetValueForcedToInt(propertyValue);
            else if (targetType.FullName == "System.Int16")
                return GetValueForcedToShort(propertyValue);
            else if (targetType.FullName == "System.Byte")
                return GetValueForcedToByte(propertyValue);
            else if (targetType.FullName == "System.Boolean")
                return GetValueForcedToBoolean(propertyValue);
            else if (targetType.FullName == "System.Char")
                return GetValueForcedToChar(propertyValue);
            else if (targetType.FullName == "System.DateTime")
                return GetValueForcedToDate(propertyValue);
            else if (targetType.IsArray)
                return GetValueForcedToArray(propertyValue);
            else if (targetType.IsClass)
                return GetValueForcedToClass(propertyValue);
            else if (targetType.IsEnum)
                return GetValueForcedToEnum(propertyValue);
            
            // No Identified Type
            return propertyValue;
        }

        /// <summary>
        /// Converts the 'propertyValue' instance to string.
        /// </summary>
        /// <param name="propertyValue">The value to be converted.</param>
        /// <returns>A value converted to string.</returns>
        private static string GetValueForcedToString(object propertyValue)
        {
            Type sourceType = propertyValue.GetType();
            if ((sourceType.FullName == "System.Double") || (sourceType.FullName == "System.Float") ||
                (sourceType.FullName == "System.Int64") || (sourceType.FullName == "System.Int32") ||
                (sourceType.FullName == "System.Int16") || (sourceType.FullName == "System.Byte") ||
                (sourceType.FullName == "System.Boolean"))
                return propertyValue.ToString();
            else if (sourceType.FullName == "System.DateTime")
                return DateTimeHelper.ToString((DateTime)propertyValue);
            else if ((sourceType.IsArray) && (sourceType.GetElementType().FullName == "System.Byte"))
                return StringsHelper.ByteArrayToString((byte[])propertyValue);
            else if (sourceType.IsEnum)
                return ((int)propertyValue).ToString();

            return Convert.ToString(propertyValue);
        }

        /// <summary>
        /// Converts the 'propertyValue', which is a boolean instance to numeric.
        /// </summary>
        /// <param name="propertyValue">The value to be converted.</param>
        /// <returns>A boolean value converted to numeric.</returns>
        private static object GetBooleanValueForcedToNumeric(object propertyValue)
        {
            return (((bool)propertyValue) ? -1 : 0);
        }

        /// <summary>
        /// Converts the 'propertyValue', which is a date instance to numeric.
        /// </summary>
        /// <param name="propertyValue">The value to be converted.</param>
        /// <returns>A date value converted to numeric.</returns>
        private static object GetDateValueForcedToNumeric(object propertyValue)
        {
            return ((DateTime)propertyValue).ToOADate();
        }

        /// <summary>
        /// Converts the 'propertyValue' instance to double.
        /// </summary>
        /// <param name="propertyValue">The value to be converted.</param>
        /// <returns>A value converted to double.</returns>
        private static double GetValueForcedToDouble(object propertyValue)
        {
            Type sourceType = propertyValue.GetType();
            if (sourceType.FullName == "System.String")
                return Double.Parse((string)propertyValue);
            else if (sourceType.FullName == "System.Boolean")
                return (double)GetBooleanValueForcedToNumeric(propertyValue);
            else if (sourceType.FullName == "System.DateTime")
                return (double)GetDateValueForcedToNumeric(propertyValue);

            return Convert.ToDouble(propertyValue);
        }

        /// <summary>
        /// Converts the 'propertyValue' instance to float.
        /// </summary>
        /// <param name="propertyValue">The value to be converted.</param>
        /// <returns>A value converted to float.</returns>
        private static float GetValueForcedToFloat(object propertyValue)
        {
            Type sourceType = propertyValue.GetType();
            if (sourceType.FullName == "System.String")
                return Convert.ToInt16(Double.Parse((string)propertyValue));
            else if (sourceType.FullName == "System.Boolean")
                return (float)GetBooleanValueForcedToNumeric(propertyValue);
            else if (sourceType.FullName == "System.DateTime")
                return (float)GetDateValueForcedToNumeric(propertyValue);

            return Convert.ToSingle(propertyValue);
        }

        /// <summary>
        /// Converts the 'propertyValue' instance to long.
        /// </summary>
        /// <param name="propertyValue">The value to be converted.</param>
        /// <returns>A value converted to long.</returns>
        private static long GetValueForcedToLong(object propertyValue)
        {
            Type sourceType = propertyValue.GetType();
            if (sourceType.FullName == "System.String")
                return System.Convert.ToInt32((string)propertyValue);
            else if (sourceType.FullName == "System.Boolean")
                return (long)GetBooleanValueForcedToNumeric(propertyValue);
            else if (sourceType.FullName == "System.DateTime")
                return (long)GetDateValueForcedToNumeric(propertyValue);

            return Convert.ToInt64(propertyValue);
        }

        /// <summary>
        /// Converts the 'propertyValue' instance to int.
        /// </summary>
        /// <param name="propertyValue">The value to be converted.</param>
        /// <returns>A value converted to int.</returns>
        private static int GetValueForcedToInt(object propertyValue)
        {
            Type sourceType = propertyValue.GetType();
            if (sourceType.FullName == "System.String")
                return Convert.ToInt32(Double.Parse((string)propertyValue));
            else if (sourceType.FullName == "System.Boolean")
                return (int)GetBooleanValueForcedToNumeric(propertyValue);
            else if (sourceType.FullName == "System.DateTime")
                return (int)GetDateValueForcedToNumeric(propertyValue);

            return Convert.ToInt32(propertyValue);
        }

        /// <summary>
        /// Converts the 'propertyValue' instance to short.
        /// </summary>
        /// <param name="propertyValue">The value to be converted.</param>
        /// <returns>A value converted to short.</returns>
        private static short GetValueForcedToShort(object propertyValue)
        {
            Type sourceType = propertyValue.GetType();
            if (sourceType.FullName == "System.String")
                return Convert.ToInt16(Double.Parse((string)propertyValue));
            else if (sourceType.FullName == "System.Boolean")
                return (short)GetBooleanValueForcedToNumeric(propertyValue);
            else if (sourceType.FullName == "System.DateTime")
                return (short)GetDateValueForcedToNumeric(propertyValue);

            return Convert.ToInt16(propertyValue);
        }

        /// <summary>
        /// Converts the 'propertyValue' instance to byte.
        /// </summary>
        /// <param name="propertyValue">The value to be converted.</param>
        /// <returns>A value converted to byte.</returns>
        private static byte GetValueForcedToByte(object propertyValue)
        {
            Type sourceType = propertyValue.GetType();
            if (sourceType.FullName == "System.String")
                return Convert.ToByte(Double.Parse((string)propertyValue));
            else if (sourceType.FullName == "System.Boolean")
                return (byte)GetBooleanValueForcedToNumeric(propertyValue);
            else if (sourceType.FullName == "System.DateTime")
                return (byte)GetDateValueForcedToNumeric(propertyValue);

            return Convert.ToByte(propertyValue);
        }

        /// <summary>
        /// Converts the 'propertyValue' instance to bool.
        /// </summary>
        /// <param name="propertyValue">The value to be converted.</param>
        /// <returns>A value converted to bool.</returns>
        private static bool GetValueForcedToBoolean(object propertyValue)
        {
            return Convert.ToBoolean(propertyValue);
        }

        /// <summary>
        /// Converts the 'propertyValue' instance to char.
        /// </summary>
        /// <param name="propertyValue">The value to be converted.</param>
        /// <returns>A value converted to char.</returns>
        private static char GetValueForcedToChar(object propertyValue)
        {
            return (char)propertyValue;
        }

        /// <summary>
        /// Converts the 'propertyValue' instance to datetime.
        /// </summary>
        /// <param name="propertyValue">The value to be converted.</param>
        /// <returns>A value converted to datetime.</returns>
        private static DateTime GetValueForcedToDate(object propertyValue)
        {
            Type sourceType = propertyValue.GetType();
            if (sourceType.FullName == "System.Boolean")
                return new DateTime(Convert.ToInt64(propertyValue));
            else if ((sourceType.FullName == "System.Double") || (sourceType.FullName == "System.Float") ||
                (sourceType.FullName == "System.Int64") || (sourceType.FullName == "System.Int32") ||
                (sourceType.FullName == "System.Int16") || (sourceType.FullName == "System.Byte") ||
                (sourceType.FullName == "System.Boolean"))
                return DateTime.FromOADate((double)propertyValue);
            else if (sourceType.FullName == "System.String")
            {
                DateTime dValue;
                return DateTime.TryParse((string)propertyValue, out dValue) ? dValue : DateTime.FromOADate(Double.Parse((string)propertyValue));
            }
            return Convert.ToDateTime(propertyValue);
        }

        /// <summary>
        /// Converts the 'propertyValue' instance to array.
        /// </summary>
        /// <param name="propertyValue">The value to be converted.</param>
        /// <returns>A value converted to array.</returns>
        private static object GetValueForcedToArray(object propertyValue)
        {
            return propertyValue;
        }

        /// <summary>
        /// Converts the 'propertyValue' instance to a class.
        /// </summary>
        /// <param name="propertyValue">The value to be converted.</param>
        /// <returns>A value converted to a class.</returns>
        private static object GetValueForcedToClass(object propertyValue)
        {
            return propertyValue;
        }

        /// <summary>
        /// Converts the 'propertyValue' instance to a enum.
        /// </summary>
        /// <param name="propertyValue">The value to be converted.</param>
        /// <returns>A value converted to a enum.</returns>
        private static object GetValueForcedToEnum(object propertyValue)
        {
            return propertyValue;
        }

    }

    /// <summary>
    /// Represents errors that occur during Default Property handling.
    /// </summary>
    public class DefaultPropertyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of a DefaulPropertyException class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DefaultPropertyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of a DefaulPropertyException class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception tha tis the cause of the current exception
        /// or a null reference (Nothing in Visual Basic) if no inner exception is specified.
        /// </param>
        public DefaultPropertyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
}
