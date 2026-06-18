using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetDefaultBankAccountWithCurrencyRequestType : BaseRequestType
    {
        public int  CashListTypeID { get; set; }

        public int MediaTypeID { get; set; }

        public int ProductCode { get; set; }
    }
}
