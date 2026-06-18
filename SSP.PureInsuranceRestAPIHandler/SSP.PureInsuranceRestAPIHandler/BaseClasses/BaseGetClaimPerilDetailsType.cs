using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{

    public class BaseGetClaimPerilDetailsType
    {
        public int ClaimPerilKey { get; set; }
        public string TypeCode { get; set; }
        public int BaseClaimPerilKey { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public decimal SumInsured { get; set; }
        public string RIBand { get; set; }
        public string GisScreenCode { get; set; }
        public System.Collections.Generic.List<BaseGetClaimReserveDetailsType> Reserve { get; set; }
        public System.Collections.Generic.List<BaseGetClaimRecoveryDetailsType> Recovery { get; set; }
        public System.Collections.Generic.List<BaseGetClaimPaymentDetailsType> ClaimPayments { get; set; }
        public System.Collections.Generic.List<BaseGetClaimReceiptDetailsType> ClaimReceipts { get; set; }
        public BasePagedResponse PagedReserve { get; set; }
        public BasePagedResponse PagedRecovery { get; set; }
        public BasePagedResponse PagedClaimPayments { get; set; }
        public BasePagedResponse PagedClaimReceipts { get; set; }
    }

}