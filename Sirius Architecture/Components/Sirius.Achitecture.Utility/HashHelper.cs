using System.ComponentModel;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// Generate a good non-colliding hash from individual hash codes, suitable for use in <c>GetHashCode</c> methods.
    /// </summary>
    /// <remarks>
    /// There are lots of different hash algorithms available. The more complex ones are better,
    /// but slower, and they require holding state from one value to the next. For the majority of uses,
    /// a simple stateless algorithm will do, and that's what is implemented here.
    /// </remarks>
    public static class HashHelper {

        // Any two prime numbers will do, as long as they are not known bad values.
        private const int _multiplier1 = 17;
        private const int _multiplier2 = 23;

        /// <summary>
        /// Begin the process of generating a hash.
        /// </summary>
        public static int Begin { get { return _multiplier1; } }

        /// <summary>
        /// Add a new hash value into the current hash.
        /// </summary>
        /// <param name="hash">Hash being generated</param>
        /// <param name="newHash">Hash value to be added</param>
        /// <returns>Updated hash</returns>
        public static int AddHash(this int hash, int newHash) {
            return unchecked(hash * _multiplier2 + newHash);
        }

        /// <summary>
        /// Add the hash of a new value into the current hash.
        /// </summary>
        /// <param name="hash">Hash being generated</param>
        /// <param name="value">Value whose hash is to be added</param>
        /// <returns>Updated hash</returns>
        /// <remarks>
        /// If the value to be added is <see langword="null"/>, then it will be treated as if its <c>GetHashCode</c>
        /// method returned zero. There is no need to check for non-null values before calling this method.
        /// This method involves a virtual method call, so it might be slower than <see cref="AddHash"/> for value types.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static int AddHashOf(this int hash, object value) {
            return AddHash(hash, value == null ? 0 : value.GetHashCode());
        }
    }
}
