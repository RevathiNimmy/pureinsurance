namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseFindCashListReceiptsResponseTypeRow
    {
        //[DBCol("cashlistitem_id")]
        public int CashListItemKey { get; set; }
        //[DBCol("Insurance_File_Cnt")]
        public int InsuranceFileKey { get; set; }
        //[DBCol("MediaType_Id")]
        public int MediaTypeKey { get; set; }
        //[DBCol("MediaType")]
        public string MediaTypeDescription { get; set; }
        //[DBCol("MediaType_Status_Id")]
        public int MediaTypeStatusKey { get; set; }
        //[DBCol("MediaTypeStatus")]
        public string MediaTypeStatusDescription { get; set; }
        //[DBCol("document_ref")]
        public string DocumentRef { get; set; }
        //[DBCol("Branch")]
        public string BranchDescription { get; set; }
        //[DBCol("ClientCode")]
        public string ClientCode { get; set; }
        //[DBCol("ClientName")]
        public string ClientName { get; set; }
        //[DBCol("PolicyNumber")]
        public string PolicyNumber { get; set; }
        //[DBCol("DrawnBankName")]
        public string DrawnBankName { get; set; }
        //[DBCol("MediaReference")]
        public string MediaReference { get; set; }
        //[DBCol("MediaTypeCode")]
        public string MediaTypeCode { get; set; }
        //[DBCol("MediaType_Status_Code")]
        public string MediaTypeStatusCode { get; set; }
        //[DBCol("CurrentStatus")]
        public string CurrentStatus { get; set; }
    }
}
