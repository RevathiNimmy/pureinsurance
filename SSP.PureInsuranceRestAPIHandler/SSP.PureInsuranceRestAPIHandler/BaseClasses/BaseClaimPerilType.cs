using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimPerilType
    {

        public string Description { get; set; }
        public System.Collections.Generic.List<BaseClaimPerilRecoveryType> Recovery { get; set; }
        public System.Collections.Generic.List<BaseClaimPerilReserveType> Reserve { get; set; }
        public string TypeCode { get; set; }


        public int ClaimPerilId { get; set; }

        public int SamStagingClaimPerilKey { get; set; }

        public string ClassOfBusinessCode { get; set; }

        public int ClassOfBusinessId { get; set; }

        public decimal TransactionAmount { get; set; }

    }
}
