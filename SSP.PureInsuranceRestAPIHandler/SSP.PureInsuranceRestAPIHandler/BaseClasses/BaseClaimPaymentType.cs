using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimPaymentType
    {
        public BaseClaimPaymentAdvancedTaxDetailsType AdvancedTaxDetails { get; set; }
        public int BaseCaseKey { get; set; }
        public int BaseClaimKey { get; set; }
        public int BaseClaimPerilKey { get; set; }
        public BasePaymentCashListType CashList { get; set; }
        public System.Collections.Generic.List<BaseClaimPaymentItemType> ClaimPaymentItem { get; set; }
        public System.Collections.Generic.List<BaseClaimPaymentTaxItemType> ClaimPaymentTaxItems { get; set; }
        public string ClaimVersionDescription { get; set; }
        public int ClientKey { get; set; }
        public bool CloseClaimOnZeroReserveRecoveryBalance { get; set; }
        public bool CloseClaimOnFinalPayment { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsExGratia { get; set; }
        public bool IsThisPayment { get; set; }
        public string OurRef { get; set; }
        public int PartyKey { get; set; }
        public BaseClaimPayeeType Payee { get; set; }
        public bool PaymentOnly { get; set; }
        public ClaimPaymentPartyTypeType PaymentPartyType { get; set; }
        public BaseClaimReceiptPayeeType ReceiptPayee { get; set; }
        public int ReserveKey { get; set; }
        public bool SkipTaxItemExpansion { get; set; }
        public string UltimatePayee { get; set; }
        public bool ViewMode { get; set; }

        public int VersionId { get; set; }

        public int InsuranceFileCNT { get; set; }

        public string AdvanceScript { get; set; }

        public int SourceId { get; set; }

        public int CurrencyId { get; set; }

        public int ClaimId { get; set; }

        public int ClaimPerilId { get; set; }

        public bool TaxIsWithHoldingTax { get; set; }

        public bool ClaimPaymentWorkflowEnabled { get; set; }

        public string ClaimPaymentIsGross { get; set; }

        public string AllowNegativeReserve { get; set; }

        public string ClaimNumber { get; set; }

        public decimal PaymentToLossXRate { get; set; }

        public System.DateTime TransactionDate { get; set; }

        public int LossCurrencyId { get; set; }

        public int PaymentId { get; set; }

        public int ClassOfBusinessID { get; set; }

        public string ClassOfBusinessCode { get; set; }

        public bool PostClaimTaxesSeperately { get; set; }

        public decimal TotCurrAmount { get; set; }

        public decimal TotTaxAmount { get; set; }

        public decimal TotTaxAmountWHT { get; set; }

        public decimal TotPaymentAmountNet { get; set; }

        public decimal TotTaxWHTAmount { get; set; }

        public decimal TotExcessAmount { get; set; }

        public int BaseCurrencyId { get; set; }

        public decimal BaseAmount { get; set; }

        public int AccountCurrencyId { get; set; }

        public decimal AccountAmount { get; set; }

        public decimal SystemAmount { get; set; }

        public int SystemCurrencyId { get; set; }

        public decimal CurrencyToBaseXRate { get; set; }

        public System.DateTime CurrencyToBaseDate { get; set; }

        public decimal AccountToBaseXRate { get; set; }

        public System.DateTime AccountToBaseDate { get; set; }

        public decimal SystemToBaseXRate { get; set; }

        public System.DateTime SystemToBaseDate { get; set; }

        public string Comments { get; set; }

        public bool IsReferred { get; set; }

        public int DocumentId { get; set; }

        public int TreatyId { get; set; }

        public int SequenceNo { get; set; }

        public bool PaymentToReinsurer { get; set; }

        public int ExgRateReasonId { get; set; }

        public int ClaimPaymentToId { get; set; }

        public System.DateTime DateOfPayment { get; set; }

    }
}
