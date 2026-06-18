using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateQuoteStatusCommandResponse : BaseResponseType
    {
        public int InsuranceFileKey { get; set; }
        public QuoteStatusType? QuoteStatusKey { get; set; }
    }
}