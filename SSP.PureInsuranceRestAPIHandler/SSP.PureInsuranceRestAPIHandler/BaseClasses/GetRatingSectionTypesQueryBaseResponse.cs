using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRatingSectionTypesQueryBaseResponse : BaseResponseType
    {
        public List<BaseGetRatingSectionTypesResponseTypeRow> RatingSectionTypes { get; set; }
    }
}
