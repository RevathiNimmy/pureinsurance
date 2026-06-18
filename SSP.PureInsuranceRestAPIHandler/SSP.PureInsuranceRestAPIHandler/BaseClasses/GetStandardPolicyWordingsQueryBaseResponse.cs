using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetStandardPolicyWordingsQueryBaseResponse : BasePagedResponse
    {
        public List<BaseGetStandardPolicyWordingsResponseTypeRow> DocumentTemplates { get; set; }
        public bool IsInsuranceFile { get; set; }
    }
}
