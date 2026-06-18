using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sirius.Architecture.Data {

    /// <summary>
    /// A collection of column names and values for use when building a dynamic SQL statement.
    /// </summary>
    public class SqlColumnCollection : Dictionary<string, object>, ICloneable {

        #region Public Methods

        /// <summary>
        /// Returns true if the collection contains any genuine data to be written to the database.
        /// Genuine data is defined as any values that are not <see cref="Missing.Value"/>.
        /// </summary>
        public bool AnyNotMissing() {
            return this.Any(item => item.Value != Missing.Value);
        }

        /// <summary>
        /// Returns true if the collection contains any populated data to be written to the database.
        /// Genuine data is defined as any values that are not <see cref="Missing.Value"/> or <see langword="null"/>.
        /// </summary>
        public bool AnyNotNull() {
            return this.Any(item => item.Value != Missing.Value && item.Value != DBNull.Value && item.Value != null);
        }

        /// <summary>
        /// Set all values in the collection to <see cref="Missing.Value"/>. This is useful for
        /// resetting the data at the start of a new row.
        /// </summary>
        public void ClearAllValues() {
            foreach(string key in this.Keys.ToList()) {
                this[key] = Missing.Value;
            }
        }

        /// <summary>
        /// Set all <see langword="null"/> values in the collection to <see cref="Missing.Value"/>. This is useful for
        /// cutting down the amount of data going into an <c>INSERT</c> statement.
        /// </summary>
        public void ClearNullValues() {
            foreach(string key in this.Keys.ToList()) {
                object value = this[key];
                if(value == null || value == DBNull.Value) {
                    this[key] = Missing.Value;
                }
            }
        }

        /// <summary>
        /// Set all values in the collection to <see langword="null"/>. This is useful for
        /// resetting the data at the start of a new row.
        /// </summary>
        public void SetAllValuesToNull() {
            foreach(string key in this.Keys.ToList()) {
                this[key] = null;
            }
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
        public SqlColumnCollection Clone() {
            SqlColumnCollection copy = new SqlColumnCollection();
            foreach(var item in this) {
                // Attempt a deep copy of each value by cloning it if it is cloneable.
                object value = item.Value;
                object valueCopy = null;
                if(value is ICloneable) {
                    valueCopy = ((ICloneable) value).Clone();
                } else {
                    valueCopy = value;
                }
                copy.Add(item.Key, valueCopy);
            }
            return copy;
        }

        #endregion
    }
}
