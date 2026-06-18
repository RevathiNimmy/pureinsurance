namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseUpdateReceiptMediaTypeStatusRequestTypeRow
    {

        ////(1, int.MaxValue, ErrorMessage = "The CashListItemKey field is required")]
        //
        public int CashListItemKey { get; set; }
        public string Comments { get; set; }
        public string DocumentRef { get; set; }
        public int InsuranceFileKey { get; set; }

        public string MediaTypeCode { get; set; }

        public string MediaTypeStatusCode { get; set; }

        public System.DateTime ModifiedDate { get; set; }
        public bool SkipArchiveOnEdit { get; set; }

        public int MediaTypeId { get; set; }

        public int MediaTypeStatusId { get; set; }

        public string InsuranceRef { get; set; }
    }
}
