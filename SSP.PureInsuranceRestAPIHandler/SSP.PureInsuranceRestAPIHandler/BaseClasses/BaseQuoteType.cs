namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseQuoteType : BaseRequestType
    {
        public int AgentKey { get; set; }
        public bool AgentKeySpecified { get; set; }
        public string AlternativeRef { get; set; }
        public string AnalysisCode { get; set; }

        public System.DateTime CoverEndDate { get; set; }

        public System.DateTime CoverStartDate { get; set; }

        public string CurrencyCode { get; set; }
        public string Description { get; set; }

        public string InsuredName { get; set; }

        public string ProductCode { get; set; }
        public string QuoteRef { get; set; }
        public string UnderwritingYearCode { get; set; }
    }
}
