using System.Collections.Generic;
using System.Linq;
namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class IsPendingTransferQueryResponse : BaseIsPendingTransferResponseType
    {
        public IEnumerable<IsPendingTransferQueryBaseResponse> IsPendingTransferResponse { get; set; } = Enumerable.Empty<IsPendingTransferQueryBaseResponse>();
    }
}
