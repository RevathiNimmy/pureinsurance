using Sirius.Architecture.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Sirius.Architecture.Data {

    /// <summary>
    /// A collection of database errors.
    /// </summary>
    [Serializable]
    public class DatabaseErrorCollection : ReadOnlyCollection<DatabaseError> {

        #region Constructors

        /// <summary>
        /// Construct a new instance of this class with all required information.
        /// This constructor supports <see cref="SiriusConnection"/>, and is not intended to be called directly in your code.
        /// </summary>
        /// <param name="list">A list of database errors.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public DatabaseErrorCollection(IList<DatabaseError> list)
            : base(list) {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a list of error object information in human-readable format.
        /// </summary>
        public override string ToString() {

            return VBCodeFormat.ToString<DatabaseError>(this, null, false);

        }

        #endregion

    }

}
