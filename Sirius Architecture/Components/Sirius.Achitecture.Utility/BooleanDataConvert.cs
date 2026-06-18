using System;
using System.ComponentModel;
using System.Data.SqlTypes;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// Boolean database value conversion methods. Use these to interpret boolean data that is not stored
    /// in the SQL Server native format of 0 = false / 1 = true. The legacy Swift formats of 'F' = false /
    /// 'T' = true and 0 = false / -1 = true are supported, as is the native format in all possible integer
    /// data types. This class exists ONLY to support existing columns; all new columns should use the SQL
    /// Server native format for boolean values.
    /// </summary>
    public static class BooleanDataConvert {

        #region Public Shared Methods - ToBoolean (Byte)

        /// <overloads>
        /// Interpret a database integer or String value as a boolean value.
        /// </overloads>
        /// <summary>
        /// Interpret a database integer value as a boolean value (0 = false, nonzero = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static Boolean ToBoolean(Byte value) {
            return value != 0;
        }

        /// <summary>
        /// Interpret a database integer value as a boolean value (0 = false, nonzero = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static Boolean? ToBoolean(Byte? value) {
            if(!value.HasValue) {
                return null;
            } else if(value.Value == 0) {
                return false;
            } else {
                return true;
            }
        }

        /// <summary>
        /// Interpret a database integer value as a boolean value (0 = false, nonzero = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        internal static SqlBoolean ToBoolean(SqlByte value) {
            if(value.IsNull) {
                return SqlBoolean.Null;
            } else if(value.Value == 0) {
                return SqlBoolean.False;
            } else {
                return SqlBoolean.True;
            }
        }

        #endregion

        #region Public Shared Methods - ToBoolean (Int16)

        /// <summary>
        /// Interpret a database integer value as a boolean value (0 = false, nonzero = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static Boolean ToBoolean(Int16 value) {
            return value != 0;
        }

        /// <summary>
        /// Interpret a database integer value as a boolean value (0 = false, nonzero = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static Boolean? ToBoolean(Int16? value) {
            if(!value.HasValue) {
                return null;
            } else if(value.Value == 0) {
                return false;
            } else {
                return true;
            }
        }

        /// <summary>
        /// Interpret a database integer value as a boolean value (0 = false, nonzero = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        internal static SqlBoolean ToBoolean(SqlInt16 value) {
            if(value.IsNull) {
                return SqlBoolean.Null;
            } else if(value.Value == 0) {
                return SqlBoolean.False;
            } else {
                return SqlBoolean.True;
            }
        }

        #endregion

        #region Public Shared Methods - ToBoolean (Int32)

        /// <summary>
        /// Interpret a database integer value as a boolean value (0 = false, nonzero = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static Boolean ToBoolean(Int32 value) {
            return value != 0;
        }

        /// <summary>
        /// Interpret a database integer value as a boolean value (0 = false, nonzero = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static Boolean? ToBoolean(Int32? value) {
            if(!value.HasValue) {
                return null;
            } else if(value.Value == 0) {
                return false;
            } else {
                return true;
            }
        }

        /// <summary>
        /// Interpret a database integer value as a boolean value (0 = false, nonzero = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        internal static SqlBoolean ToBoolean(SqlInt32 value) {
            if(value.IsNull) {
                return SqlBoolean.Null;
            } else if(value.Value == 0) {
                return SqlBoolean.False;
            } else {
                return SqlBoolean.True;
            }
        }

        #endregion

        #region Public Shared Methods - ToBoolean (Int64)

        /// <summary>
        /// Interpret a database integer value as a boolean value (0 = false, nonzero = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static Boolean ToBoolean(Int64 value) {
            return value != 0;
        }

        /// <summary>
        /// Interpret a database integer value as a boolean value (0 = false, nonzero = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static Boolean? ToBoolean(Int64? value) {
            if(!value.HasValue) {
                return null;
            } else if(value.Value == 0) {
                return false;
            } else {
                return true;
            }
        }

        /// <summary>
        /// Interpret a database integer value as a boolean value (0 = false, nonzero = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        internal static SqlBoolean ToBoolean(SqlInt64 value) {
            if(value.IsNull) {
                return SqlBoolean.Null;
            } else if(value.Value == 0) {
                return SqlBoolean.False;
            } else {
                return SqlBoolean.True;
            }
        }

        #endregion

        #region Public Shared Methods - ToBoolean (String)

        /// <summary>
        /// Interpret a database String value as a boolean value ('F' = false, 'T' = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static Boolean? ToBoolean(String value) {
            if(value == null) {
                return null;
            } else if(value == "F") {
                return false;
            } else if(value == "T") {
                return true;
            } else {
                return null;
            }
        }

        /// <summary>
        /// Interpret a database String value as a boolean value ('F' = false, 'T' = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        internal static SqlBoolean ToBoolean(SqlString value) {
            if(value.IsNull) {
                return new SqlBoolean();
            } else if(value.Value == "F") {
                return ((SqlBoolean) (false));
            } else if(value.Value == "T") {
                return ((SqlBoolean) (true));
            } else {
                return new SqlBoolean();
            }
        }

        #endregion

        #region Public Shared Methods - ToByte

        /// <overloads>
        /// Interpret a boolean value as a database integer value (0 = false, 1 = true).
        /// </overloads>
        /// <summary>
        /// Interpret a boolean value as a database integer value (0 = false, 1 = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static Byte ToByte(Boolean value) {
            if(value) {
                return 1;
            } else {
                return 0;
            }
        }

        /// <summary>
        /// Interpret a boolean value as a database integer value (0 = false, 1 = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static Byte? ToByte(Boolean? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return ToByte(value.Value);
            }
        }

        /// <summary>
        /// Interpret a boolean value as a database integer value (0 = false, 1 = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        internal static SqlByte ToByte(SqlBoolean value) {
            if(value.IsNull) {
                return SqlByte.Null;
            } else {
                return ToByte(value.Value);
            }
        }

        #endregion

        #region Public Shared Methods - ToInt16

        /// <overloads>
        /// Interpret a boolean value as a database integer value (0 = false, 1 = true).
        /// </overloads>
        /// <summary>
        /// Interpret a boolean value as a database integer value (0 = false, 1 = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static Int16 ToInt16(Boolean value) {
            if(value) {
                return 1;
            } else {
                return 0;
            }
        }

        /// <summary>
        /// Interpret a boolean value as a database integer value (0 = false, 1 = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static Int16? ToInt16(Boolean? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return ToInt16(value.Value);
            }
        }

        /// <summary>
        /// Interpret a boolean value as a database integer value (0 = false, 1 = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        internal static SqlInt16 ToInt16(SqlBoolean value) {
            if(value.IsNull) {
                return SqlInt16.Null;
            } else {
                return ToInt16(value.Value);
            }
        }

        #endregion

        #region Public Shared Methods - ToInt32

        /// <overloads>
        /// Interpret a boolean value as a database integer value (0 = false, 1 = true).
        /// </overloads>
        /// <summary>
        /// Interpret a boolean value as a database integer value (0 = false, 1 = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static Int32 ToInt32(Boolean value) {
            if(value) {
                return 1;
            } else {
                return 0;
            }
        }

        /// <summary>
        /// Interpret a boolean value as a database integer value (0 = false, 1 = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static Int32? ToInt32(Boolean? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return ToInt32(value.Value);
            }
        }

        /// <summary>
        /// Interpret a boolean value as a database integer value (0 = false, 1 = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        internal static SqlInt32 ToInt32(SqlBoolean value) {
            if(value.IsNull) {
                return SqlInt32.Null;
            } else {
                return ToInt32(value.Value);
            }
        }

        #endregion

        #region Public Shared Methods - ToInt64

        /// <overloads>
        /// Interpret a boolean value as a database integer value (0 = false, 1 = true).
        /// </overloads>
        /// <summary>
        /// Interpret a boolean value as a database integer value (0 = false, 1 = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static Int64 ToInt64(Boolean value) {
            if(value) {
                return 1;
            } else {
                return 0;
            }
        }

        /// <summary>
        /// Interpret a boolean value as a database integer value (0 = false, 1 = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static Int64? ToInt64(Boolean? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return ToInt64(value.Value);
            }
        }

        /// <summary>
        /// Interpret a boolean value as a database integer value (0 = false, 1 = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        internal static SqlInt64 ToInt64(SqlBoolean value) {
            if(value.IsNull) {
                return SqlInt64.Null;
            } else {
                return ToInt64(value.Value);
            }
        }

        #endregion

        #region Public Shared Methods - ToString

        /// <overloads>
        /// Interpret a boolean value as a database String value ('F' = false, 'T' = true).
        /// </overloads>
        /// <summary>
        /// Interpret a boolean value as a database String value ('F' = false, 'T' = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static String ToString(Boolean value) {
            if(value) {
                return "T";
            } else {
                return "F";
            }
        }

        /// <summary>
        /// Interpret a boolean value as a database String value ('F' = false, 'T' = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static String ToString(Boolean? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Interpret a boolean value as a database String value ('F' = false, 'T' = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        internal static SqlString ToString(SqlBoolean value) {
            if(value.IsNull) {
                return SqlString.Null;
            } else {
                return ToString(value.Value);
            }
        }

        #endregion

        #region Public Shared Methods - ToVBInt16

        /// <overloads>
        /// Interpret a boolean value as a VB-compatible integer value (0 = false, -1 = true).
        /// </overloads>
        /// <summary>
        /// Interpret a boolean value as a VB-compatible integer value (0 = false, -1 = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static Int16 ToVBInt16(Boolean value) {
            if(value) {
                return -1;
            } else {
                return 0;
            }
        }

        /// <summary>
        /// Interpret a boolean value as a VB-compatible integer value (0 = false, -1 = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static Int16? ToVBInt16(Boolean? value) {
            if(!value.HasValue) {
                return null;
            } else {
                return ToVBInt16(value.Value);
            }
        }

        /// <summary>
        /// Interpret a boolean value as a VB-compatible integer value (0 = false, -1 = true).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        internal static SqlInt16 ToVBInt16(SqlBoolean value) {
            if(value.IsNull) {
                return SqlInt16.Null;
            } else {
                return ToVBInt16(value.Value);
            }
        }

        #endregion
    }
}
