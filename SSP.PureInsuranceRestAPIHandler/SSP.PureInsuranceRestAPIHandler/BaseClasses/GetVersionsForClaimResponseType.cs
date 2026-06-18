using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetVersionsForClaimResponseType : BaseResponseType
    {
        public bool isPreviouslyLocked { get; set; }

        public string newProperty { get; set; }

        public List<BaseGetVersionsForClaimResponseTypeRow> versions { get; set; }
    }
}
