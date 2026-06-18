namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimPerilSummaryQuery : GetClaimPerilSummaryQueryBase
    {
        public int ReserveTypePageNumber { get; set; }
        public int ReserveTypePageSize { get; set; }
        public int PerilTotalsPageNumber { get; set; }
        public int PerilTotalsPageSize { get; set; }
        public int SalvageRecoveryPerilsPageNumber { get; set; }
        public int SalvageRecoveryPerilsPageSize { get; set; }
        public int TPRecoveryPerilsPageNumber { get; set; }
        public int TPRecoveryPerilsPageSize { get; set; }
        public string ReserveTypeSortBy { get; set; }
        public string PerilTotalsSortBy { get; set; }
        public string SalvageRecoveryPerilsSortBy { get; set; }
        public string TPRecoveryPerilsSortBy { get; set; }
    }
}
