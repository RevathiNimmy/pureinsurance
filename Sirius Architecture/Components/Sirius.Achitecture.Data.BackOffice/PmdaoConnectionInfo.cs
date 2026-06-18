using System;

namespace Sirius.Architecture.Data.BackOffice {

    /// <summary>
    /// The strongly-typed equivalent of a "connection string" for a PMDAO database.
    /// </summary>
    /// <remarks>
    /// This class is fully mutable because its fields must be passed by reference to a COM interface.
    /// As a result, you must explicitly clone it whenever you clone its parent object.
    /// </remarks>
    [Serializable]
    internal class PmdaoConnectionInfo : ICloneable {

        #region Public Fields

        public string SiriusUserName;
        public short SourceID;
        public short LanguageID;
        public string CallingAppName;
        public object UserName;
        public object Password;
        public object DataSourceName;
        public object DatabaseName;

        #endregion

        #region Object Members

        public override string ToString() {
            return string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}",
                SiriusUserName,
                SourceID,
                LanguageID,
                CallingAppName,
                UserName,
                Password,
                DataSourceName,
                DatabaseName);
        }

        #endregion

        #region ICloneable Members

        public PmdaoConnectionInfo Clone() {
            return new PmdaoConnectionInfo {
                SiriusUserName = this.SiriusUserName,
                SourceID = this.SourceID,
                LanguageID = this.LanguageID,
                CallingAppName = this.CallingAppName,
                UserName = this.UserName,
                Password = this.Password,
                DataSourceName = this.DataSourceName,
                DatabaseName = this.DatabaseName
            };
        }

        object ICloneable.Clone() {
            return Clone();
        }

        #endregion
    }
}
