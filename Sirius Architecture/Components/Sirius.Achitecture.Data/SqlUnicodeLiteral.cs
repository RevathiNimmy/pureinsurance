using Sirius.Architecture.Utility;
using System;

namespace Sirius.Architecture.Data {

    /// <summary>
    /// Represents a Transact-SQL Unicode literal string. <see cref="SqlStatementBuilder"/> and <see cref="TransactSqlFormat"/>
    /// will render a <c>SqlUnicodeLiteral</c> object with a <c>N</c> prefix.
    /// </summary>
    /// <remarks>
    /// This class has immutable value semantics. It exists only to "mark" strings for special treatment in building SQL statements.
    /// </remarks>
    [Serializable]
    public class SqlUnicodeLiteral : ICloneable, IEquatable<SqlUnicodeLiteral> {

        #region Private Fields

        private string _value;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new object with the specified value.
        /// </summary>
        /// <param name="value">Value</param>
        /// <remarks>
        /// If a <see langword="null"/> value is specified, it will be regarded as a blank string.
        /// To represent a true <see langword="null"/> value, create a <see langword="null"/> instance
        /// of this class, or use the explicit conversion operator (which obeys this logic already).
        /// </remarks>
        public SqlUnicodeLiteral(string value) {
            _value = Cast.DefaultIfNull(value);
        }

        /// <summary>
        /// Create a new object with a blank value.
        /// </summary>
        public SqlUnicodeLiteral() {
            _value = string.Empty;
        }

        #endregion

        #region ICloneable Methods

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        object ICloneable.Clone() {

            return Clone();

        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public SqlUnicodeLiteral Clone() {

            SqlUnicodeLiteral copy = new SqlUnicodeLiteral();
            copy._value = this._value;
            return copy;

        }

        #endregion

        #region IEquatable Methods

        /// <summary>
        /// Determine whether the two operands are considered equal.
        /// </summary>
        /// <param name="value1">An object to compare.</param>
        /// <param name="value2">An object to compare.</param>
        /// <returns>True if the objects are both <see langword="null"/>, or if both are not <see langword="null"/> and <c>value1.Equals(value2)</c> returns true.</returns>
        public static bool operator ==(SqlUnicodeLiteral value1, SqlUnicodeLiteral value2) {

            if(object.ReferenceEquals(value1, null)) {
                return object.ReferenceEquals(value2, null);
            } else {
                return ((IEquatable<SqlUnicodeLiteral>) value1).Equals((SqlUnicodeLiteral) value2);
            }

        }

        /// <summary>
        /// Determine whether the two operands are considered unequal.
        /// </summary>
        /// <param name="value1">An object to compare.</param>
        /// <param name="value2">An object to compare.</param>
        /// <returns>The opposite of <c>value1 = value2</c>.</returns>
        public static bool operator !=(SqlUnicodeLiteral value1, SqlUnicodeLiteral value2) {

            return !(value1 == value2);

        }

        /// <summary>
        /// Determine whether the specified object is equal to this one.
        /// </summary>
        /// <param name="value">The object to compare to.</param>
        /// <returns>True if the object is the same instance as this one, or if it is the same type and has the same value.</returns>
        public override bool Equals(object value) {

            if(value is SqlUnicodeLiteral) {
                return ((IEquatable<SqlUnicodeLiteral>) this).Equals((SqlUnicodeLiteral) value);
            } else {
                return false;
            }

        }

        /// <summary>
        /// Determine whether the specified object is equal to this one.
        /// </summary>
        /// <param name="value">The object to compare to.</param>
        /// <returns>True if the object has the same value as this one.</returns>
        bool IEquatable<SqlUnicodeLiteral>.Equals(SqlUnicodeLiteral value) {

            if(object.ReferenceEquals(value, null)) {
                return false;
            } else if(object.ReferenceEquals(value, this)) {
                return true;
            } else {
                return ValueEquals(value);
            }

        }

        /// <summary>
        /// Determine whether the specified object is equal to this one.
        /// </summary>
        /// <param name="value">The object to compare to.</param>
        /// <returns>True if the object has the same value as this one.</returns>
        private bool ValueEquals(SqlUnicodeLiteral value) {

            return this._value == value._value;

        }

        /// <summary>
        /// Return the hash code for this object.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode() {

            return this._value.GetHashCode();

        }

        #endregion

        #region Object Methods

        /// <summary>
        /// Return the string value of this object.
        /// </summary>
        public override string ToString() {

            return this._value;

        }

        #endregion

        #region Conversion Operators

        public static explicit operator SqlUnicodeLiteral(string value) {

            if(value == null) {
                return null;
            } else {
                return new SqlUnicodeLiteral(value);
            }
        }

        public static explicit operator string(SqlUnicodeLiteral value) {

            if(value == null) {
                return null;
            } else {
                return value.ToString();
            }
        }

        #endregion

    }

}
