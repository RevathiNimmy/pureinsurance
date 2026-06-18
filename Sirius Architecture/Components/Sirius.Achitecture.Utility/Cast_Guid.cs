using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Sirius.Architecture.Utility {

    // All members of the class that deal with the Guid logical data type.
    partial class Cast {

        #region Public Shared Methods - Guid

        /// <overloads>
        /// Convert a guid value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a guid value from one representation to another.
        /// </summary>
        public static Guid ToGuid(Guid value, Guid defaultValue) {
            return DefaultIfNull(ToGuid(value), defaultValue);
        }

        /// <summary>
        /// Convert a guid value from one representation to another.
        /// </summary>
        public static Guid ToGuid(Guid? value, Guid defaultValue) {
            return DefaultIfNull(ToGuid(value), defaultValue);
        }

        /// <summary>
        /// Convert a guid value from one representation to another.
        /// </summary>
        public static Guid ToGuid(SqlGuid value, Guid defaultValue) {
            return DefaultIfNull(ToGuid(value), defaultValue);
        }

        /// <summary>
        /// Convert a guid value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support reading values from ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static Guid ToGuid(String value, Guid defaultValue) {
            return DefaultIfNull(ToGuid(value), defaultValue);
        }

        /// <summary>
        /// Convert a guid value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support reading values from ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static Guid ToGuid(SqlString value, Guid defaultValue) {
            return DefaultIfNull(ToGuid(value), defaultValue);
        }

        /// <summary>
        /// Convert a guid value from one representation to another.
        /// </summary>
        public static Guid ToGuid(SqlParameter value, Guid defaultValue) {
            return DefaultIfNull(ToGuid(value), defaultValue);
        }

        /// <summary>
        /// Convert a guid value from one representation to another.
        /// </summary>
        public static Guid ToGuid(Object value, Guid defaultValue) {
            return DefaultIfNull(ToGuid(value), defaultValue);
        }

        #endregion

        #region Public Shared Methods - Nullable(Of Guid)

        /// <summary>
        /// Convert a guid value from one representation to another.
        /// </summary>
        public static Guid? ToGuid(Guid value) {
            return value;
        }

        /// <summary>
        /// Convert a guid value from one representation to another.
        /// </summary>
        public static Guid? ToGuid(Guid? value) {
            return value;
        }

        /// <summary>
        /// Convert a guid value from one representation to another.
        /// </summary>
        public static Guid? ToGuid(SqlGuid value) {
            if(value.IsNull) {
                return null;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a guid value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support reading values from ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static Guid? ToGuid(String value) {
            if(value == null) {
                return null;
            } else {
                return new Guid(value);
            }
        }

        /// <summary>
        /// Convert a guid value from one representation to another.
        /// </summary>
        /// <remarks>
        /// This is not a standard or recommended conversion, it is only included to support reading values from ADO.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static Guid? ToGuid(SqlString value) {
            if(value.IsNull) {
                return null;
            } else {
                return new Guid(value.Value);
            }
        }

        /// <summary>
        /// Convert a guid value from one representation to another.
        /// </summary>
        public static Guid? ToGuid(SqlParameter value) {
            return ToGuid(value.Value);
        }

        /// <summary>
        /// Convert a guid value from one representation to another.
        /// </summary>
        public static Guid? ToGuid(Object value) {
            if(value == null || value is DBNull) {
                return null;
                // ---------- Guid ----------
            } else if(value is Guid) {
                return ToGuid((Guid) value);
            } else if(value is Guid?) {
                return ToGuid((Guid?) value);
            } else if(value is SqlGuid) {
                return ToGuid((SqlGuid) value);
                // ---------- String ----------
            } else if(value is String) {
                return ToGuid((String) value);
            } else if(value is SqlString) {
                return ToGuid((SqlString) value);
                // ---------- Parameter ----------
            } else if(value is SqlParameter) {
                return ToGuid((SqlParameter) value);
            } else {
                throw new InvalidCastException(String.Format(Properties.Resources.ErrorInvalidCast, RcwHelper.TypeName(value), typeof(Guid?)));
            }
        }

        #endregion

        #region Public Shared Methods - Object

        /// <overloads>
        /// Convert a guid value from one representation to another.
        /// </overloads>
        /// <summary>
        /// Convert a guid value from one representation to another.
        /// </summary>
        public static Object ToObjGuid(Guid? value) {
            if(!value.HasValue) {
                return DBNull.Value;
            } else {
                return value.Value;
            }
        }

        /// <summary>
        /// Convert a guid value from one representation to another.
        /// </summary>
        public static Object ToObjGuid(SqlGuid value) {
            if(value.IsNull) {
                return DBNull.Value;
            } else {
                return value.Value;
            }
        }

        #endregion

        #region Public Shared Methods - DefaultIfNull

        /// <summary>
        /// Convert a <c>NULL</c> value to the default value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>The new value.</returns>
        public static Guid DefaultIfNull(Guid? value) {
            return DefaultIfNull(value, Guid.Empty);
        }

        /// <summary>
        /// Convert a <c>NULL</c> value to the default value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return this value instead of null.</param>
        /// <returns>The new value.</returns>
        public static Guid DefaultIfNull(Guid? value, Guid defaultValue) {
            return value.GetValueOrDefault(defaultValue);
        }

        #endregion

        #region Public Shared Methods - NullIfDefault

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>The new value.</returns>
        public static Guid? NullIfDefault(Guid value) {
            return NullIfDefault(value, Guid.Empty);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static Guid? NullIfDefault(Guid value, Guid defaultValue) {
            if(value == defaultValue) {
                return null;
            } else {
                return value;
            }
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>The new value.</returns>
        public static Guid? NullIfDefault(Guid? value) {
            return NullIfDefault(value, Guid.Empty);
        }

        /// <summary>
        /// Convert the default value to a <c>NULL</c> value, leaving any other value untouched.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="defaultValue">Return null instead of this value.</param>
        /// <returns>The new value.</returns>
        public static Guid? NullIfDefault(Guid? value, Guid defaultValue) {
            if(value.GetValueOrDefault(defaultValue) == defaultValue) {
                return null;
            } else {
                return value;
            }
        }

        #endregion

    }

}
