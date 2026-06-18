using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class PayClaimRequestType : BaseRequestType
    {
        public BaseClaimPaymentType claimPayment { get; set; }

        public List<BaseClaimPaymentType> claimPerilPayment { get; set; }

        public int getSavedTaxOfPeril { get; set; }

        public byte[] timeStamp { get; set; }

    }
}
