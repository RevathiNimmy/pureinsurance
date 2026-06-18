using System;
using System.Reflection;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// Functions for accessing late-bound type members without the use of Option Strict Off.
    /// </summary>
    public static class LateBound {

        #region Private Constants

        private const BindingFlags BindingFlagsDefault = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase;

        #endregion

        #region Public Shared Methods - GetProperty overloads

        /// <overloads>
        /// Wrapper round the <see cref="Type.GetProperty(string, BindingFlags)"/> function that throws an exception if the property is not found.
        /// </overloads>
        /// <summary>
        /// Wrapper round the <see cref="Type.GetProperty(string, BindingFlags)"/> function that throws an exception if the property is not found.
        /// </summary>
        /// <param name="objectType">The type of the object to write the property value to.</param>
        /// <param name="propertyName">The name of the property to retrieve.</param>
        /// <returns>The <see cref="PropertyInfo"/> object.</returns>
        public static PropertyInfo GetProperty(Type objectType, String propertyName) {
            return GetProperty(objectType, propertyName, BindingFlagsDefault, null);
        }

        /// <summary>
        /// Wrapper round the <see cref="Type.GetProperty(string, BindingFlags)"/> function that throws an exception if the property is not found.
        /// </summary>
        /// <param name="objectType">The type of the object to write the property value to.</param>
        /// <param name="propertyName">The name of the property to retrieve.</param>
        /// <param name="bindingFlags">Specify how the search is conducted.</param>
        /// <returns>The <see cref="PropertyInfo"/> object.</returns>
        public static PropertyInfo GetProperty(Type objectType, String propertyName, BindingFlags bindingFlags) {
            return GetProperty(objectType, propertyName, bindingFlags, null);
        }

        /// <summary>
        /// Wrapper round the <see cref="Type.GetProperty(string, BindingFlags)"/> function that throws an exception if the property is not found.
        /// </summary>
        /// <param name="objectType">The type of the object to write the property value to.</param>
        /// <param name="propertyName">The name of the property to retrieve.</param>
        /// <param name="propertyType">Only retrieve the property if its type matches this one.</param>
        /// <returns>The <see cref="PropertyInfo"/> object.</returns>
        public static PropertyInfo GetProperty(Type objectType, String propertyName, Type propertyType) {
            return GetProperty(objectType, propertyName, BindingFlagsDefault, propertyType);
        }

        /// <summary>
        /// Wrapper round the <see cref="Type.GetProperty(string, BindingFlags)"/> function that throws an exception if the property is not found.
        /// </summary>
        /// <param name="objectType">The type of the object to write the property value to.</param>
        /// <param name="propertyName">The name of the property to retrieve.</param>
        /// <param name="bindingFlags">Specify how the search is conducted.</param>
        /// <param name="propertyType">Only retrieve the property if its type matches this one.</param>
        /// <returns>The <see cref="PropertyInfo"/> object.</returns>
        public static PropertyInfo GetProperty(Type objectType, String propertyName, BindingFlags bindingFlags, Type propertyType) {

            PropertyInfo propertyInfo = null;
            if(propertyType == null) {
                propertyInfo = objectType.GetProperty(propertyName, bindingFlags);
                if(propertyInfo == null) {
                    throw new InvalidFilterCriteriaException(String.Format(Properties.Resources.ErrorBadProperty, objectType.FullName, propertyName));
                }
            } else {
                propertyInfo = objectType.GetProperty(propertyName, bindingFlags, null, propertyType, new Type[] { }, null);
                if(propertyInfo == null) {
                    throw new InvalidFilterCriteriaException(String.Format(Properties.Resources.ErrorBadPropertyType, objectType.FullName, propertyName, propertyType.FullName));
                }
            }
            return propertyInfo;
        }

        #endregion

        #region Public Shared Methods - ArrayConvert

        /// <summary>
        /// Convert all elements in an array to an array of a different type.
        /// </summary>
        /// <typeparam name="TInput">The element type to convert from.</typeparam>
        /// <typeparam name="TOutput">The element type to convert to.</typeparam>
        /// <param name="inputArray">The original array.</param>
        /// <param name="convertFunction">The conversion function to apply to each element.</param>
        /// <returns>The converted array, or <c>null</c> if the original array is <c>null</c>.</returns>
        /// <remarks>
        /// This method works exactly the same as <see cref="Array.ConvertAll"/>, but it does not complain
        /// if the input array is <c>null</c>. This makes it useful for web service "copy out" methods.
        /// </remarks>
        public static TOutput[] ArrayConvert<TInput, TOutput>(TInput[] inputArray, Converter<TInput, TOutput> convertFunction) {

            if(inputArray == null) {
                return null;
            }

            TOutput[] outputArray = new TOutput[inputArray.Length];
            for(int i = 0; i < inputArray.Length; i++) {
                outputArray[i] = convertFunction(inputArray[i]);
            }
            return outputArray;
        }

        #endregion
    }
}
