using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetListofUnapprovedPaymentQueryBaseResponse : BaseResponseType
    {
        public List<BaseGetListofUnapprovedPaymentResponseRowType> ListofUnapprovedPayment { get; set; }
    }
}
