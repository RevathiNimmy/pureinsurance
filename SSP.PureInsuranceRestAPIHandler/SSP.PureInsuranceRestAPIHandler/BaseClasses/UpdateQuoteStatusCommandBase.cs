using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateQuoteStatusCommandBase : BaseRequestType
    {

        public int InsuranceFileKey { get; set; }
        public QuoteStatusType? QuoteStatusKey { get; set; }
    }
}
