using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCdtClaimReinsuranceTypeClaimRIArrangement
    {
        public int ClaimAllocationType { get; set; }
        public int ClaimKey { get; set; }
        public int RiskKey { get; set; }
        public System.Collections.Generic.List<BaseCdtClaimRIArrangmentLineType> ClaimRIArrangmentLine { get; set; }
        public decimal Payment { get; set; }
        public int RIArrangementKey { get; set; }
        public string RIBandCode { get; set; }
        public string RIModelCode { get; set; }

        public int RIBandId { get; set; }

        public int RIModelId { get; set; }

        public int ClaimRIArrangementId { get; set; }
        public decimal Recovery { get; set; }
        public decimal Reserve { get; set; }
        public int SAMStagingClaimRIArrangementKey { get; set; }
        public decimal Salvage { get; set; }
        public decimal SumInsured { get; set; }
        public decimal ThisPayment { get; set; }
        public decimal ThisRecovery { get; set; }
        public decimal ThisReserve { get; set; }
        public decimal ThisSalvage { get; set; }
    }
}
