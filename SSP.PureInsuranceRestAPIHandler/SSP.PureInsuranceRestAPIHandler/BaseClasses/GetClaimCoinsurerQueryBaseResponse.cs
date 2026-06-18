using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimCoinsurerQueryBaseResponse : BasePagedResponse
    {
        public string ClaimNumber { get; set; }
        public List<BaseGetClaimCoinsurerResponseTypeRow> Coinsurers { get; set; }
        public decimal TotalCurrentShareValue { get; set; }
        public decimal TotalShare { get; set; }
    }
}
