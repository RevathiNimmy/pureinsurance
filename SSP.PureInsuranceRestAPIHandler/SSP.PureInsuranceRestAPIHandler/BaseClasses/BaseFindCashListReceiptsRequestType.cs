namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseFindCashListReceiptsRequestType : BaseRequestType
    {
        public int PartyKey { get; set; }

        public bool PartyKeyFieldSpecified { get; set; }

        public string BankAccountCode { get; set; }

        public string InsuranceRef { get; set; }

        public System.DateTime CollectionDateFrom { get; set; }

        public bool CollectionDateFromFieldSpecified { get; set; }

        public System.DateTime CollectionDateTo { get; set; }

        public bool CollectionDateToFieldSpecified { get; set; }

        public string MediaReference { get; set; }

        public string MediaTypeStatusCode { get; set; }

        public string DrawnBankCode { get; set; }

        public string DocumentRef { get; set; }

        public int MaxRowsToFetch { get; set; }

        public bool MaxRowsToFetchFieldSpecified { get; set; }


        public int AgentKey { get; set; }
    }
}
