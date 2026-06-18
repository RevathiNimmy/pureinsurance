using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GenerateClaimsDocumentsCommandResponse
    {
        public List<BaseGenerateClaimsDocumentsResponseTypeRow> Documents { get; set; }
    }
}
