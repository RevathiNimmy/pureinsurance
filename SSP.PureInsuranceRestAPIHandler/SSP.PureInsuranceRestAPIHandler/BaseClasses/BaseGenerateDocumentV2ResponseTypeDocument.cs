namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGenerateDocumentV2ResponseTypeDocument
    {
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }
        public string DocumentCode { get; set; }
        public string DocumentDescription { get; set; }
        public string MergedFilePath { get; set; }
        public byte[] SpooledZipFile { get; set; }
        public bool IsSplitDocument { get; set; }
        public string ParentDocumentCode { get; set; }
    }
}
