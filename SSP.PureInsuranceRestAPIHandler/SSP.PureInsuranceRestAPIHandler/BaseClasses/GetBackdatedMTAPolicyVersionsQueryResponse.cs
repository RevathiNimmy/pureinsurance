using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetBackdatedMTAPolicyVersionsQueryResponse : BasePagedResponse
    {
        public List<BaseAddBackDatedMtaQuoteResponseTypeRow> BackdatedTransactions { get; set; }
        public string FailureReason { get; set; }
    }
}
