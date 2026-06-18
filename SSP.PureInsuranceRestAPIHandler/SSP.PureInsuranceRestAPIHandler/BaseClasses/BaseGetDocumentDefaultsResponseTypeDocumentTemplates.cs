namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetDocumentDefaultsResponseTypeDocumentTemplates
    {
        public string DocumentGroupCode { get; set; }
        public string DocumentGroupDescription { get; set; }
        public int DocumentGroupID { get; set; }
        public string DocumentSubGroupCode { get; set; }
        public string DocumentSubGroupDescription { get; set; }
        public int DocumentSubGroupID { get; set; }
        public string DocumentTemplateCode { get; set; }
        public string DocumentTemplateDescription { get; set; }
        public int DocumentTemplateID { get; set; }
        public bool InternalOnly { get; set; }
        public bool SelectedByDefault { get; set; }
        public string EmailDocumentSubjectCode { get; set; }
        public string EmailDocumentAttachmentCode { get; set; }
    }
}
