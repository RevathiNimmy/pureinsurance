using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Sirius.Architecture.Utility {

    // TODO: Rename the EnumFields class to Enum2.

    /// <summary>
    /// Utility methods for enums.
    /// </summary>
    public static partial class EnumFields {

        #region Private Constants

        private const string ResourceManagerWarning = "Use a localisable Description attribute instead, " +
            "e.g. Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.ResourceDescriptionAttribute.";

        #endregion

        #region Public Extension Methods

        /// <summary>
        /// Given an enum value, if a <see cref="DescriptionAttribute"/> attribute has been defined on it, then return that.
        /// Otherwise return the enum name.
        /// </summary>
        /// <typeparam name="T">Enum type to look in</typeparam>
        /// <param name="value">Enum value</param>
        /// <returns>Description or name</returns>
        /// <remarks>
        /// To localise the description text, use a description attribute that takes its value from a resource, e.g.
        /// <c>Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.ResourceDescriptionAttribute</c>.
        /// </remarks>
        public static string ToDescription<T>(this T value) where T : struct {
            if(!typeof(T).IsEnum) {
                throw new ArgumentException(Properties.Resources.TypeIsNotAnEnum);
            }
            var fn = Enum.GetName(typeof(T), value);
            if(fn == null) {
                return string.Empty;
            }
            var fi = typeof(T).GetField(fn, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static);
            if(fi == null) {
                return string.Empty;
            }
            return GetDescription(fi);
        }

        /// <summary>
        /// Given a nullable enum value, if a <see cref="DescriptionAttribute"/> attribute has been defined on it, then return that.
        /// Otherwise return the enum name. If the value is null, return an empty string.
        /// </summary>
        /// <typeparam name="T">Enum type to look in</typeparam>
        /// <param name="value">Enum value</param>
        /// <returns>Description or name or empty string</returns>
        /// <remarks>
        /// To localise the description text, use a description attribute that takes its value from a resource, e.g.
        /// <c>Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.ResourceDescriptionAttribute</c>.
        /// </remarks>
        public static string ToDescription<T>(this T? value) where T : struct {
            return value.HasValue ? ToDescription(value.Value) : string.Empty;
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// A more useful implementation of <see cref="Enum.Parse(Type, String)"/>.
        /// </summary>
        /// <typeparam name="T">Enum type to act on</typeparam>
        /// <param name="value">String to parse</param>
        /// <param name="ignoreCase">Ignore case?</param>
        /// <returns>Enum value, or null if the string is null or empty</returns>
        public static T? Parse<T>(string value, bool ignoreCase = false) where T : struct {
            if(!typeof(T).IsEnum) {
                throw new ArgumentException(Properties.Resources.TypeIsNotAnEnum);
            }
            if(string.IsNullOrEmpty(value)) {
                return null;
            }
            return (T) Enum.Parse(typeof(T), value, ignoreCase);
        }

        /// <summary>
        /// List all values in an enum, along with their human-readable descriptions. The data is returned in
        /// a format suitable for data binding. The human-readable description is specified with the
        /// <see cref="DescriptionAttribute"/> attribute.
        /// If not present, the field name is used instead.
        /// </summary>
        /// <param name="type">The enum type to bind to. This also works for any other type that contains public static fields.</param>
        /// <returns>A list of Description/Value pairs, with one row per public static field in the specified type.</returns>
        public static IEnumerable<BindingInfo> GetBindingList(Type type) {
            return GetEnumFields(type).Select(fi => new BindingInfo(GetDescription(fi), fi.GetValue(null)));
        }

        /// <summary>
        /// OBSOLETE
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(ResourceManagerWarning)]
        public static IEnumerable<BindingInfo> GetBindingList(Type type, ResourceManager resourceManager) {
            return GetEnumFields(type).Select(fi => new BindingInfo(GetDescription(fi, resourceManager, null), fi.GetValue(null)));
        }

        /// <summary>
        /// OBSOLETE
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(ResourceManagerWarning)]
        public static IEnumerable<BindingInfo> GetBindingList(Type type, ResourceManager resourceManager, CultureInfo culture) {
            return GetEnumFields(type).Select(fi => new BindingInfo(GetDescription(fi, resourceManager, culture), fi.GetValue(null)));
        }

        #endregion

        #region Private Static Methods

        private static FieldInfo[] GetEnumFields(Type type) {
            return type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static);
        }

        private static string GetDescription(FieldInfo fi) {
            var da = (DescriptionAttribute) fi.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
            return da != null ? da.Description : fi.Name;
        }

        // Obsolete
        private static string GetDescription(FieldInfo fi, ResourceManager rm, CultureInfo culture) {
            var da = (DescriptionAttribute) fi.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
            var rn = da != null ? da.Description : fi.ReflectedType.Name + fi.Name;
            return rm.GetString(rn, culture);
        }

        #endregion
    }
}
