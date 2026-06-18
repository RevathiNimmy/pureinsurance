namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetStandardPolicyWordingsResponseTypeRow
    {
        //[DBCol("code")]
        public string Code { get; set; }
        //[DBCol("description")]
        public string Description { get; set; }
        //[DBCol("document_template_id")]
        public int DocumentTemplateId { get; set; }
        //[DBCol("OriginalDocumentCode")]
        public string OriginalCode { get; set; }
    }
}
