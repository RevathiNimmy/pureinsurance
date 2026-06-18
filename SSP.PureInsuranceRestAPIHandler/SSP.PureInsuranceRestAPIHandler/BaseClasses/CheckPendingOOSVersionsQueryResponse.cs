using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CheckPendingOOSVersionsQueryResponse : BasePagedResponse
    {
        public bool HasPendingOOSVersions { get; set; }
        public List<BaseCheckPendingOosVersionsResponseTypePolicies> ResultData { get; set; }
    }
}
