using System.Collections.Generic;
using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetInstalmentQuotesQueryBaseResponse : BasePagedResponse
    {
        public int PartyBankId { get; set; }
        public System.DateTime PreferredInstalmentDueDate { get; set; }

        public List<BaseGetInstalmentQuotesResponseTypeRow> Quotes { get; set; }
    }
}
