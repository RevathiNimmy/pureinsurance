using System.Data;

namespace Sirius.Architecture.Data {
    
    /// <summary>
    /// Allows a business object to construct itself (i.e. de-serialise) from a data reader.
    /// </summary>
    public interface IDataReadable {

        /// <summary>
        /// Populate object state from the current row of a data source.
        /// </summary>
        /// <param name="row">Data row to populate the object from</param>
        void SetObjectData(IDataRecord row);
    }
}
