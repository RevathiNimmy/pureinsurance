namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimPerilSummaryQueryBase : BaseRequestType
    {
        public int ClaimKey { get; set; }
        public bool IncludeReserveTypes { get; set; }
        public bool IncludeSalvageRecovery { get; set; }
        public bool IncludeTPRecovery { get; set; }
        public bool IncludeTotals { get; set; }
    }
}
