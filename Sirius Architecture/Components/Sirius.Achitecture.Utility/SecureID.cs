using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// Generate a new GUID using a cryptographically-secure random number generator so that the result cannot be predicted.
    /// </summary>
    public static class SecureID {

        #region Private Constants

        private enum GuidVariant {
            ReservedNCS = 0x00,
            Standard = 0x02,
            ReservedMicrosoft = 0x06,
            ReservedFuture = 0x07
        }

        private enum GuidVersion {
            MACAddress = 0x01,
            DCESecurity = 0x02,
            MD5Hash = 0x03,
            Random = 0x04,
            SHA1Hash = 0x5
        }

        private static class GuidBytes {
            public const int VariantIndex = 8;
            public const int VariantMask = 0x3f;
            public const int VariantShift = 6;
            public const int VersionIndex = 7;
            public const int VersionMask = 0x0f;
            public const int VersionShift = 4;
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Generate a new GUID using a cryptographically-secure random number generator so that the result cannot be predicted.
        /// </summary>
        public static Guid NewGuid() {

            // Fill a byte array of the same length as a GUID with cryptographically random data.
            var bytes = new byte[Marshal.SizeOf(typeof(Guid))];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);

            // Set the bit flags correctly to indicate how the data was generated.
            bytes[GuidBytes.VariantIndex] &= GuidBytes.VariantMask;
            bytes[GuidBytes.VariantIndex] |= ((int) GuidVariant.Standard << GuidBytes.VariantShift);
            bytes[GuidBytes.VersionIndex] &= GuidBytes.VersionMask;
            bytes[GuidBytes.VersionIndex] |= ((int) GuidVersion.Random << GuidBytes.VersionShift);

            return new Guid(bytes);
        }

        #endregion
    }
}
