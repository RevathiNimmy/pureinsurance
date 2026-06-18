
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetDefaultBankAccountWithCurrencyQueryBaseResponse : BasePagedResponse
    {
        public List<BaseGetDefaultBankAccountWithCurrencyResponseTypeResultsRow> Results { get; set; }
    }
}
