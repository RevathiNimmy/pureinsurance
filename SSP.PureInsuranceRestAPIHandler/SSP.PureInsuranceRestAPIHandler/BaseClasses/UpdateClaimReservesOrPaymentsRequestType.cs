using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateClaimReservesOrPaymentsRequestType : BaseRequestType
    {
        public int ClaimKey { get; set; }

        public BaseClaimPaymentType ClaimPayment { get; set; }

        public List<BaseClaimPerilType> ClaimPeril { get; set; }

        public int ProcessType { get; set; }

        public byte[] TimeStamp { get; set; }

    }
}
