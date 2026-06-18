using System.Collections.Generic;
using System.Text;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// Useful extension methods for collections.
    /// </summary>
    public static class CollectionHelper {

        #region Public Static Methods - Dictionary.ItemOrDefault

        /// <summary>
        /// Safe alternative to the standard <c>Item</c> property, that returns a default value if the key is not found.
        /// </summary>
        /// <typeparam name="TKey">Dictionary key type</typeparam>
        /// <typeparam name="TValue">Dictionary value type</typeparam>
        /// <param name="dictionary">Dictionary to query</param>
        /// <param name="key">Key to find</param>
        /// <returns>Key value or default value</returns>
        public static TValue ItemOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) {
            return ItemOrDefault(dictionary, key, default(TValue));
        }

        /// <summary>
        /// Safe alternative to the standard <c>Item</c> property, that returns a default value if the key is not found.
        /// </summary>
        /// <typeparam name="TKey">Dictionary key type</typeparam>
        /// <typeparam name="TValue">Dictionary value type</typeparam>
        /// <param name="dictionary">Dictionary to query</param>
        /// <param name="key">Key to find</param>
        /// <param name="defaultValue">Value to return if key is not found</param>
        /// <returns>Key value or default value</returns>
        public static TValue ItemOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue) {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        #endregion

        #region Public Static Methods - String.Join

        /// <summary>
        /// Join a collection of strings together with separators between each item.
        /// </summary>
        /// <param name="items">Items to join</param>
        /// <param name="separator">Separator</param>
        /// <returns>Complete list</returns>
        public static string Join(this IEnumerable<string> items, string separator) {
            var text = new StringBuilder();
            var first = true;
            foreach(var item in items) {
                if(first) {
                    first = false;
                } else {
                    text.Append(separator);
                }
                text.Append(item);
            }
            return text.ToString();
        }

        #endregion
    }
}
