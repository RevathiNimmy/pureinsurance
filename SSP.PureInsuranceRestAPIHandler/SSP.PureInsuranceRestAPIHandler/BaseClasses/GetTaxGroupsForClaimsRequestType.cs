using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetTaxGroupsForClaimsRequestType : BaseRequestType
    {
        public bool Is_withholding_tax { get; set; }

        public string transactionTypeCode { get; set; }

    }
}
