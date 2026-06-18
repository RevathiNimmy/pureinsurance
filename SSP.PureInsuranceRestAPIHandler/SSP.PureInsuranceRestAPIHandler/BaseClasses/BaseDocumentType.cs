using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseDocumentType
    {
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string DocDescription { get; set; }
        public int DocNum { get; set; }
        public DMEDocType? DocumentType { get; set; }
        public int FolderNum { get; set; }
        public string FolderPath { get; set; }
        public string UploadedBy { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
    }
}
