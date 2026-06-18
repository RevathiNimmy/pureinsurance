using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddBackDatedMTAQuoteCommandResponse
    {
        public List<BaseAddBackDatedMtaQuoteResponseTypeRow> BackdatedTransactions { get; set; }
        public string FailureReason { get; set; }
    }
}
