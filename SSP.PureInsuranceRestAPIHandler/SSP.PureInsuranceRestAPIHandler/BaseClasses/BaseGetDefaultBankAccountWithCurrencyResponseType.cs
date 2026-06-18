using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetDefaultBankAccountWithCurrencyResponseType :BaseResponseType
    {
        public List<BaseGetDefaultBankAccountWithCurrencyResponseTypeResultsRow> Results { get; set; }

    }
}
