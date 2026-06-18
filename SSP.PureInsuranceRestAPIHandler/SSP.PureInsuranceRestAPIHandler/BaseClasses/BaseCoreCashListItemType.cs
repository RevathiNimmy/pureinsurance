using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCoreCashListItemType
    {
        public string BranchCode { get; set; }
        public string AccountShortCode { get; set; }
        public int AccountKey { get; set; }
        public string AllocationStatusCode { get; set; }
        public int AllocationStatusKey { get; set; }
        public decimal Amount { get; set; }
        public decimal Amount_Tendered { get; set; }
        public string BankReference { get; set; }
        public int TransDetailKey { get; set; }
        public int CashListItemKey { get; set; }
        public bool CashListItemKeySpecified { get; set; }
        public System.DateTime ChequeDate { get; set; }
        public System.DateTime Collection_Date { get; set; }
        public string ContactName { get; set; }
        public string FurtherDetails { get; set; }
        public bool IsProduceDocument { get; set; }
        public string MediaReference { get; set; }
        public string MediaTypeCode { get; set; }
        public int MediaTypeKey { get; set; }
        public decimal Original_Amount { get; set; }
        public string OurReference { get; set; }
        public bool SkipPosting { get; set; }
        public decimal TaxAmount { get; set; }
        public bool TaxAmountSpecified { get; set; }
        public string TaxBandCode { get; set; }
        public int TaxBandKey { get; set; }
        public bool TaxBandKeySpecified { get; set; }
        public string TheirReference { get; set; }
        public int PartyKey { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public BaseAllocationType AllocationDetails { get; set; }
        public BaseSimpleAddressType ContactAddress { get; set; }
        public BaseCoreCashListItemTypeInstalmentPlanDetails[] InstalmentPlanDetailsField { get; set; }

        public bool IsValidated { get; set; }
        public DateTime? CurrencyBaseDate { get; set; }
        public decimal? CurrencyBaseXrate { get; set; }
        public DateTime? AccountBaseDate { get; set; }
        public decimal? AccountBaseXrate { get; set; }
        public DateTime? SystemBaseDate { get; set; }
        public decimal? SystemBaseXrate { get; set; }
        public int OverrideReason { get; set; }
    }
}
