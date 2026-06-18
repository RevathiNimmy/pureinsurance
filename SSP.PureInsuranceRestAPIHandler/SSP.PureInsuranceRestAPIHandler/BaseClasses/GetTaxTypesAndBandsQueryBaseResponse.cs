using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetTaxTypesAndBandsQueryBaseResponse : BasePagedResponse
    {
        public List<BaseGetTaxTypesAndBandsResponseRow> Taxes { get; set; }
    }
}
