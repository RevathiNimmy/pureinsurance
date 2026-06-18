namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetProductDocumentsResponseTypeProductDocumentsRow
    {
        public string DTDescription { get; set; }
        //[DBCol("Document_Template_Code")]
        public string DocumentTemplateCode { get; set; }
        //[DBCol("Document_Type_Id")]
        public int DocumentTypeKey { get; set; }
        //[DBCol("functional_area")]
        public int FunctionalArea { get; set; }
        //[DBCol("GIS_Scheme_Id")]
        public int GISSchemeKey { get; set; }
        //[DBCol("generate_through_BO")]
        public bool GenerateThroughBO { get; set; }
        //[DBCol("generate_through_SAM")]
        public bool GenerateThroughSAM { get; set; }
        //[DBCol("is_agent")]
        public bool IsAgent { get; set; }
        //[DBCol("is_client")]
        public bool IsClient { get; set; }
        //[DBCol("is_office")]
        public bool IsOffice { get; set; }
        //[DBCol("PMB_Doc_Link_Id")]
        public int PMBDocLinkKey { get; set; }
        public string PTDDescription { get; set; }
        public string PTDescription { get; set; }
        //[DBCol("Process_Type_Id")]
        public int ProcessTypeKey { get; set; }
        //[DBCol("process_types_docs_id")]
        public int ProcessTypesDocsKey { get; set; }
        //[DBCol("product_id")]
        public int ProductKey { get; set; }
        //[DBCol("production_order")]
        public int ProductionOrder { get; set; }
        public string SDescription { get; set; }
        //[DBCol("source_id")]
        public int SourceKey { get; set; }
        //[DBCol("spool_document")]
        public int SpoolDocument { get; set; }
    }
}
