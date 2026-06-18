namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimReceiptTaxesQuery : ClaimReceiptCommandBase
    {
        public int TaxItemsPageNumber { get; set; }
        public int TaxItemsPageSize { get; set; }
        public int ReceiptItemsPageNumber { get; set; }
        public int ReceiptItemsPageSize { get; set; }
        public int RecoveriesPageNumber { get; set; }
        public int RecoveriesPageSize { get; set; }
        public string TaxItemsSortBy { get; set; }
        public string ReceiptItemsSortBy { get; set; }
        public string RecoveriesSortBy { get; set; }
    }
}
