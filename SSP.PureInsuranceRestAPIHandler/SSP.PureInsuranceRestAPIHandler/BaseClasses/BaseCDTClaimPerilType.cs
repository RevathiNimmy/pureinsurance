using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCdtClaimPerilType
    {
        public int BaseClaimPerilKey { get; set; }
        public System.Collections.Generic.List<BaseCDTClaimPaymentType> ClaimPayment { get; set; }
        public System.Collections.Generic.List<BaseCdtClaimReceiptType> ClaimReceipt { get; set; }
        public string Description { get; set; }
        public System.Collections.Generic.List<BaseCdtRecoveryType> Recovery { get; set; }
        public System.Collections.Generic.List<BaseCdtReserveType> Reserve { get; set; }
        public int SAMStagingClaimPerilKey { get; set; }
        public string TypeCode { get; set; }
        public int SAMStagingBaseClaimPerilKey { get; set; }
        public int SiriusBaseClaimPerilKey { get; set; }
        public int SiriusClaimPerilKey { get; set; }
        public bool BaseClaimPerilKeySpecified { get; set; }

    }
}
