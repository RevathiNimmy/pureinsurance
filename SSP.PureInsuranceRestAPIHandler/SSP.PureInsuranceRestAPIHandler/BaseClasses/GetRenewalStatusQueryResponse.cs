using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRenewalStatusQueryResponse : BaseResponseType
    {
         public BaseGetRenewalStatusResponseType RenewalStatus { get; set; }
    }
}