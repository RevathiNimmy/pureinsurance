using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindCaseCommandBaseResponse : BasePagedResponse
    {
        public List<BaseFindCaseResponseTypeCaseDetailsRow> CaseDetails { get; set; }
    }
}
