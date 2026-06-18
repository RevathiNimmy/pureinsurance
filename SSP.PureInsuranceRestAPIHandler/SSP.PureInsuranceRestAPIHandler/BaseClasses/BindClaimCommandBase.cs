using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BindClaimCommandBase : BaseRequestType
    {
        public int BaseCaseKey { get; set; }
        public int ClaimKey { get; set; }
        public int InsuranceFileCNT { get; set; }
        public BaseClaimPaymentType ClaimPayment { get; set; }
        public List<BaseClaimPaymentType> ClaimPerilPayment { get; set; }
        public bool CloseClaimOnZeroReserveRecoveryBalance { get; set; }
        public bool CloseClaimOnFinalPayment { get; set; }
        public bool ExternalHandler { get; set; }
        public bool IgnoreWarnings { get; set; }
        public bool NoTrans { get; set; }
        public int ProcessType { get; set; }
        public bool SkipSaveTransaction { get; set; }
        public byte[] ApiTimeStamp { get; set; }
        public int SourceId { get; set; }
        public string ResultingStatus { get; set; }
        public bool TPASettleDirectly { get; set; } = false;
        public bool IsTPASpecified { get; set; } = false;
        public BaseClaimReceiptType ClaimReceipt { get; set; }
        public string? CoinsuranceTreatmentCode { get; set; }

    }
}
