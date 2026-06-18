using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimPaymentTaxGroupsQueryBaseResponse : BasePagedResponse
    {
        public List<BaseGetClaimPaymentTaxGroupsResponseTypeRow> TaxGroup { get; set; }
    }
}
