using System;
using System.Globalization;
using System.Text;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// Hex binary conversion methods. These are only implemented here because the .NET BCL contains several incompatible
    /// implementations of binhex encoding, all of them hidden, apart from a couple that make you jump through hoops to
    /// call them.
    /// </summary>
    /// <remarks>
    /// If you want Base64 encoding, use methods on the .NET BCL <see cref="Convert"/> class instead.
    /// </remarks>
    public static class BinHexConvert {

        #region Public Shared Methods - ToByteArray

        /// <summary>
        /// Parse a String of hex digits into a Byte array.
        /// </summary>
        /// <param name="value">The text to parse.</param>
        /// <returns>The Byte array equivalent.</returns>
        /// <exception cref="FormatException">One of the characters is not a legal hex digit.</exception>
        /// <exception cref="OverflowException">One of the character pairs represents a number that will not fit in one Byte.</exception>
        /// <remarks>
        /// If the number of hex digits is odd, a missing leading zero on the first Byte is assumed.
        /// </remarks>
        public static Byte[] ToByteArray(String value) {

            // Propogate null correctly.
            if(value == null) {
                return null;
            }

            // If the number of characters is odd, just assume that the first Byte is missing a leading zero.
            // This is simpler and easier than throwing a nasty exception.
            value = value.Trim();
            if(value.Length % 2 > 0) {
                value = "0" + value;
            }

            // Parse each pair of characters into a Byte value.
            int byteCount = value.Length / 2;
            Byte[] byteArray = new Byte[byteCount];
            for(int byteIndex = 0; byteIndex <= byteCount - 1; byteIndex++) {
                byteArray[byteIndex] = Byte.Parse(value.Substring(byteIndex * 2, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
            }
            return byteArray;
        }

        #endregion

        #region Public Shared Methods - ToString

        /// <summary>
        /// Format a Byte array as a String of hex digits.
        /// </summary>
        /// <param name="value">The Byte array to format.</param>
        /// <returns>The text equivalent.</returns>
        public static String ToString(Byte[] value) {

            // Propogate null correctly.
            if(value == null) {
                return null;
            }

            // Format the bytes correctly and append them to a String.
            StringBuilder text = new StringBuilder(value.Length * 2);
            foreach(Byte valueElement in value) {
                text.Append(valueElement.ToString("X2", CultureInfo.InvariantCulture));
            }
            return text.ToString();
        }

        #endregion
    }
}
