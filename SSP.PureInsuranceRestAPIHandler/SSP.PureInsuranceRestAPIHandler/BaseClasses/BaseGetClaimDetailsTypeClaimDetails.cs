namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimDetailsTypeClaimDetails : BaseClaimType
    {
        public string GisScreenCode { get; set; }
        public int ClaimKey { get; set; }
        public string UnderwritingYearCode { get; set; }
        public int InsuranceFolderKey { get; set; }
    }
}