using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindInsuranceFileForClaimsCommandResponse : BasePagedResponse
    {
        public int InsuranceFileKey { get; set; }
        public List<BaseFindInsuranceFileResponseTypeRow> InsuranceFileDetails { get; set; }
    }
}
