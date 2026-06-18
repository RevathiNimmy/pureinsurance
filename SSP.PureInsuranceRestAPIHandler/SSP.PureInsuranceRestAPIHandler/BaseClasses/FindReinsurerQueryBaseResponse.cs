
using SSP.PureInsuranceRestAPIHandler.BaseClasses;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindReinsurerQueryBaseResponse : BasePagedResponse
    {
        public List<BaseFindReinsurerResponseTypeReinsurersRow> Reinsurers { get; set; }
    }
}
