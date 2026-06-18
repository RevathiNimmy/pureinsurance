using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRIPropTreatiesQueryResponse : BaseResponseType
    {
        public List<BaseGetRIPropTreatiesResponseTypeTreatiesRow> Treaties { get; set; }
    }
}
