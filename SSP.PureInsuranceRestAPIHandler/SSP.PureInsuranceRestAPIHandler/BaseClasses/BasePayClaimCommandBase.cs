using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePayClaimCommandBase : BaseRequestType
    {
        public BaseClaimPaymentType ClaimPayment { get; set; }
        public System.Collections.Generic.List<BaseClaimPaymentType> ClaimPerilPayment { get; set; }
        public int GetSavedTaxOfPeril { get; set; }

        public bool IsDataTransferClaim { get; set; }

        public bool DataTransferIsUsingFullClaimVersioning { get; set; }

        public bool DataTransferClaimHasClaimRiskDataSpecified { get; set; }

        public int PaymentPartyAccountId { get; set; }

        public string PaymentPartyAccountCode { get; set; }

        public string PaymentPartyShortCode { get; set; }

        public bool DataTransferClaimHasSpecifiedReinsurance { get; set; }

        public int SourceId { get; set; }

    }
}
