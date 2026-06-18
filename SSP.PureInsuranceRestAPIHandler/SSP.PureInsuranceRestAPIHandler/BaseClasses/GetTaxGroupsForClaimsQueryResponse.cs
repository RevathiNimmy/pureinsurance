
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetTaxGroupsForClaimsQueryResponse : BasePagedResponse
    {
        public List<BaseGetTaxGroupsForClaimsResponseTypeRow> TaxGroups { get; set; }
    }
}
