using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUnallocatedClaimPaymentsResponseType : BaseGetUnallocatedClaimPaymentsResponseType
    {
        public List<BaseGetUnallocatedClaimPaymentsResponseTypeRow> unallocatedClaimPayments { get; set; }
    }
}
