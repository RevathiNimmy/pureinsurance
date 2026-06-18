using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class ClaimReceiptCommandBase : BaseRequestType
    {
        public BaseClaimReceiptType ClaimReceipt { get; set; }
        public List<BaseClaimReceiptType> ClaimReceiptCollection { get; set; }
        public bool CloseClaimOnZeroReserveRecoveryBalance { get; set; }
        public int GetSavedTaxOfPeril { get; set; }
        public bool PostTransaction { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
        
        public int SourceId { get; set; }
        
        public int ReceiptPartyAccountId { get; set; }
        
        public string ReceiptPartyAccountCode { get; set; }
        
        public string ReceiptPartyShortCode { get; set; }
        
        public bool IsDataTransferClaim { get; set; }
        
        public bool DataTransferIsUsingFullClaimVersioning { get; set; }
        
        public bool DataTransferClaimHasClaimRiskDataSpecified { get; set; }
        
        public bool DataTransferClaimHasSpecifiedReinsurance { get; set; }
    }
}
