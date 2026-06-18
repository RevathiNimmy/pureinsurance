using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetEventNoteQueryResponse : BasePagedResponse
    {
        public List<BaseGetEventNoteResponseTypeRow> EventNotes { get; set; }
    }
}
