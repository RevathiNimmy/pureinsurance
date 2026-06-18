namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimRIArrangementLinesRI2007RequestType : BaseRequestType
    {

        public int ArrangementKey { get; set; }

        public int ClaimKey { get; set; }
        public bool IsRecovery { get; set; }
        public bool IsRecoverySpecified { get; set; }
        public int Mode { get; set; }
        public bool ModeSpecified { get; set; }
    }
}
