using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateClaimReservesOrPaymentsCommandBaseResponse : BaseResponseType
    {
        public string ResultingStatus { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
        public List<BaseUpdateClaimReservesOrPaymentsResponseTypeWarnings> Warnings { get; set; }
    }
}
