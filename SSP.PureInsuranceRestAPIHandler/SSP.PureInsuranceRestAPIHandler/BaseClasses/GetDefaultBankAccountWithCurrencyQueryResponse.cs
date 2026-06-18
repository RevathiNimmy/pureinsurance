
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetDefaultBankAccountWithCurrencyQueryResponse : BasePagedResponse
    {
        public GetDefaultBankAccountWithCurrencyQueryBaseResponse GetDefaultBankAccountWithCurrencyResponse { get; set; }
    }
}
