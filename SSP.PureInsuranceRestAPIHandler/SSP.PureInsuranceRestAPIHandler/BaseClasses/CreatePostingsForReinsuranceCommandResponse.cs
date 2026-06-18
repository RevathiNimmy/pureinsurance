using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CreatePostingsForReinsuranceCommandResponse
    {
        public IEnumerable<CreatePostingsForReinsuranceCommandBaseResponse> CreatePostingsForReinsuranceResponse { get; set; } = System.Linq.Enumerable.Empty<CreatePostingsForReinsuranceCommandBaseResponse>();
    }
}
