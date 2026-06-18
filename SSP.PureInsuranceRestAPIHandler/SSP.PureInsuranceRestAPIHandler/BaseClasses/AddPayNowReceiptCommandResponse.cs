using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{

    public class AddPayNowReceiptCommandResponse : BaseResponseType
    {
        public int CashTransactionKey { get; set; }
    }
}
