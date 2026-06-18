using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetListQueryResponse : BasePagedResponse
    {
        public string AdditionalResult { get; set; }
        public List<BaseGetListResponseType> GetListResponse { get; set; }
    }
}
