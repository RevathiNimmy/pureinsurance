namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRIPropTreatiesQuery : GetRIPropTreatiesQueryBase
    {
        public int RIArrangementKey { get; set; }
        public string TreatyType { get; set; } = string.Empty;
    }
}
