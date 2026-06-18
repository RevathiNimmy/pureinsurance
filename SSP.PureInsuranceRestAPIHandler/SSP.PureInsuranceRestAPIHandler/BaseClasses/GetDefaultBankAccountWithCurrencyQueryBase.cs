
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetDefaultBankAccountWithCurrencyQueryBase : BaseRequestType
    {
        public int CashListTypeID { get; set; }
        public int MediaTypeID { get; set; }
        public string ProductCode { get; set; } = string.Empty;
    }
}
