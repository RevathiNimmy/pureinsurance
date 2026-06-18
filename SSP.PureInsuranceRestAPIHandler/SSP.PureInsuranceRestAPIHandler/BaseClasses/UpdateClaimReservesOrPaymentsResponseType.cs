using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateClaimReservesOrPaymentsResponseType : BaseResponseType
    {
        public string ResultingStatus { get; set; }

        public byte[] TimeStamp { get; set; }

        public List<BaseUpdateClaimReservesOrPaymentsResponseTypeWarnings> Warnings { get; set; }

    }
}
