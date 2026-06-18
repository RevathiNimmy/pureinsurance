namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseFindDocumentTemplatesResponseTypeRow : BaseResponseType
    {
        //[DBCol("Code")]
        public string Code { get; set; }
        //[DBCol("Description")]
        public string Description { get; set; }
        //[DBCol("DocumentTemplateKey")]
        public int DocumentTemplateKey { get; set; }
        //[DBCol("EffectiveDate")]
        public System.DateTime EffectiveDate { get; set; }
        //[DBCol("Type")]
        public string Type { get; set; }
    }
}
