using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetMIDFilesQueryResponse : BasePagedResponse
    {
        public List<BaseGetMIDFilesResponseTypeRow> MIDFiles { get; set; }
    }
}
