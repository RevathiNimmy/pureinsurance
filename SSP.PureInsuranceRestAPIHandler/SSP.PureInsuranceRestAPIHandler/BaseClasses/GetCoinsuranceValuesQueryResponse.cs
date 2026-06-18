using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetCoinsuranceValuesQueryResponse : BasePagedResponse
    {
        public List<BaseGetCoinsuranceValuesResponseTypeRow> CoInsurers { get; set; }
        public int DefaultId { get; set; }
        public bool IsRecovered { get; set; }
        public bool IsSurcharged { get; set; }
    }
}
