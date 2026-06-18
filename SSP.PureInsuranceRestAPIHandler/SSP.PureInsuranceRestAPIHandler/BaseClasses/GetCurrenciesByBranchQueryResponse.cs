using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetCurrenciesByBranchQueryResponse : BasePagedResponse
    {
        public string BaseCurrencyCode { get; set; }
        public string BaseCurrencyDescription { get; set; }
        public List<BaseGetCurrenciesByBranchResponseTypeRow> Currencies { get; set; }
    }
}
