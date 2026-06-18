using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetMIDFileDetailsQueryResponse : BasePagedResponse
    {
        public bool FailuresOnly { get; set; }
        public int FileSequenceNumber { get; set; }
        public List<BaseGetMIDFileDetailsResponseTypeRow> Policies { get; set; }
    }
}
