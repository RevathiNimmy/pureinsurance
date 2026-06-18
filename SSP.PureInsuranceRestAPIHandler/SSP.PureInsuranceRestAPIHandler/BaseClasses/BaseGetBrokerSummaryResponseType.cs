using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetBrokerSummaryResponseType : BaseResponseType
    {
        public System.Collections.Generic.List<BaseGetBrokerSummaryResponseTypeRow> InsuranceFileDetails { get; set; }
    }
}
