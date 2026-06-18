using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimPerilSummaryResponseTypeReserveType
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public System.Collections.Generic.List<BaseGetClaimPerilSummaryResponseTypeReserveTypeRow> Perils { get; set; }
    }
}
