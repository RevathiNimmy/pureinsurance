
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUnallocatedClaimPaymentsQueryResponse : BasePagedResponse
    {
        public List<BaseGetUnallocatedClaimPaymentsResponseTypeRow> UnallocatedClaimPayments { get; set; }
    }
}
