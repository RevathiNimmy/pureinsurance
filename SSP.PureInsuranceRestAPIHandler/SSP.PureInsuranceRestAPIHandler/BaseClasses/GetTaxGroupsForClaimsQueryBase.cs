
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetTaxGroupsForClaimsQueryBase : BaseRequestType
    {
        public bool Is_withholding_tax { get; set; }
        public string TransactionTypeCode { get; set; }
    }
}
