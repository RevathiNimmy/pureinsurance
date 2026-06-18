using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseNBQuoteResponseType : BaseResponseType
    {
        public AddPartyCommandBaseResponse Insured { get; set; }
        public System.Collections.Generic.List<BaseNBQuoteResponseTypePolicy> Policy { get; set; }

    }
}
