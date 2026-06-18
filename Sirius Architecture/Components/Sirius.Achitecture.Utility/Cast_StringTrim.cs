using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Sirius.Architecture.Utility {

    // Extra members of the class that deal with the String logical data type, but trim the data first.
    partial class Cast {

        #region Public Shared Methods - String (Trimmed)

        /// <overloads>
        /// <summary>
        /// Convert a String value from one representation to another, and also trim leading and trailing
        /// spaces (null values and all other whitespace characters are preserved).
        /// </summary>
        /// <remarks>
        /// This method is intended for reading and writing to CHAR columns in the database.
        /// </remarks>
        /// </overloads>
        /// <summary>
        /// Convert a String value from one representation to another, and also trim leading and trailing
        /// spaces (null values and all other whitespace characters are preserved).
        /// </summary>
        /// <remarks>
        /// This method is intended for reading and writing to CHAR columns in the database.
        /// </remarks>
        public static String ToStringTrim(String value, String defaultValue) {
            return DefaultIfNull(ToStringTrim(value), defaultValue);
        }

        /// <summary>
        /// Convert a String value from one representation to another, and also trim leading and trailing
        /// spaces (null values and all other whitespace characters are preserved).
        /// </summary>
        /// <remarks>
        /// This method is intended for reading and writing to CHAR columns in the database.
        /// </remarks>
        public static String ToStringTrim(SqlString value, String defaultValue) {
            return DefaultIfNull(ToStringTrim(value), defaultValue);
        }

        /// <summary>
        /// Convert a String value from one representation to another, and also trim leading and trailing
        /// spaces (null values and all other whitespace characters are preserved).
        /// </summary>
        /// <remarks>
        /// This method is intended for reading and writing to CHAR columns in the database.
        /// </remarks>
        public static String ToStringTrim(SqlParameter value, String defaultValue) {
            return DefaultIfNull(ToStringTrim(value), defaultValue);
        }

        /// <summary>
        /// Convert a String value from one representation to another, and also trim leading and trailing
        /// spaces (null values and all other whitespace characters are preserved).
        /// </summary>
        /// <remarks>
        /// This method is intended for reading and writing to CHAR columns in the database.
        /// </remarks>
        public static String ToStringTrim(Object value, String defaultValue) {
            return DefaultIfNull(ToStringTrim(value), defaultValue);
        }

        #endregion

        #region Public Shared Methods - String (Trimmed)

        /// <summary>
        /// Convert a String value from one representation to another, and also trim leading and trailing
        /// spaces (null values and all other whitespace characters are preserved).
        /// </summary>
        /// <remarks>
        /// This method is intended for reading and writing to CHAR columns in the database.
        /// </remarks>
        public static String ToStringTrim(String value) {
            return Trim(ToString(value));
        }

        /// <summary>
        /// Convert a String value from one representation to another, and also trim leading and trailing
        /// spaces (null values and all other whitespace characters are preserved).
        /// </summary>
        /// <remarks>
        /// This method is intended for reading and writing to CHAR columns in the database.
        /// </remarks>
        public static String ToStringTrim(SqlString value) {
            return Trim(ToString(value));
        }

        /// <summary>
        /// Convert a String value from one representation to another, and also trim leading and trailing
        /// spaces (null values and all other whitespace characters are preserved).
        /// </summary>
        /// <remarks>
        /// This method is intended for reading and writing to CHAR columns in the database.
        /// </remarks>
        public static String ToStringTrim(SqlParameter value) {
            return Trim(ToString(value));
        }

        /// <summary>
        /// Convert a String value from one representation to another, and also trim leading and trailing
        /// spaces (null values and all other whitespace characters are preserved).
        /// </summary>
        /// <remarks>
        /// This method is intended for reading and writing to CHAR columns in the database.
        /// </remarks>
        public static String ToStringTrim(Object value) {
            return Trim(ToString(value));
        }

        #endregion

        #region Public Shared Methods - Object (Trimmed)

        /// <overloads>
        /// <summary>
        /// Convert a String value from one representation to another, and also trim leading and trailing
        /// spaces (null values and all other whitespace characters are preserved).
        /// </summary>
        /// <remarks>
        /// This method is intended for reading and writing to CHAR columns in the database.
        /// </remarks>
        /// </overloads>
        /// <summary>
        /// Convert a String value from one representation to another, and also trim leading and trailing
        /// spaces (null values and all other whitespace characters are preserved).
        /// </summary>
        /// <remarks>
        /// This method is intended for reading and writing to CHAR columns in the database.
        /// </remarks>
        public static Object ToObjStringTrim(String value) {
            if(value == null) {
                return DBNull.Value;
            } else {
                return Trim(value);
            }
        }

        /// <summary>
        /// Convert a String value from one representation to another, and also trim leading and trailing
        /// spaces (null values and all other whitespace characters are preserved).
        /// </summary>
        /// <remarks>
        /// This method is intended for reading and writing to CHAR columns in the database.
        /// </remarks>
        public static Object ToObjStringTrim(SqlString value) {
            if(value.IsNull) {
                return DBNull.Value;
            } else {
                return Trim(value.Value);
            }
        }

        #endregion

        #region Private Shared Methods - Trim

        /// <summary>
        /// Trim leading and trailing spaces from a value. Null values and all other whitespace characters are preserved.
        /// </summary>
        /// <remarks>
        /// This method is intended for reading and writing to CHAR columns in the database.
        /// </remarks>
        private static String Trim(String value) {
            if(value == null) {
                return value;
            } else {
                return value.Trim(' ');
            }
        }

        /// <summary>
        /// Trim leading and trailing spaces from a value. Null values and all other whitespace characters are preserved.
        /// </summary>
        /// <remarks>
        /// This method is intended for reading and writing to CHAR columns in the database.
        /// </remarks>
        private static SqlString Trim(SqlString value) {
            if(value.IsNull) {
                return value;
            } else {
                return new SqlString(value.Value.Trim(' '));
            }
        }

        #endregion

    }

}
