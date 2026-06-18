using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class DeleteRenewalCommandResponse :DeleteRenewalCommandBaseResponse
    {
        public byte[] QuoteTimeStamp { get; set; } = new byte[0];
    }
}
