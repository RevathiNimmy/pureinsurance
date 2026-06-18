namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimDetailsQuery : GetClaimDetailsQueryBase
    {
        public int ReservePageNumber { get; set; }
        public int ReservePageSize { get; set; }
        public int RecoveryPageNumber { get; set; }
        public int RecoveryPageSize { get; set; }
        public int ClaimPaymentsPageNumber { get; set; }
        public int ClaimPaymentsPageSize { get; set; }
        public int ClaimReceiptsPageNumber { get; set; }
        public int ClaimReceiptsPageSize { get; set; }
        public string ReserveSortBy { get; set; }
        public string RecoverySortBy { get; set; }
        public string ClaimPaymentsSortBy { get; set; }
        public string ClaimReceiptsSortBy { get; set; }
    }
}
