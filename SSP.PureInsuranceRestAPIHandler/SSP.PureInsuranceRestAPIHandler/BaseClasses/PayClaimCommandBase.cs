
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class PayClaimCommandBase :BaseRequestType
    {
        public BaseClaimPaymentType ClaimPayment { get; set; }
        public List<BaseClaimPaymentType> ClaimPerilPayment { get; set; }
        public int GetSavedTaxOfPeril { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];

        
        public bool IsDataTransferClaim { get; set; }
        
        public bool DataTransferIsUsingFullClaimVersioning { get; set; }
        
        public bool DataTransferClaimHasClaimRiskDataSpecified { get; set; }
        
        public int PaymentPartyAccountId { get; set; }
        
        public string PaymentPartyAccountCode { get; set; }
        
        public string PaymentPartyShortCode { get; set; }
        
        public int SourceId { get; set; }
        
        public string CallingBy { get; set; } = string.Empty;
        
        public bool DataTransferClaimHasSpecifiedReinsurance { get; set; }
    }
}
